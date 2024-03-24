using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Selling Shop Item", menuName = "Selling Shop Item")]
public class SellingShopItemSO : ScriptableObject
{
    [SerializeField] private string _packName;
    [SerializeField] private ShopItemSO[] _shopItemsToSell;

    public string PackName => _packName;
    public ShopItemSO[] ShopItemsToSell => _shopItemsToSell;
}
