using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SettingsController : UIService<SettingsController>
{
    [BoxGroup("Setup"), SerializeField] private float _openWaitTime = 0.2f;
    [BoxGroup("Setup"), SerializeField] private SettingMenu[] _settingMenus;

    [BoxGroup("Title Board"), SerializeField] private OpenCloseAnimTrigger _titleBoardTrigger;
    [BoxGroup("Title Board"), SerializeField] private float _titleBoardOffAndOnWaitTime = 0.4f;
    [BoxGroup("Title Board"), SerializeField] private TMP_Text _titleText;

    private SettingMenu _currentSettingMenu;

    public override void Open() {
        base.Open();

        IEnumerator enumerator() {
            yield return new WaitForSeconds(_openWaitTime);
            ChangeMenu(0);
        }

        StartCoroutine(enumerator());
    }

    public override void Close() {
        base.Close();
        if (_currentSettingMenu) {
            _currentSettingMenu.Close();
            _currentSettingMenu = null;
        }
    }

    public bool ValidateMenu(int index) {
        return index >= 0 && index < _settingMenus.Length;
    }

    public void ChangeMenu(int index) {
        if (!ValidateMenu(index)) {
            return;
        }

        SettingMenu menu = _settingMenus[index];
        if (menu != null) {
            if (_currentSettingMenu) {
                _currentSettingMenu.Close();
            }

            StartCoroutine(TitleCoroutine(menu.SettingName, !_currentSettingMenu));

            _currentSettingMenu = menu;
            _currentSettingMenu.Open();
        }
    }

    private IEnumerator TitleCoroutine(string titleName, bool onlyOpen) {
        if (!onlyOpen) {
            _titleBoardTrigger.Close();
            yield return new WaitForSeconds(_titleBoardOffAndOnWaitTime);
        }

        _titleText.text = titleName;

        _titleBoardTrigger.Open();
    }
}
