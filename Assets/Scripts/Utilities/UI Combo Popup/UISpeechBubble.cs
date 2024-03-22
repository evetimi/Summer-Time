using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UISpeechBubble : MonoBehaviour
{
    [SerializeField] private TMP_Text _speechText;

    private void Reset() {
        if (!TryGetComponent(out _speechText)) {
            _speechText = GetComponentInChildren<TMP_Text>();
        }
    }

    public void SetText(string text, Color textColor) {
        if (_speechText != null) {
            _speechText.text = text;
            _speechText.color = textColor;
        }
    }

    public void DestroySelf() {
        Destroy(gameObject);
    }
}
