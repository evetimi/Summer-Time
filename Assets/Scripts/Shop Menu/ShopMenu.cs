using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopMenu : UISingleton<ShopMenu>
{
    [SerializeField] private ShopList _shopList;
    [SerializeField] private OpenCloseAnimTrigger _titleTrigger;
    [SerializeField] private TMP_Text _titleText;
    [SerializeField] private float _titleTriggerWaitTime = 0.22f;
    [SerializeField] private int _startSellingListIndex;
    [SerializeField] private SellingShopItemSO[] _sellingList;

    private int _currentIndex;

    public override void Open() {
        base.Open();

        _currentIndex = -1;
        ChooseSellingList(_startSellingListIndex);
    }

    public bool ValidateSellingList(int index) {
        return index >= 0 && index < _sellingList.Length;
    }

    public void ChooseSellingList(int index) {
        if (!ValidateSellingList(index)) {
            return;
        }

        if (_currentIndex == index) {
            return;
        }

        _currentIndex = index;

        _shopList.SetSellingShopItem(_sellingList[_currentIndex]);

        StartCoroutine(SetTitleCoroutine(_sellingList[_currentIndex].PackName));
    }

    private IEnumerator SetTitleCoroutine(string titleName) {
        _titleTrigger.Close();

        yield return new WaitForSeconds(_titleTriggerWaitTime);

        _titleText.text = titleName;
        _titleTrigger.Open();
    }
}
