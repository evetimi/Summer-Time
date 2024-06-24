using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TsunamiEvent : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private ParticleSystem _effect;
    [SerializeField] private Animator _anim;

    public void SpawnTsunami() {
        if (_audioSource) {
            _audioSource.Play();
        }

        if (_effect) {
            _effect.Play();
        }

        if (_anim) {
            _anim.SetTrigger("play");
        }
    }

    public void DoEventEffect() {
        if (MainGameplayController.Instance) {
            MainGameplayController.Instance.ImmediatelyDestroyAllGameItem();
        }
    }
}
