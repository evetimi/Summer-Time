using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TsunamiEvent : MonoBehaviour
{
    [SerializeField] private ParticleSystem _effect;
    [SerializeField] private Animator _anim;

    public void SpawnTsunami() {
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
