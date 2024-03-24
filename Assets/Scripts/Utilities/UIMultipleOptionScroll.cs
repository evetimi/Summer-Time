using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIMultipleOptionScroll : MonoBehaviour
{
    [BoxGroup("Setup"), SerializeField] private int _startIndex;
    [BoxGroup("Setup"), SerializeField] private string[] _options;
    [BoxGroup("Setup"), SerializeField] private TMP_Text[] _displayTexts;
    [BoxGroup("Setup"), SerializeField] private OpenCloseAnimTrigger _leftButtonTrigger;
    [BoxGroup("Setup"), SerializeField] private OpenCloseAnimTrigger _rightButtonTrigger;

    [BoxGroup("Swipe Effect"), SerializeField] private float _spacingBetweenDisplayText = 100f;
    [BoxGroup("Swipe Effect"), SerializeField] private float _moveDuration = 0.2f;

    [BoxGroup("Runtime"), ReadOnly, ShowInInspector] private int _currentIndex;
    [BoxGroup("Runtime"), ReadOnly, ShowInInspector] private int _displayTextIndex;

    private Vector3[] _3scrollPositions;

    private void OnValidate() {
        ValidateStartIndex();
    }

    private void ValidateStartIndex() {
        if (_startIndex < 0) {
            _startIndex = 0;
        } else if (_startIndex >= _options.Length) {
            _startIndex = _options.Length - 1;
        }
    }

    private void OnEnable() {
        if (!ValidateDisplayTexts()) {
            return;
        }

        Get3ScrollPositions();
        ValidateStartIndex();

        _displayTexts[0].text = _options[_startIndex];
        _displayTexts[1].transform.position = _3scrollPositions[2];
        _displayTextIndex = 0;

        _currentIndex = _startIndex;

        SetButtonVisible();
    }

    public bool ValidateDisplayTexts() {
        return _displayTexts != null && _displayTexts.Length == 2 && _displayTexts[0] != null && _displayTexts[1] != null;
    }

    public Vector3[] Get3ScrollPositions() {
        if (_3scrollPositions == null || _3scrollPositions.Length != 3) {
            _3scrollPositions = new Vector3[3];
        }

        _3scrollPositions[0] = new(transform.position.x - _spacingBetweenDisplayText, transform.position.y, transform.position.z);
        _3scrollPositions[1] = new(transform.position.x, transform.position.y, transform.position.z);
        _3scrollPositions[2] = new(transform.position.x + _spacingBetweenDisplayText, transform.position.y, transform.position.z);

        return _3scrollPositions;
    }

    public void SwipeRight() {
        if (!ValidateDisplayTexts()) {
            return;
        }

        if (_currentIndex + 1 >= _options.Length) {
            return;
        }

        _currentIndex++;

        Get3ScrollPositions();

        _displayTexts[_displayTextIndex].transform.position = _3scrollPositions[1];
        _displayTexts[_displayTextIndex].transform.DOKill();
        _displayTexts[_displayTextIndex].transform.DOMove(_3scrollPositions[0], _moveDuration).SetEase(Ease.InOutSine);

        _displayTextIndex = 1 - _displayTextIndex;

        _displayTexts[_displayTextIndex].text = _options[_currentIndex];
        _displayTexts[_displayTextIndex].transform.position = _3scrollPositions[2];
        _displayTexts[_displayTextIndex].transform.DOKill();
        _displayTexts[_displayTextIndex].transform.DOMove(_3scrollPositions[1], _moveDuration).SetEase(Ease.InOutSine);

        SetButtonVisible();
    }

    public void SwipeLeft() {
        if (!ValidateDisplayTexts()) {
            return;
        }

        if (_currentIndex - 1 < 0) {
            return;
        }

        _currentIndex--;

        Get3ScrollPositions();

        _displayTexts[_displayTextIndex].transform.position = _3scrollPositions[1];
        _displayTexts[_displayTextIndex].transform.DOKill();
        _displayTexts[_displayTextIndex].transform.DOMove(_3scrollPositions[2], _moveDuration).SetEase(Ease.InOutSine);

        _displayTextIndex = 1 - _displayTextIndex;

        _displayTexts[_displayTextIndex].text = _options[_currentIndex];
        _displayTexts[_displayTextIndex].transform.position = _3scrollPositions[0];
        _displayTexts[_displayTextIndex].transform.DOKill();
        _displayTexts[_displayTextIndex].transform.DOMove(_3scrollPositions[1], _moveDuration).SetEase(Ease.InOutSine);

        SetButtonVisible();
    }

    public void SetButtonVisible() {
        if (_leftButtonTrigger) {
            if (_currentIndex == 0) {
                _leftButtonTrigger.Close();
            } else {
                _leftButtonTrigger.Open();
            }
        }

        if (_rightButtonTrigger) {
            if (_currentIndex == _options.Length - 1) {
                _rightButtonTrigger.Close();
            } else {
                _rightButtonTrigger.Open();
            }
        }
    }
}
