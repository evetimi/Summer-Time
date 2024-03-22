using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIComboPopup : MonoBehaviour
{
    [SerializeField] private ComboRankSetup _comboRankSetup;
    [SerializeField] private bool _destroyPreviousOnNewSpawn;

    private UISpeechBubble _previousComboSpeechBubble;

    public void SpawnCombo(int comboRankIndex) {
        if (_comboRankSetup == null || !_comboRankSetup.TryGetComboRank(comboRankIndex, out var comboRank)) {
            return;
        }
        
        if (comboRank.comboPopups == null || comboRank.comboPopups.Length == 0 || comboRank.comboTexts == null || comboRank.comboTexts.Length == 0) {
            return;
        }

        if (_previousComboSpeechBubble != null) {
            Destroy(_previousComboSpeechBubble.gameObject);
        }

        UISpeechBubble comboPopup = comboRank.comboPopups[Random.Range(0, comboRank.comboPopups.Length)];
        string comboText = comboRank.comboTexts[Random.Range(0, comboRank.comboTexts.Length)];
        Color textColor = comboRank.textColors[Random.Range(0, comboRank.textColors.Length)];

        _previousComboSpeechBubble = Instantiate(comboPopup, transform);
        _previousComboSpeechBubble.SetText(comboText, textColor);
    }
}
