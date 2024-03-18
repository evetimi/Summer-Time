using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UITimeCountdown : MonoBehaviour
{
    [SerializeField] private Transform _waveTransform;
    [SerializeField] private Transform _startTimePosition;
    [SerializeField] private Transform _endTimePosition;

    [SerializeField] private UIAmountDisplay _timerDisplay;

    private float _startTime;
    private float _currentTime;

    public void OnGenerateLevelCompleted(MainGameplayController controller) {
        SetStartTimer(controller.GameTimer);
    }

    public void SetStartTimer(float startTime) {
        _startTime = startTime;
    }

    public void SetTimer(float currentTimer) {
        _currentTime = currentTimer;

        if (_startTime <= 0f) {
            return;
        }

        if (_waveTransform != null && _startTimePosition != null && _endTimePosition != null) {
            float rate = 1f - _currentTime / _startTime;
            _waveTransform.position = Vector3.Lerp(_startTimePosition.position, _endTimePosition.position, rate);
        }

        if (_timerDisplay) {
            int minute = (int)(_currentTime / 60f);
            _timerDisplay.SetAmount(minute, (int)(_currentTime % 60f));
        }
    }

    private void Update() {
        if (MainGameplayController.Instance) {
            SetTimer(MainGameplayController.Instance.GameTimer);
        }
    }
}
