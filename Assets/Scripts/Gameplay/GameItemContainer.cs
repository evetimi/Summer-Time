using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameItemContainer : MonoBehaviour
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
}
