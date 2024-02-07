using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCloseAnimTrigger : MonoBehaviour
{
    private bool BoolType => triggerType == TriggerType.Bool;
    private bool TriggType => triggerType == TriggerType.Trigger;

    [BoxGroup("Properties"), SerializeField] private Animator _anim;
    [BoxGroup("Properties")] public TriggerType triggerType;
    [BoxGroup("Properties"), ShowIf(nameof(BoolType))] public string boolName = "enabled";
    [BoxGroup("Properties"), ShowIf(nameof(TriggType))] public string triggerOnName = "open";
    [BoxGroup("Properties"), ShowIf(nameof(TriggType))] public string triggerOffName = "close";

    public enum TriggerType {
        Trigger,
        Bool
    }

    private void Reset() {
        _anim = GetComponent<Animator>();
    }

    public void Open() {
        if (BoolType) {
            _anim.SetBool(boolName, true);
        } else if (TriggType) {
            _anim.SetTrigger(triggerOnName);
        }
    }

    public void Close() {
        if (BoolType) {
            _anim.SetBool(boolName, false);
        } else if (TriggType) {
            _anim.SetTrigger(triggerOffName);
        }
    }
}
