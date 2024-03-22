using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Combo Rank Setup", menuName = "Combo Rank Setup")]
public class ComboRankSetup : ScriptableObject
{
    [SerializeField] private ComboRankList[] _comboRankList;

    [System.Serializable]
    public class ComboRankList {
        public UISpeechBubble[] comboPopups;
        public string[] comboTexts;
        [ListDrawerSettings(AddCopiesLastElement = true)] public Color[] textColors;
    }

    public bool IsComboRankExisted(int comboRankIndex) {
        return _comboRankList != null &&
            (comboRankIndex >= 0 && comboRankIndex < _comboRankList.Length);
    }

    public bool TryGetComboRank(int comboRankIndex, out ComboRankList comboRankList) {
        if (IsComboRankExisted(comboRankIndex)) {
            comboRankList = _comboRankList[comboRankIndex];
            return true;
        } else if (comboRankIndex >= _comboRankList.Length) {
            comboRankList = _comboRankList[^1];
            return true;
        } else {
            comboRankList = null;
            return false;
        }
    }
}
