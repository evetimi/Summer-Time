using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIComboPopup : MonoBehaviour
{
    [SerializeField] private ComboRankSetup _comboRankSetup;
    [SerializeField] private bool _destroyPreviousOnNewSpawn;
    [SerializeField] private AudioSource _voiceSource;

    private UISpeechBubble _previousComboSpeechBubble;

    public void SpawnCombo(int comboRankIndex) {
        if (_comboRankSetup == null || !_comboRankSetup.TryGetComboRank(comboRankIndex, out var comboRank)) {
            return;
        }
        
        if (comboRank.comboPopups == null || comboRank.comboPopups.Length == 0 || comboRank.comboTextsAndVoices == null || comboRank.comboTextsAndVoices.Length == 0) {
            return;
        }

        if (_previousComboSpeechBubble != null) {
            Destroy(_previousComboSpeechBubble.gameObject);
        }

        UISpeechBubble comboPopup = comboRank.comboPopups[Random.Range(0, comboRank.comboPopups.Length)];
        var comboTextsAndVoices = comboRank.comboTextsAndVoices[Random.Range(0, comboRank.comboTextsAndVoices.Length)];
        Color textColor = comboRank.textColors[Random.Range(0, comboRank.textColors.Length)];

        _previousComboSpeechBubble = Instantiate(comboPopup, transform);
        _previousComboSpeechBubble.SetText(comboTextsAndVoices.text, textColor);
        Speak(_voiceSource, comboTextsAndVoices.voice);
    }

    private void Speak(AudioSource source, AudioClip clip) {
        if (source && clip) {
            source.clip = clip;
            source.Play();
        }
    }
}
