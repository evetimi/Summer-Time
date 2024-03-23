using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIStatContainer : MonoBehaviour
{
    [SerializeField] private Animator _anim;
    [SerializeField] private TMP_Text _titleText;
    [SerializeField] private TMP_Text _amountText;

    public Animator Anim => _anim;
    public TMP_Text TitleText => _titleText;
    public TMP_Text AmountText => _amountText;

    public void SetEnable(bool enable) {
        if (_anim) {
            _anim.SetBool("enable", enable);
        }
    }
}
