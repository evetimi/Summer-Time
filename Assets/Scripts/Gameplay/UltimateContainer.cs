using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UltimateContainer : MonoBehaviour
{
    [BoxGroup("Setup"), SerializeField] private int _requiredEnergy = 100;
    [BoxGroup("Setup"), SerializeField] private float _ultimateDuration = 5f;
    [BoxGroup("Setup"), SerializeField] private ParticleSystem _getEnergyEffect;
    [BoxGroup("Setup"), SerializeField] private ParticleSystem _readyEffect;
    [BoxGroup("Setup"), SerializeField] private ParticleSystem _useEffect;

    [BoxGroup("Visual"), SerializeField] private Transform _waveTransform;
    [BoxGroup("Visual"), SerializeField] private Transform _emptyWavePosition;
    [BoxGroup("Visual"), SerializeField] private Transform _fullWavePosition;

    [BoxGroup("Events"), SerializeField] public UnityEvent OnUltimateUsed;
    [BoxGroup("Events"), SerializeField] public UnityEvent OnUltimateFinished;

    [BoxGroup("Runtime"), ReadOnly, ShowInInspector] private int _currentEnergy;

    private float _ultimateTimer;

    public bool IsUsingUltimate { get; private set; }

    public void SetUltimate() {
        _requiredEnergy = 100;
        _ultimateDuration = 5f;
        _currentEnergy = 0;
    }

    private void Update() {
        if (IsUsingUltimate) {
            _ultimateTimer -= Time.deltaTime;
            if (_ultimateTimer <= 0f) {
                StopUltimate();
            }
        }
    }

    private void PlayEffect(ParticleSystem effect) {
        if (effect != null) {
            effect.Play();
        }
    }

    private void StopEffect(ParticleSystem effect) {
        if (effect != null) {
            effect.Stop();
        }
    }

    public void GetEnergy(GameItem sourceEnergy, int amount) {
        if (_currentEnergy >= _requiredEnergy) {
            return;
        }

        _currentEnergy += amount;

        if (_waveTransform != null && _emptyWavePosition != null && _fullWavePosition != null) {
            float rate = (float)_currentEnergy / _requiredEnergy;
            _waveTransform.position = Vector3.Lerp(_emptyWavePosition.position, _fullWavePosition.position, rate);
        }

        PlayEffect(_getEnergyEffect);
        if (_currentEnergy > _requiredEnergy) {
            _currentEnergy = _requiredEnergy;
            PlayEffect(_readyEffect);
        }
    }

    public void UseUltimate() {
        if (_currentEnergy >= _requiredEnergy) {
            StopEffect(_readyEffect);
            PlayEffect(_useEffect);
            _ultimateTimer = _ultimateDuration;
            IsUsingUltimate = true;

            if (Pointer.Instance) {
                Pointer.Instance.SetFinishGameItemOnHover(true);
            }

            OnUltimateUsed?.Invoke();
        }
    }

    public void StopUltimate() {
        _currentEnergy = 0;
        IsUsingUltimate = false;
        StopEffect(_useEffect);

        if (Pointer.Instance) {
            Pointer.Instance.SetFinishGameItemOnHover(false);
        }

        OnUltimateFinished?.Invoke();
    }
}
