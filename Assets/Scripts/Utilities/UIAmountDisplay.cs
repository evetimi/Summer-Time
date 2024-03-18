using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIAmountDisplay : MonoBehaviour
{
    [BoxGroup("Setup")] public bool useTMPText;
    [BoxGroup("Setup"), DisableIf(nameof(useTMPText))] public Text amountText;
    [BoxGroup("Setup"), EnableIf(nameof(useTMPText))] public TMP_Text tmpAmountText;
    [BoxGroup("Setup"), Tooltip("Separator between amount and max (ex: 4/10)")] public string separator = "/";
    
    [BoxGroup("Current Amount")] public bool isDisplayingCurrentAmount = true;
    [BoxGroup("Current Amount"), SerializeField, ShowIf(nameof(isDisplayingCurrentAmount))] private AmountDisplayProperty _currentAmountProperty;

    [BoxGroup("Max Amount")] public bool isDisplayingMaxAmount = true;
    [BoxGroup("Max Amount"), SerializeField, ShowIf(nameof(isDisplayingMaxAmount))] private AmountDisplayProperty _maxAmountProperty;

    [BoxGroup("Runtime"), ShowInInspector, ShowIf(nameof(isDisplayingCurrentAmount))] private int _current = 0;
    [BoxGroup("Runtime"), ShowInInspector, ShowIf(nameof(isDisplayingMaxAmount))] private int _max = 0;

    public int Current => _current;
    public int Max => _max;

    [System.Serializable]
    public struct AmountDisplayProperty {
        [Tooltip("If the total digit is less than this, will add 0 before the number to match the total digit required")]
        public int minDigitShowing;
        [Tooltip("Display the prefix of the number")]
        public string displayPrefix;
        [Tooltip("Display the subfix of the number")]
        public string displaySubfix;
    }

    private void Reset() {
        amountText = GetComponent<Text>();
        tmpAmountText = GetComponent<TMP_Text>();

        useTMPText = (tmpAmountText && !amountText);
    }

    private void OnValidate() {
        if (amountText || tmpAmountText) {
            SetAmount(_current, _max);
        }
    }

    public void SetAmount(int amount, int max) {
        static string getNumberSetup(int n, int digitCount) {
            string result = n.ToString();
            int length = result.Length;
            while (length < digitCount) {
                length++;
                result = "0" + result;
            }
            return result;
        }

        _current = amount;
        this._max = max;

        if ((!useTMPText && amountText == null) || (useTMPText && tmpAmountText == null)) {
            return;
        }

        string text = "";

        if (isDisplayingCurrentAmount) {
            text += $"{_currentAmountProperty.displayPrefix}{getNumberSetup(_current, _currentAmountProperty.minDigitShowing)}{_currentAmountProperty.displayPrefix}";
        }

        if (isDisplayingCurrentAmount && isDisplayingMaxAmount) {
            text += separator;
        }

        if (isDisplayingMaxAmount) {
            text += $"{_maxAmountProperty.displayPrefix}{getNumberSetup(max, _maxAmountProperty.minDigitShowing)}{_maxAmountProperty.displaySubfix}";
        }

        if (!useTMPText) {
            amountText.text = text;
        } else {
            tmpAmountText.text = text;
        }
    }
}
