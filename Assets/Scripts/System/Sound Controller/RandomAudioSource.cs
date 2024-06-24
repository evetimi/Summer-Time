using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RandomAudioSource : MonoBehaviour
{
    [SerializeField] private AudioClip[] randomClips;

    private AudioSource audiosource;

    public AudioSource ActualAudioSource { get { return audiosource; } }

    private void Awake() {
        audiosource = GetComponent<AudioSource>();
    }

    public void PlayRandom() {
        if (randomClips == null || randomClips.Length == 0) {
            return;
        }

        AudioClip clip = randomClips[Random.Range(0, randomClips.Length)];
        audiosource.clip = clip;

        audiosource.Play();
    }
}
