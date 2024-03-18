using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameItemContainer : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
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

    public void OnPointerDown(PointerEventData eventData) {
        Debug.Log("Clicked Container");
        if (Pointer.Instance) {
            Pointer.Instance.RegisterContainer(this);
        }
    }

    public void OnPointerUp(PointerEventData eventData) {
        Debug.Log("Up Container");
        if (Pointer.Instance) {
            Pointer.Instance.UnregisterContainer();
        }
    }
}
