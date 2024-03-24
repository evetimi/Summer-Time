using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shop Item", menuName = "Shop Item")]
public class ShopItemSO : ScriptableObject
{
    [SerializeField] private Sprite _icon;
    [SerializeField] private Sprite _imageShowcase;
    [SerializeField] private string _itemName = "Summer Pack";
    [SerializeField] private string _itemDescription = "Get the pack to enhance your gameplay experience!";
    [SerializeField] private bool _sellWithCoin = true;
    [SerializeField] private int _coinAmountNeeded = 2000;
    [SerializeField] private bool _sellWithPearl = true;
    [SerializeField] private int _pearlAmountNeeded = 100;

    public Sprite Icon => _icon;
    public Sprite ImageShowcase => _imageShowcase;
    public string ItemName => _itemName;
    public string ItemDescription => _itemDescription;
    public bool IsSellingWithCoin => _sellWithCoin;
    public int CoinAmountNeeded => _coinAmountNeeded;
    public bool IsSellingWithPearl => _sellWithPearl;
    public int PearlAmountNeeded => _pearlAmountNeeded;
}
