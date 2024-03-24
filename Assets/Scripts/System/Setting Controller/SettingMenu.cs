using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingMenu : MonoBehaviour
{
    [SerializeField] private string _settingName = "Settings";
    [SerializeField] private OpenCloseAnimTrigger _animTrigger;

    public string SettingName => _settingName;

    private void Reset() {
        _animTrigger = GetComponent<OpenCloseAnimTrigger>();
    }

    public void Open() {
        if (_animTrigger) {
            _animTrigger.Open();
        }
    }

    public void Close() {
        if (_animTrigger) {
            _animTrigger.Close();
        }
    }
}
