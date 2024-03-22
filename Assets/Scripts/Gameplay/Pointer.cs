using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Pointer : MonoBehaviourSingleton<Pointer>
{
    [BoxGroup("Setup"), SerializeField] private ParticleSystem _dragEffect;
    [BoxGroup("Setup"), SerializeField] private EffectSideProperty _leftRotation;
    [BoxGroup("Setup"), SerializeField] private EffectSideProperty _rightRotation;
    [BoxGroup("Setup"), SerializeField] private EffectSideProperty _upRotation;
    [BoxGroup("Setup"), SerializeField] private EffectSideProperty _downRotation;
    [BoxGroup("Setup"), SerializeField] private float _delayClick = 0.2f;
    [BoxGroup("Setup"), SerializeField] private float _requiredDragLengthToMove = 10f;

    [BoxGroup("Runtime"), ReadOnly, ShowInInspector] private GameItemContainer _chosenContainer;

    [System.Serializable]
    private struct EffectSideProperty {
        public Vector3 offsetPosition;
        public Vector3 rotation;
    }

    public bool Clickable { get; private set; }
    public bool FinishGameItemOnHover { get; private set; }
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

        if (MainGameplayController.Instance == null || !MainGameplayController.Instance.IsPlaying || !MainGameplayController.Instance.CanDragItem) {
            return false;
        }

        return true;
    }

    [Button]
    public void SetFinishGameItemOnHover(bool enabled) {
        FinishGameItemOnHover = enabled;
    }

    public void HoverContainer(GameItemContainer container) {
        if (FinishGameItemOnHover && container && container.ContainItem && MainGameplayController.Instance != null && MainGameplayController.Instance.IsPlaying) {
            MainGameplayController.Instance.FinishGameItem(container.ContainItem);
        }
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
                SwapWith(MainGameplayController.Instance.GetGameItemContainer(_chosenContainer.GridPosition.x + 1, _chosenContainer.GridPosition.y), _rightRotation);
            } else if (LastMousePosition.x - currentMousePosition.x >= _requiredDragLengthToMove) {
                SwapWith(MainGameplayController.Instance.GetGameItemContainer(_chosenContainer.GridPosition.x - 1, _chosenContainer.GridPosition.y), _leftRotation);
            } else if (currentMousePosition.y - LastMousePosition.y >= _requiredDragLengthToMove) {
                SwapWith(MainGameplayController.Instance.GetGameItemContainer(_chosenContainer.GridPosition.x, _chosenContainer.GridPosition.y + 1), _upRotation);
            } else if (LastMousePosition.y - currentMousePosition.y >= _requiredDragLengthToMove) {
                SwapWith(MainGameplayController.Instance.GetGameItemContainer(_chosenContainer.GridPosition.x, _chosenContainer.GridPosition.y - 1), _downRotation);
            }
        }
    }

    private void SwapWith(GameItemContainer container, EffectSideProperty effectProperty) {
        //MouseChosen = false;
        if (_dragEffect) {
            _dragEffect.transform.position = _chosenContainer.transform.position + effectProperty.offsetPosition;
            _dragEffect.transform.localRotation = Quaternion.Euler(effectProperty.rotation);
            _dragEffect.Play();
        }

        _chosenContainer.ContainItem.SwapWith(container);
        _chosenContainer = null;
    }
}
