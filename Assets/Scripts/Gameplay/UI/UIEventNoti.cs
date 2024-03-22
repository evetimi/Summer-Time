using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEventNoti : MonoBehaviour
{
    [SerializeField] private Animator _anim;
    [SerializeField] private UIAmountDisplay _amountDisplay;
    [SerializeField] private Color _normalTextColor = Color.white;
    [SerializeField] private Color _warningTextColor = Color.red;

    private bool enableWarning;

    public void On10sAwayFromEventHappens() {
        _anim.SetTrigger("warning");
        enableWarning = true;
    }

    public void OnEventHappened() {
        _anim.SetTrigger("end");
        enableWarning = false;
    }

    private void Update() {
        if (enableWarning && MainGameplayController.Instance) {
            int seconds = (int)(MainGameplayController.Instance.GameTimer - MainGameplayController.Instance.EventHappensAt);

            _amountDisplay.tmpAmountText.color = (seconds > 3) ? _normalTextColor: _warningTextColor;

            int minutes = seconds / 60;
            seconds %= 60;
            _amountDisplay.SetAmount(minutes, seconds);
        }
    }
}
