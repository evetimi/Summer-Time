using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuMusic : MonoBehaviour
{
    [SerializeField] private RandomAudioSource _musicRandomAudioSource;
    [SerializeField] private float _startDelay = 5f;
    [SerializeField] private float _checkNextMusicDelay = 10f;

    private float _nextMusicDelayTimer;

    private void Start() {
        IEnumerator enumerator() {
            yield return new WaitForSeconds(_startDelay);
            PlayRandomMusic();
        }


        StartCoroutine(enumerator());
        _nextMusicDelayTimer = _checkNextMusicDelay + _startDelay;
    }

    private void Update() {
        _nextMusicDelayTimer -= Time.deltaTime;
        if (_nextMusicDelayTimer < 0f) {
            _nextMusicDelayTimer = _checkNextMusicDelay;
            if (!_musicRandomAudioSource.ActualAudioSource.isPlaying) {
                PlayRandomMusic();
            }
        }
    }

    public void PlayRandomMusic() {
        _musicRandomAudioSource.PlayRandom();
    }
}
