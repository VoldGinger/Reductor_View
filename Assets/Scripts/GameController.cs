using System;
using Services;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] private CameraMovement _cameraMovement;
    [SerializeField] private Button _backButton;
    [Header("Details")]
    [SerializeField] private Transform _mainDetailTransform;
    [SerializeField] private float _detailsSpacing;
    [SerializeField] private Material _basicMaterial;
    [SerializeField] private Material _transparentMaterial;
    [SerializeField] private float _detailMovementSpeed;

    [Header("List")]
    [SerializeField] private GameObject _dropButtonPrefab;
    [SerializeField] private Transform _listHolder;
    [SerializeField] private Color _activeColor;
    [SerializeField] private Color _inactiveColor;
    [SerializeField] private float _listMovementSpeed;

    private DropDownList _detailList;
    private DetailController _detailController;
    private bool _detailIsOpened;

    private void Awake()
    {
        _backButton.onClick.AddListener(BackToMenu);
        _detailController = new DetailController(_mainDetailTransform, _detailsSpacing, _basicMaterial, _transparentMaterial, _detailMovementSpeed);
        _detailList = new DropDownList(_detailController.Details, _dropButtonPrefab, _listHolder, _inactiveColor, _activeColor, _listMovementSpeed);
        _detailList.OnListOpened += OpenList;
        _detailList.OnListClosed += CloseList;

    }
    private void OpenList()
    {
        _detailController.ExplodeDetails();
        _cameraMovement.SetDistance(-1.638584f);
        _detailIsOpened = true;
    }
    private void CloseList()
    {

        _detailController.CloseDetails();
        _detailIsOpened = false;
        FocusOnMainDetail();
    }
    private void Start()
    {
        _detailController.ShowAllDetails();


        _detailList.OnDetailButtonClicked += FocusOnDetail;
        _detailList.OnButtonClickedDouble += FocusOnMainDetail;
    }
    private void FocusOnMainDetail()
    {
        _cameraMovement.SetMainTarget(_detailIsOpened);
        _detailController.ShowAllDetails();
    }
    private void FocusOnDetail(string detailName)
    {
        var detail = _detailController.GetDetailByName(detailName);
        _cameraMovement.SetTarget(detail.transform);
        _detailController.ShowComponentSolo(detail);
    }

    private void OnDestroy()
    {
        _detailList.OnListOpened -= OpenList;
        _detailList.OnListClosed -= CloseList;
        _detailList.OnButtonClickedDouble -= FocusOnMainDetail;

        _backButton.onClick.RemoveListener(BackToMenu);
    }

    private void BackToMenu()
    {
        ServiceLocator.Instance.GetService<SceneLoader>().LoadScene("Menu Scene");
    }



}
