using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameItemContainer : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Animator _anim;
    [SerializeField] private Vector2Int _gridPosition;
    [SerializeField] private GameItem _containItem;

    public Vector2Int GridPosition {
        get => _gridPosition;
        set => _gridPosition = value;
    }

    public GameItem ContainItem {
        get => _containItem;
        set => _containItem = value;
    }

    public void DestroyItem() {
        if (_containItem != null) {
            _containItem.FinishItem();
        }
    }

    private void SetAnim(string name, bool value) {
        if (_anim != null) {
            _anim.SetBool(name, value);
        }
    }

    public void OnPointerDown(PointerEventData eventData) {
        Debug.Log("Clicked Container");
        if (Pointer.Instance) {
            Pointer.Instance.RegisterContainer(this);
            SetAnim("clicked", true);
        }
    }

    public void OnPointerUp(PointerEventData eventData) {
        Debug.Log("Up Container");
        if (Pointer.Instance) {
            Pointer.Instance.UnregisterContainer();
            SetAnim("clicked", false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        SetAnim("hover", true);
        if (Pointer.Instance) {
            Pointer.Instance.HoverContainer(this);
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        SetAnim("hover", false);
    }
}
