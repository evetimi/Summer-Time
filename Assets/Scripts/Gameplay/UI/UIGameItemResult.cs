using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIGameItemResult : MonoBehaviour
{
    [SerializeField] private Animator _anim;
    [SerializeField] private Image _iconImage;
    [SerializeField] private TMP_Text _text;

    public Image IconImage => _iconImage;
    public TMP_Text Text => _text;

    public void SetEnable(bool enable) {
        if (_anim) {
            _anim.SetBool("enable", enable);
        }
    }
}
