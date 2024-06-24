using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChainLevel : MonoBehaviour
{
    [BoxGroup("TEST"), SerializeField] private bool _testSimulate;
    [BoxGroup("TEST"), SerializeField] private float _testAmount = 0.07f;

    [BoxGroup("Properties"), SerializeField] private float _gearRotateMultiplies = 100f;
    [BoxGroup("Properties"), SerializeField] private RawImage[] _chains;
    [BoxGroup("Properties"), SerializeField] private Transform[] _gearsRollLeft;
    [BoxGroup("Properties"), SerializeField] private Transform[] _gearsRollRight;

    [BoxGroup("Sound Effects"), SerializeField] private AudioSource _gearRollAudioSource;

    private float _currentMove;

    private void Start() {
        _currentMove = 0f;
    }

    private void Update() {
        if (_testSimulate) {
            if (_testAmount > 0f) {
                MoveDown(_testAmount);
            } else {
                MoveUp(_testAmount);
            }
        }
    }

    private void MoveChain(float amount) {
        foreach (RawImage chain in _chains) {
            Rect rect = chain.uvRect;
            rect.y += amount;
            chain.uvRect = rect;
        }

        foreach (var gear in _gearsRollLeft) {
            gear.Rotate(Vector3.forward, amount * _gearRotateMultiplies);
        }

        foreach (var gear in _gearsRollRight) {
            gear.Rotate(Vector3.forward, -amount * _gearRotateMultiplies);
        }

        if (_gearRollAudioSource) {
            _gearRollAudioSource.Play();
        }
    }

    public void MoveUp(float amount) {
        amount = Mathf.Abs(amount);

        MoveChain(-amount);
    }

    public void MoveDown(float amount) {
        amount = Mathf.Abs(amount);

        MoveChain(amount);
    }
}
