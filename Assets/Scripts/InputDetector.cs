using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputDetector : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public event Action OnDragBegan;
    public event Action OnDragEnded;


    public void OnPointerDown(PointerEventData eventData)
    {
        OnDragBegan?.Invoke();
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        OnDragEnded?.Invoke();
    }
}
