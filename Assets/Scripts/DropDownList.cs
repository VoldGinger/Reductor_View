using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Object = UnityEngine.Object;


public class DropDownList
{
    public event Action<string> OnDetailButtonClicked;
    public event Action OnListClosed;
    public event Action OnListOpened;
    public event Action OnButtonClickedDouble;

    private readonly GameObject _dropButtonPrefab;
    private readonly Transform _listHolder;
    private readonly Color _inactiveColor;
    private readonly Color _activeColor;
    private readonly float _listMovementSpeed;
    private Button _activeButton;


    public DropDownList(List<Detail> details, GameObject dropButtonPrefab, Transform listHolder, Color inactiveColor, Color activeColor, float listMovementSpeed)
    {
        _dropButtonPrefab = dropButtonPrefab;
        _listHolder = listHolder;
        _inactiveColor = inactiveColor;
        _activeColor = activeColor;
        _listMovementSpeed = listMovementSpeed;

        _mainButtonRect = _listHolder.GetChild(0) as RectTransform;
        _buttonTransforms = new List<RectTransform>();
        _mainButton = _listHolder.GetComponentInChildren<Button>();

        _mainButton.onClick.AddListener(SetListState);
        InstantiateDetailButtons(details);
    }


    private readonly List<RectTransform> _buttonTransforms;
    private readonly Button _mainButton;
    private readonly RectTransform _mainButtonRect;

    private bool _isListOpened;



    private void InstantiateDetailButtons(List<Detail> details)
    {
        foreach (var detail in details.OrderBy(x => x.DetailOrder))
        {
            var buttonGo = Object.Instantiate(_dropButtonPrefab, _mainButtonRect.position,
                quaternion.identity, _listHolder);
            var textGUI = buttonGo.GetComponentInChildren<TextMeshProUGUI>();
            textGUI.text = detail.name;
            _buttonTransforms.Add(buttonGo.transform as RectTransform);
            Button button = buttonGo.GetComponent<Button>();
            button.onClick.AddListener(() => HandleButton(button, textGUI.text));
        }
        HideButtons();
    }

    // тут много делаю GetComponent операций. Понимаю что это накладно. Чтобы избежать этого сделал бы словарь
    // с кнопками где ключем был бы текст кнопки, а значением класс со всем необходимым (изображением, самой кнопкой)
    private void HandleButton(Button button, string buttonText)
    {
        var buttonImage = button.GetComponent<Image>();
        if (button == _activeButton)
        {
            buttonImage.color = _inactiveColor;
            _activeButton = null;
            OnButtonClickedDouble?.Invoke();
            return;
        }
        if (_activeButton)
        {
            _activeButton.GetComponent<Image>().color = _inactiveColor;
        }
        _activeButton = button;
        buttonImage.color = _activeColor;
        OnDetailButtonClicked?.Invoke(buttonText);
    }

    private void HideButtons()
    {
        foreach (var buttonTransform in _buttonTransforms)
        {
            buttonTransform.gameObject.SetActive(false);
        }
    }

    private void ShowButtons()
    {

        foreach (var buttonTransform in _buttonTransforms)
        {
            buttonTransform.gameObject.SetActive(true);
        }
    }
    private void OpenList()
    {
        OnListOpened?.Invoke();
        _isListOpened = true;
        ShowButtons();
        float currentYCoord = _mainButtonRect.localPosition.y - _mainButtonRect.rect.height / 2;
        foreach (var t in _buttonTransforms)
        {
            t.DOLocalMoveY(currentYCoord - t.rect.height / 2, _listMovementSpeed);
            currentYCoord -= t.rect.height;
        }
    }

    private void CloseList()
    {
        OnListClosed?.Invoke();
        if (_activeButton != null) _activeButton.GetComponent<Image>().color = _inactiveColor;
        _activeButton = null;
        _isListOpened = false;
        foreach (var t in _buttonTransforms)
        {
            t.DOMoveY(_mainButtonRect.position.y, _listMovementSpeed);
        }

    }


    private void SetListState()
    {
        if (_isListOpened)
            CloseList();
        else
            OpenList();
    }




}
