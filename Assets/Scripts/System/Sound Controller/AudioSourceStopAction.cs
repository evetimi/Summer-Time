using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioSourceStopAction : MonoBehaviour
{
    public StopAction stopAction;
    public AudioSource linkedAudioSource;

    public enum StopAction {
        None,
        Disable,
        Destroy
    }

    private bool enableCheck;

    private void Reset() {
        linkedAudioSource = GetComponent<AudioSource>();
    }
    
    void Update()
    {
        if (linkedAudioSource == null)
            return;

        if (enableCheck) {
            if (!linkedAudioSource.isPlaying) {
                enableCheck = false;
                DoStopAction();
            }
        } else {
            if (linkedAudioSource.isPlaying) {
                enableCheck = true;
            }
        }
    }

    private void DoStopAction() {
        switch (stopAction) {
            case StopAction.Disable: { linkedAudioSource.gameObject.SetActive(false); break; }
            case StopAction.Destroy: { Destroy(linkedAudioSource.gameObject); break; }
        }
    }
}
