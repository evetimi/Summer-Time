using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopList : MonoBehaviour
{
    [BoxGroup("Shop List"), SerializeField] private OpenCloseAnimTrigger _listTrigger;
    [BoxGroup("Shop List"), SerializeField] private float _listTriggerWaitTime = 0.22f;
    [BoxGroup("Shop List"), SerializeField] private ShopItemComponent _shopItemPrefab;
    [BoxGroup("Shop List"), SerializeField] private Transform _shopItemContainer;
    [BoxGroup("Shop List"), SerializeField] private Transform _middlePosition;
    [BoxGroup("Shop List"), SerializeField] private float _moveTime = 0.1f;
    [BoxGroup("Shop List"), SerializeField] private OpenCloseAnimTrigger _leftButtonTrigger;
    [BoxGroup("Shop List"), SerializeField] private OpenCloseAnimTrigger _rightButtonTrigger;
    [BoxGroup("Shop List"), SerializeField] private SellingShopItemSO _sellingShopItem;

    [BoxGroup("Shop Description"), SerializeField] private OpenCloseAnimTrigger _shopDescriptionTrigger;
    [BoxGroup("Shop Description"), SerializeField] private Image _imageShowcase;
    [BoxGroup("Shop Description"), SerializeField] private TMP_Text _itemName;
    [BoxGroup("Shop Description"), SerializeField] private TMP_Text _itemDescription;
    [BoxGroup("Shop Description"), SerializeField] private TMP_Text _coinCostText;
    [BoxGroup("Shop Description"), SerializeField] private TMP_Text _pearlCostText;
    [BoxGroup("Shop Description"), SerializeField] private Button _coinButton;
    [BoxGroup("Shop Description"), SerializeField] private Button _pearlButton;
    [BoxGroup("Shop Description"), SerializeField] private GameObject _buyButton;
    [BoxGroup("Shop Description"), SerializeField] private GameObject _equipButton;
    [BoxGroup("Shop Description"), SerializeField] private GameObject _equippedText;

    private List<ShopItemComponent> _shopItemComponents;
    private int _currentIndex;

    public void SetSellingShopItem(SellingShopItemSO sellingShopItemSO) {
        if (sellingShopItemSO == null)
            return;
        
        IEnumerator enumerator() {
            _listTrigger.Close();

            yield return new WaitForSeconds(_listTriggerWaitTime);

            _listTrigger.Open();

            _sellingShopItem = sellingShopItemSO;

            _shopItemComponents ??= new List<ShopItemComponent>(10);
            int index = 0;

            foreach (var item in sellingShopItemSO.ShopItemsToSell) {
                if (index >= _shopItemComponents.Count) {
                    _shopItemComponents.Add(Instantiate(_shopItemPrefab, _shopItemContainer));
                }

                _shopItemComponents[index].gameObject.SetActive(true);
                _shopItemComponents[index].SetItem(this, item, index);
                index++;
            }

            for (; index < _shopItemComponents.Count; index++) {
                _shopItemComponents[index].gameObject.SetActive(false);
            }

            _currentIndex = 0;

            yield return null;

            SetDescription(_shopItemComponents[_currentIndex].ContainShopItem);
            _shopDescriptionTrigger.Open();

            Vector3 direction = _middlePosition.position - _shopItemComponents[_currentIndex].transform.position;
            Vector3 targetPosition = _shopItemContainer.position + direction;
            _shopItemContainer.transform.position = targetPosition;
            //Vector3 localTargetPosition = _shopItemContainer.parent.InverseTransformPoint(targetPosition);
            //_shopItemContainer.transform.localPosition = localTargetPosition;

            SetButtonVisual();
        }

        StartCoroutine(enumerator());
    }

    public bool ValidateIndex(int index) {
        return index >= 0 && index < _shopItemComponents.Count && _shopItemComponents[index].gameObject.activeSelf;
    }

    public void ScrollTo(int index) {
        if (!ValidateIndex(index)) {
            return;
        }

        _currentIndex = index;
        Vector3 direction = _middlePosition.position - _shopItemComponents[_currentIndex].transform.position;
        Vector3 targetPosition = _shopItemContainer.position + direction;
        Vector3 localTargetPosition = _shopItemContainer.parent.InverseTransformPoint(targetPosition);

        _shopDescriptionTrigger.Close();

        _shopItemContainer.DOKill();
        _shopItemContainer.DOLocalMove(localTargetPosition, _moveTime).SetEase(Ease.InOutSine).OnComplete(() => {
            SetDescription(_shopItemComponents[_currentIndex].ContainShopItem);
            _shopDescriptionTrigger.Open();
        });

        SetButtonVisual();
    }

    public void ScrollNext() {
        ScrollTo(_currentIndex + 1);
    }

    public void ScrollBack() {
        ScrollTo(_currentIndex - 1);
    }

    private void SetDescription(ShopItemSO shopItemSO) {
        _imageShowcase.sprite = shopItemSO.ImageShowcase;
        _itemName.text = shopItemSO.ItemName;
        _itemDescription.text = shopItemSO.ItemDescription;
        
        if (shopItemSO.IsSellingWithCoin) {
            _coinButton.interactable = true;
            _coinCostText.text = shopItemSO.CoinAmountNeeded.ToString();
        } else {
            _coinButton.interactable = false;
            _coinCostText.text = "No Offer!";
        }

        if (shopItemSO.IsSellingWithPearl) {
            _pearlButton.interactable = true;
            _pearlCostText.text = shopItemSO.PearlAmountNeeded.ToString();
        } else {
            _pearlButton.interactable = false;
            _pearlCostText.text = "No Offer!";
        }

        if (_currentIndex == 0) {
            _buyButton.SetActive(false);
            _equipButton.SetActive(false);
            _equippedText.SetActive(true);
        } else if (_currentIndex == 1) {
            _buyButton.SetActive(false);
            _equipButton.SetActive(true);
            _equippedText.SetActive(false);
        } else {
            _buyButton.SetActive(true);
            _equipButton.SetActive(false);
            _equippedText.SetActive(false);
        }
    }

    public void SetButtonVisual() {
        if (_leftButtonTrigger) {
            if (_currentIndex == 0) {
                _leftButtonTrigger.Close();
            } else {
                _leftButtonTrigger.Open();
            }
        }

        if (_rightButtonTrigger) {
            if (_currentIndex == _shopItemComponents.Count - 1 || !_shopItemComponents[_currentIndex + 1].gameObject.activeSelf) {
                _rightButtonTrigger.Close();
            } else {
                _rightButtonTrigger.Open();
            }
        }
    }
}
