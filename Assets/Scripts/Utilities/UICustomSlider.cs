using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICustomSlider : MonoBehaviour
{
    [BoxGroup("Setup"), SerializeField] private Slider _slider;
    [BoxGroup("Setup"), SerializeField] private TMP_Text _displayText;
    [BoxGroup("Setup"), SerializeField] private OpenCloseAnimTrigger _leftButtonTrigger;
    [BoxGroup("Setup"), SerializeField] private OpenCloseAnimTrigger _rightButtonTrigger;
    [BoxGroup("Setup"), SerializeField] private int _startValue;
    [BoxGroup("Setup"), SerializeField] private int _maxValue;
    [BoxGroup("Setup"), SerializeField] private float _slideTime = 0.1f;

    [BoxGroup("Runtime"), ReadOnly, ShowInInspector] private int _currentValue;

    private void OnValidate() {
        if (_startValue < 0) {
            _startValue = 0;
        } else if (_startValue > _maxValue) {
            _startValue = _maxValue;
        }
    }

    private void OnEnable() {
        _slider.value = (float)_startValue / _maxValue;
        _currentValue = _startValue;
        SetVisible();
    }

    public void SlideRight() {
        if (_slider == null) {
            return;
        }

        if (_currentValue >= _maxValue) {
            return;
        }

        _currentValue++;
        float rate = (float)_currentValue / _maxValue;

        _slider.DOKill();
        _slider.DOValue(rate, _slideTime).SetEase(Ease.InOutSine);

        SetVisible();
    }

    public void SlideLeft() {
        if (_slider == null) {
            return;
        }

        if (_currentValue <= 0) {
            return;
        }

        _currentValue--;
        float rate = (float)_currentValue / _maxValue;

        _slider.DOKill();
        _slider.DOValue(rate, _slideTime).SetEase(Ease.InOutSine);

        SetVisible();
    }

    public void SetVisible() {
        if (_displayText) {
            if (_maxValue == 0) {
                _displayText.text = "0%";
            } else {
                _displayText.text = $"{(int)((float)_currentValue / _maxValue * 100)}%";
            }
        }

        if (_leftButtonTrigger) {
            if (_currentValue == 0) {
                _leftButtonTrigger.Close();
            } else {
                _leftButtonTrigger.Open();
            }
        }

        if (_rightButtonTrigger) {
            if (_currentValue == _maxValue) {
                _rightButtonTrigger.Close();
            } else {
                _rightButtonTrigger.Open();
            }
        }
    }
}
