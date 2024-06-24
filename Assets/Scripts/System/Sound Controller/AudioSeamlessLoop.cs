using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSeamlessLoop : MonoBehaviour
{
    [SerializeField] private bool _playOnAwake;
    [SerializeField] private AudioClip _currentClip;
    [SerializeField] private AudioSource[] _audioSources;
    private int _currentAudioIndex;

    private LoopState _loopState;
    private double _musicDuration;
    private double _goalTime;
    private double _nextCheck;
    private bool _ending;

    public LoopState LoopState { get { return _loopState; } }
    public double MusicDuration { get { return _musicDuration; } }
    public double GoalTime { get { return _goalTime; } }
    public AudioClip CurrentClip { get { return _currentClip; } }

    //public bool IsPlaying { get { return _loopState == LoopState.Start || _loopState == LoopState.Loop || _loopState == LoopState.End; } }
    public bool IsPlaying { get { return _loopState != LoopState.NoPlaying; } }
    public bool IsStarting { get { return _loopState == LoopState.Start; } }
    public bool IsLooping { get { return _loopState == LoopState.Loop; } }
    public bool IsEnding { get { return _loopState == LoopState.End; } }

    public float Volumn { get => _audioSources[0].volume; set { foreach (var audio in _audioSources) { audio.volume = value; } } }
    public float Pitch { get => _audioSources[0].pitch; set { foreach (var audio in _audioSources) { audio.pitch = value; } } }

    private void Awake() {
        if (_playOnAwake && _currentClip != null) {
            OnPlayAudio();
        }
    }

    private void OnPlayAudio() {
        _goalTime = AudioSettings.dspTime + 0.1d;

        _audioSources[_currentAudioIndex].clip = _currentClip;
        _audioSources[_currentAudioIndex].PlayScheduled(_goalTime);

        _musicDuration = (double)_currentClip.samples / _currentClip.frequency;
        _goalTime = _goalTime + _musicDuration;
        GetNextCheck();

        _currentAudioIndex = 1 - _currentAudioIndex;

        _loopState = LoopState.Start;
    }

    private void Update() {
        if (AudioSettings.dspTime > _nextCheck) {
            if (_loopState == LoopState.Start || _loopState == LoopState.Loop) {
                PlayScheduledClip();
            } else if (_loopState == LoopState.End) {
                _loopState = LoopState.NoPlaying;
            }
        }
    }

    private void PlayScheduledClip() {
        _audioSources[_currentAudioIndex].clip = _currentClip;

        if (_currentClip != null) {
            _audioSources[_currentAudioIndex].PlayScheduled(_goalTime);

            _musicDuration = (double)_currentClip.samples / _currentClip.frequency;
            _goalTime = _goalTime + _musicDuration;
            GetNextCheck();

            _currentAudioIndex = 1 - _currentAudioIndex;
        }

        if (_ending) {
            _loopState = LoopState.End;
            _ending = false;
        } else {
            _loopState = LoopState.Loop;
        }
    }

    private void GetNextCheck() {
        if (_musicDuration < 1d) {
            _nextCheck = _goalTime - (_musicDuration / 2d);
        } else {
            _nextCheck = _goalTime - _musicDuration;
        }
    }

    /// <summary>
    /// Start the loop with the startClip with be played at start and loopClip with be looped forward. Only work if the Audio is NOT PLAYING
    /// </summary>
    /// <param name="startClip">On start clip to play</param>
    /// <param name="loopClip">Looping clip after startClip ended</param>
    public void StartPlaying(AudioClip startClip, AudioClip loopClip, bool endAfterLoopClip = false) {
        if (_loopState == LoopState.NoPlaying) {
            _currentClip = startClip;
            OnPlayAudio();

            if (endAfterLoopClip) {
                SetEndedClip(loopClip);
            } else {
                _currentClip = loopClip;
            }
        }
    }

    /// <summary>
    /// Set the next clip to loop
    /// </summary>
    /// <param name="clip">Next clip to loop</param>
    public void SetCurrentClip(AudioClip clip) {
        _currentClip = clip;
    }

    /// <summary>
    /// Set the next clip to be the end of the loop
    /// </summary>
    /// <param name="clip">The end clip</param>
    public void SetEndedClip(AudioClip clip) {
        _currentClip = clip;
        _ending = true;
    }

    public void StopImmediately() {
        _loopState = LoopState.NoPlaying;
        foreach (var audioSource in _audioSources) {
            audioSource.Stop();
        }
    }
}

public enum LoopState {
    NoPlaying,
    Start,
    Loop,
    End
}