using System;
using DG.Tweening;
using UnityEngine;
public class CameraMovement : MonoBehaviour
{

    //Camera is applying rotation around this Transform
    [SerializeField] private Transform _mainTarget;
    [SerializeField] private float _moveSensitivity;
    [SerializeField] private float _scrollSensitivity;
    [SerializeField] private float _focusDuration;
    [SerializeField] private float _minCameraDistance;
    [SerializeField] private float _maxCameraDistance;
    [SerializeField] private InputDetector _inputDetector;

    private Camera _camera => Camera.main;
    private Transform _cameraHolder;

    private Transform _currentTarget;
    private Renderer _currentTargetRenderer;
    private bool _isRotated;
    private Vector3 _previousPosition;
    private float _distanceToTarget;
    private Vector3 _deltaToTarget;
    private float _startZDistance;
    private bool _canRotate;
    private Transform _cameraTransform;


    public void SetMainTarget(bool detailIsOpened)
    {
        _currentTarget = _mainTarget;
        _currentTargetRenderer = null;
        _canRotate = false;
        _cameraHolder.DOMove(_mainTarget.position, _focusDuration).onComplete += () => _canRotate = true;
        
        if (detailIsOpened)
            SetDistance(-1.638584f);
        else
            SetDistance(_startZDistance);


    }
    public void SetTarget(Transform target)
    {
        _currentTarget = target;
        _currentTargetRenderer = target.GetComponent<Renderer>();
        _canRotate = false;
        _cameraHolder.DOMove(_currentTargetRenderer.bounds.center, _focusDuration).onComplete += () => _canRotate = true;
        SetDistance(_maxCameraDistance);
    }
    public void SetDistance(float distance)
    {
        _cameraTransform.DOLocalMoveZ(distance, _focusDuration);
    }

    private void Start()
    {
        _cameraTransform = _camera.transform;
        _canRotate = true;
        _inputDetector.OnDragBegan += BeginRotation;
        _inputDetector.OnDragEnded += EndRotation;
        _cameraHolder = _cameraTransform.parent;
        _cameraHolder.position = _mainTarget.position;
        _startZDistance = _cameraTransform.localPosition.z;
        _currentTarget = _mainTarget;
        _currentTargetRenderer = _currentTarget.GetComponent<Renderer>();
    }
    private void Update()
    {
        var position = _cameraTransform.localPosition;
        position += Vector3.forward * Input.mouseScrollDelta.y * _scrollSensitivity * Time.deltaTime;
        position.z = Mathf.Clamp(position.z, _minCameraDistance, _maxCameraDistance);
        _cameraTransform.localPosition = position;

        if (!_isRotated) return;

        Vector3 newPosition = _camera.ScreenToViewportPoint(Input.mousePosition);
        Vector3 delta = newPosition - _previousPosition;
        float rotationAroundYAxis = -delta.x * _moveSensitivity;
        float rotationAroundXAxis = delta.y * _moveSensitivity;


        _cameraHolder.Rotate(new Vector3(1, 0, 0), rotationAroundXAxis);
        _cameraHolder.Rotate(new Vector3(0, 1, 0), rotationAroundYAxis, Space.World);

        _previousPosition = newPosition;
    }
    private void BeginRotation()
    {
        _previousPosition = _camera.ScreenToViewportPoint(Input.mousePosition);
        _isRotated = true;
    }



    private void EndRotation()
    {
        _isRotated = false;
    }





}
