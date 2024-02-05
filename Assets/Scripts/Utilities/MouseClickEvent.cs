using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MouseClickEvent : MonoBehaviour, IPointerClickHandler
{
    public UnityEvent OnMouseClick;

    public void OnPointerClick(PointerEventData eventData) {
        OnMouseDown();
    }

    private void OnMouseDown() {
        Debug.Log("Clicked: " + gameObject.name);
        OnMouseClick?.Invoke();
    }
}
