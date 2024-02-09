using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public static Vector3 LastMousePosition;
    public static int UnreadyItemAmount { get; private set; }
    /// <summary>
    /// first int: x position
    /// second int: y position
    /// </summary>
    public static Action<int, int> OnStartMoving;

    /// <summary>
    /// Parameters are the GameItems to swap. Called they are done
    /// </summary>
    public static Action<GameItem, GameItem> OnItemsSwapped;

    [BoxGroup("Setup"), SerializeField] private Image _image;
    [BoxGroup("Setup"), SerializeField] private float _requiredDragLengthToMove = 10f;

    [BoxGroup("Runtime"), SerializeField, Tooltip("If Object ID = -1, it is not a normal item")] private int _itemId = -1;
    [BoxGroup("Runtime"), SerializeField] private GameItemContainer _registeredContainer;
    [BoxGroup("Runtime"), SerializeField] private Vector2Int _gridPosition;
    [BoxGroup("Runtime"), SerializeField] private bool _isWorking;

    public bool IsWorking {
        get => _isWorking;
        set => _isWorking = value;
    }

    public int ItemId => _itemId;
    public GameItemContainer RegisteredContainer => _registeredContainer;
    public Vector2Int GridPosition => _gridPosition;
    public bool IsMoving { get; private set; }
    public bool MouseChosen { get; private set; }

    public void SetItem(Sprite itemSprite, int itemId) {
        _image.sprite = itemSprite;
        _itemId = itemId;
    }

    /// <summary>
    /// This WON'T save the GameItemContainer, just modify the position, usually used for Instantiating the NEW GameItem, this also unregister the previous GameItemContainer this one was in.
    /// </summary>
    public void RegisterPosition(Vector2Int gridPosition, bool unRegisterPrevious) {
        if (unRegisterPrevious && _registeredContainer != null) {
            _registeredContainer.ContainItem = null;
        }

        _gridPosition = gridPosition;
    }

    /// <summary>
    /// This will save the GameItemContainer to this GameItem and also modify the _gridPosition, this also unregister the previous GameItemContainer this one was in.
    /// </summary>
    public void RegisterContainer(GameItemContainer container, bool unRegisterPrevious) {
        if (unRegisterPrevious && _registeredContainer != null) {
            _registeredContainer.ContainItem = null;
        }

        _registeredContainer = container;
        container.ContainItem = this;
        _gridPosition = container.GridPosition;
    }

    private void Update() {
        if (!_isWorking || IsMoving) {
            return;
        }

        if (CanGoBelow(out int x, out int y)) {
            UnreadyItemAmount++;
            GoTo(x, y);
        } else if (MouseChosen) {
            Vector3 currentMousePosition = Input.mousePosition;

            if (currentMousePosition.x - LastMousePosition.x >= _requiredDragLengthToMove) {
                SwapWith(MainGameplayController.Instance.GetGameItemContainer(_gridPosition.x + 1, _gridPosition.y));
            } else if (LastMousePosition.x - currentMousePosition.x >= _requiredDragLengthToMove) {
                SwapWith(MainGameplayController.Instance.GetGameItemContainer(_gridPosition.x - 1, _gridPosition.y));
            } else if (currentMousePosition.y - LastMousePosition.y >= _requiredDragLengthToMove) {
                SwapWith(MainGameplayController.Instance.GetGameItemContainer(_gridPosition.x, _gridPosition.y + 1));
            } else if (LastMousePosition.y - currentMousePosition.y >= _requiredDragLengthToMove) {
                SwapWith(MainGameplayController.Instance.GetGameItemContainer(_gridPosition.x, _gridPosition.y - 1));
            }
        }
    }

    private bool CanGoBelow(out int x, out int y) {
        x = _gridPosition.x;
        y = _gridPosition.y - 1;

        MainGameplayController controller = MainGameplayController.Instance;
        GameItemContainer belowContainer = controller.GetGameItemContainer(x, y);

        if (belowContainer == null) {
            // the last one already
            return false;
        }

        GameItem item = belowContainer.ContainItem;

        return item == null || item == this; // If the item is this one, means that it is registered here but not updated itself yet, so it will update.
    }

    private void GoTo(int x, int y) {
        IsMoving = true;

        MainGameplayController controller = MainGameplayController.Instance;
        GameItemContainer belowContainer = controller.GetGameItemContainer(x, y);
        Vector3 targetPosition = belowContainer.transform.position;

        RegisterContainer(belowContainer, true);

        OnStartMoving?.Invoke(x, y);
        transform.DOMove(targetPosition, controller.ItemMoveTime).SetEase(Ease.Linear).OnComplete(() => {
            if (CanGoBelow(out int x1, out int y1)) {
                GoTo(x1, y1);
            } else {
                UnreadyItemAmount--;
                IsMoving = false;
            }
        });
    }

    public void FinishItem() {

    }

    public void OnPointerDown(PointerEventData eventData) {
        Debug.Log($"Down: {name}");
        if (!MainGameplayController.Instance.CanClickPointer || !MainGameplayController.Instance.CanDragItem) {
            return;
        }

        LastMousePosition = Input.mousePosition;
        MouseChosen = true;
    }

    public void OnPointerUp(PointerEventData eventData) {
        Debug.Log($"Up: {name}");

        MouseChosen = false;
    }

    private void SwapWith(GameItemContainer container) {
        MouseChosen = false;

        if (container == null || container.ContainItem == null) {
            // Failed to swap
        } else {
            // success
            Swap(this, container.ContainItem, true);
        }
    }

    public async static void Swap(GameItem item1, GameItem item2, bool callEvent) {
        MainGameplayController controller = MainGameplayController.Instance;

        Vector3 item1Position = item1.transform.position;
        Vector3 item2Position = item2.transform.position;

        Debug.Log($"Before: {item1._registeredContainer} - {item2._registeredContainer}");

        GameItemContainer tmp = item1._registeredContainer;
        item1.RegisterContainer(item2._registeredContainer, false);
        item2.RegisterContainer(tmp, false);

        Debug.Log($"After: {item1._registeredContainer} - {item2._registeredContainer}");

        var tasks = new List<Task>();

        tasks.Add(item1.transform.DOMove(item2Position, controller.ItemMoveTime).SetEase(Ease.Linear).AsyncWaitForCompletion());
        tasks.Add(item2.transform.DOMove(item1Position, controller.ItemMoveTime).SetEase(Ease.Linear).AsyncWaitForCompletion());

        await Task.WhenAll(tasks);

        if (callEvent) {
            OnItemsSwapped?.Invoke(item1, item2);
        }
    }
}
