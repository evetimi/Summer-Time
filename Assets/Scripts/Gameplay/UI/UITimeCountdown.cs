using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UITimeCountdown : MonoBehaviour
{
    [BoxGroup("Wave"), SerializeField] private Transform _waveTransform;
    [BoxGroup("Wave"), SerializeField] private Transform _startTimePosition;
    [BoxGroup("Wave"), SerializeField] private Transform _endTimePosition;
    [BoxGroup("Wave"), SerializeField] private UIAmountDisplay _timerDisplay;

    [BoxGroup("Warning"), SerializeField] private AudioSource _audioSource;
    [BoxGroup("Warning"), SerializeField] private GameObject _30sWarning;
    [BoxGroup("Warning"), SerializeField] private AudioClip _30sWarningClip;
    [BoxGroup("Warning"), SerializeField] private GameObject _10sWarning;
    [BoxGroup("Warning"), SerializeField] private AudioClip _10sWarningClip;

    private float _startTime;
    private float _currentTime;
    private float _warningTime;

    private bool _isCounting;

    public void OnGenerateLevelCompleted(MainGameplayController controller) {
        _isCounting = true;
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

    private void Start() {
        _isCounting = false;
        _warningTime = 30f;
    }

    private void Update() {
        if (!_isCounting) {
            return;
        }

        if (MainGameplayController.Instance) {
            SetTimer(MainGameplayController.Instance.GameTimer);
            if (MainGameplayController.Instance.GameTimer <= _warningTime) {
                if (_warningTime == 30f) {
                    _warningTime = 10f;
                    ShowWarning(_30sWarning, _30sWarningClip);
                } else {
                    _warningTime = -10f;
                    ShowWarning(_10sWarning, _10sWarningClip);
                }
            }
        }
    }

    private void ShowWarning(GameObject warning, AudioClip clip) {
        warning.SetActive(true);
        _audioSource.clip = clip;
        _audioSource.Play();
    }
}
