using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemComponent : MonoBehaviour
{
    [SerializeField] private Image _iconImage;
    [ReadOnly, ShowInInspector] private ShopItemSO _containShopItem;
    [ReadOnly, ShowInInspector] private int _itemIndex;

    private ShopList _shopListContainer;

    public ShopItemSO ContainShopItem => _containShopItem;
    public int ItemIndex => _itemIndex;

    public void SetItem(ShopList shopListContainer, ShopItemSO item, int itemIndex) {
        if (item != null) {
            _containShopItem = item;
            if (_iconImage) {
                _iconImage.sprite = _containShopItem.Icon;
            }

            _shopListContainer = shopListContainer;
            _itemIndex = itemIndex;
        }
    }

    public void ClickItem() {
        if (_shopListContainer) {
            _shopListContainer.ScrollTo(_itemIndex);
        }
    }
}
