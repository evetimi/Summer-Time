using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class UIComboText : MonoBehaviour
{
    [SerializeField] private Animator _anim;
    [SerializeField] private TMP_Text _comboText;
    [SerializeField] private Vector2 _zRotationPopupRandom = new Vector2(-10f, 10f);
    [SerializeField, Range(0f, 1f)] private float _textColorRandomRangeMin = 0.4f;
    [SerializeField, Range(0f, 1f)] private float _textColorRandomRangeMax = 0.8f;
    [SerializeField] private string _comboPrefix = "Combo ";
    [SerializeField] private string _matchPrefix = "Match ";
    [SerializeField] private RandomAudioSource _audioRandom;

    private void Reset() {
        _anim = GetComponent<Animator>();
        _comboText = GetComponent<TMP_Text>();
    }

    private Color RandomColor() {
        return new Color(
            Random.Range(_textColorRandomRangeMin, _textColorRandomRangeMax),
            Random.Range(_textColorRandomRangeMin, _textColorRandomRangeMax),
            Random.Range(_textColorRandomRangeMin, _textColorRandomRangeMax),
            1f
        );
    }

    public void PopCombo(int combo) {
        if (_comboText == null)
            return;

        _comboText.text = _comboPrefix + combo.ToString() + "!";
        _comboText.transform.localRotation = Quaternion.Euler(0f, 0f, Random.Range(_zRotationPopupRandom.x, _zRotationPopupRandom.y));
        _comboText.color = RandomColor();

        if (_anim) {
            _anim.SetTrigger("pop");
        }

        if (_audioRandom) {
            _audioRandom.PlayRandom();
        }
    }

    public void PopMatch(int match) {
        if (_comboText == null)
            return;
        
        _comboText.text = _matchPrefix + match.ToString() + "!";
        _comboText.transform.localRotation = Quaternion.Euler(0f, 0f, Random.Range(_zRotationPopupRandom.x, _zRotationPopupRandom.y));
        _comboText.color = RandomColor();

        if (_anim) {
            _anim.SetTrigger("pop");
        }

        if (_audioRandom) {
            _audioRandom.PlayRandom();
        }
    }
}
