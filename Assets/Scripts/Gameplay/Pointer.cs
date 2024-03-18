using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Pointer : MonoBehaviourSingleton<Pointer>
{
    [BoxGroup("Setup"), SerializeField] private float _delayClick = 0.2f;
    [BoxGroup("Setup"), SerializeField] private float _requiredDragLengthToMove = 10f;

    [BoxGroup("Runtime"), ReadOnly, ShowInInspector] private GameItemContainer _chosenContainer;

    public bool Clickable { get; private set; }
    public GameItemContainer ChosenContainer => _chosenContainer;
    public Vector3 LastMousePosition { get; private set; }

    private float _delayTimer;

    private void Start() {
        _delayTimer = _delayClick;
    }

    private void Update() {
        Clickable = ValidatePointerClickable();

        CheckSwap();
    }

    /// <summary>
    /// To validate the click ability of the pointer
    /// </summary>
    private bool ValidatePointerClickable() {
        if (_delayTimer > 0f) {
            _delayTimer -= Time.deltaTime;
            return false;
        }

        if (MainGameplayController.Instance == null || !MainGameplayController.Instance.IsPlaying) {
            return false;
        }

        return true;
    }

    public void RegisterContainer(GameItemContainer container) {
        if (Clickable) {
            _chosenContainer = container;
            LastMousePosition = Input.mousePosition;
            _delayTimer = _delayClick;
        }
    }

    public void UnregisterContainer() {
        _chosenContainer = null;
        _delayTimer = _delayClick;
    }

    private void CheckSwap() {
        if (_chosenContainer != null) {
            Vector3 currentMousePosition = Input.mousePosition;

            if (currentMousePosition.x - LastMousePosition.x >= _requiredDragLengthToMove) {
                SwapWith(MainGameplayController.Instance.GetGameItemContainer(_chosenContainer.GridPosition.x + 1, _chosenContainer.GridPosition.y));
            } else if (LastMousePosition.x - currentMousePosition.x >= _requiredDragLengthToMove) {
                SwapWith(MainGameplayController.Instance.GetGameItemContainer(_chosenContainer.GridPosition.x - 1, _chosenContainer.GridPosition.y));
            } else if (currentMousePosition.y - LastMousePosition.y >= _requiredDragLengthToMove) {
                SwapWith(MainGameplayController.Instance.GetGameItemContainer(_chosenContainer.GridPosition.x, _chosenContainer.GridPosition.y + 1));
            } else if (LastMousePosition.y - currentMousePosition.y >= _requiredDragLengthToMove) {
                SwapWith(MainGameplayController.Instance.GetGameItemContainer(_chosenContainer.GridPosition.x, _chosenContainer.GridPosition.y - 1));
            }
        }
    }

    private void SwapWith(GameItemContainer container) {
        //MouseChosen = false;
        _chosenContainer.ContainItem.SwapWith(container);
        _chosenContainer = null;
    }
}
