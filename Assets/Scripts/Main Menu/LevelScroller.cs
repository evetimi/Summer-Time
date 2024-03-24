using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class LevelScroller : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [BoxGroup("References"), RequiredIn(PrefabKind.InstanceInScene), SerializeField] private ChainLevel _chain;
    [BoxGroup("References"), RequiredIn(PrefabKind.InstanceInScene), SerializeField] private Transform _levelContainer;
    [BoxGroup("References"), RequiredIn(PrefabKind.InstanceInScene), SerializeField] private Transform _levelMiddlePoint;
    [BoxGroup("References"), RequiredIn(PrefabKind.InstanceInScene), SerializeField] private OpenCloseAnimTrigger _lightUp;
    [BoxGroup("References"), RequiredIn(PrefabKind.InstanceInScene), SerializeField] private OpenCloseAnimTrigger _lightDown;
    [BoxGroup("References"), RequiredIn(PrefabKind.InstanceInScene), SerializeField] private OpenCloseAnimTrigger _buttonUp;
    [BoxGroup("References"), RequiredIn(PrefabKind.InstanceInScene), SerializeField] private OpenCloseAnimTrigger _buttonDown;

    [BoxGroup("Auto Scroll"), SerializeField] private float _scrollTimeToNextLevel = 0.2f;
    [BoxGroup("Auto Scroll"), SerializeField] private float _scrollTimeToFarLevel = 0.7f;

    private int _currentIndex;
    private int _farAmountConsidered;

    private void Start() {
        _farAmountConsidered = (int)(_scrollTimeToFarLevel / _scrollTimeToNextLevel + 1);
    }

    private void OnEnable() {
        _currentIndex = -1;

        StartCoroutine(StartedCoroutine());
    }

    private IEnumerator StartedCoroutine() {
        yield return null;

        int savedLevel = PlayerData.Level;
        ScrollTo(savedLevel);
    }

    public void ScrollNextLevel() {
        ScrollTo(_currentIndex + 1);
    }

    public void ScrollPreviousLevel() {
        ScrollTo(_currentIndex - 1);
    }

    public void ScrollUpToCurrent() {
        int savedLevel = PlayerData.Level;

        if (_currentIndex < savedLevel) {
            ScrollTo(savedLevel);
        }
    }

    public void ScrollDownToCurrent() {
        int savedLevel = PlayerData.Level;

        if (_currentIndex > savedLevel) {
            ScrollTo(savedLevel);
        }
    }

    public void ScrollTo(int levelIndex) {
        if (levelIndex == _currentIndex || (levelIndex < 0 || levelIndex >= _levelContainer.childCount)) {
            return;
        }

        if (Mathf.Abs(levelIndex - _currentIndex) < _farAmountConsidered) {
            // Scroll near
            ScrollTo(levelIndex, _scrollTimeToNextLevel);
        } else {
            // Scroll far
            ScrollTo(levelIndex, _scrollTimeToFarLevel);
        }
    }

    [Button]
    private void Test() {
        RectTransform chainRectTransform = _chain.GetComponent<RectTransform>();

        Debug.Log(chainRectTransform.rect.width);
    }

    private void ScrollTo(int levelIndex, float scrollTime) {
        Transform level = _levelContainer.GetChild(levelIndex);
        Vector3 direction = _levelMiddlePoint.position - level.position;
        Vector3 targetPosition = _levelContainer.position + direction;
        Vector3 localTargetPosition = _levelContainer.parent.InverseTransformPoint(targetPosition);

        RectTransform chainRectTransform = _chain.GetComponent<RectTransform>();

        Vector3[] corners = new Vector3[4];

        chainRectTransform.GetWorldCorners(corners);

        Vector3 bottomLeft = corners[0];
        Vector3 topLeft = corners[1];
        //Vector3 topRight = corners[2];
        //Vector3 bottomRight = corners[3];

        float chainHeight = topLeft.y - bottomLeft.y;

        Vector3 lastPosition = _levelContainer.position;
        _levelContainer.DOLocalMove(localTargetPosition, scrollTime).SetEase(Ease.InOutSine).OnUpdate(() => {
            float length = Vector3.Distance(_levelContainer.position, lastPosition);
            float chainScrollAmount = length / chainHeight;
            //Debug.Log($"{length} - {chainScrollAmount}");
            if (_levelContainer.position.y < lastPosition.y) {
                _chain.MoveDown(chainScrollAmount);
            } else {
                _chain.MoveUp(chainScrollAmount);
            }

            lastPosition = _levelContainer.position;
        });

        _currentIndex = levelIndex;

        int savedLevel = PlayerData.Level;

        if (savedLevel - _currentIndex > 1) {
            _lightUp.Open();
        } else {
            _lightUp.Close();
        }

        if (_currentIndex - savedLevel > 1) {
            _lightDown.Open();
        } else {
            _lightDown.Close();
        }

        if (_currentIndex == _levelContainer.childCount - 1) {
            _buttonUp.Close();
        } else {
            _buttonUp.Open();
        }

        if (_currentIndex == 0) {
            _buttonDown.Close();
        } else {
            _buttonDown.Open();
        }

        //DOTween.To(x => {

        //}, 0f, 1f, scrollTime).SetEase(Ease.InOutSine);


    }

    public void OnPointerDown(PointerEventData eventData) {
        Debug.Log("Down: " + name);
    }

    public void OnPointerUp(PointerEventData eventData) {
        Debug.Log("Up: " + name);
    }
}
