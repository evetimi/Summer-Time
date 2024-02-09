using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameItemContainer : MonoBehaviour
{
    [BoxGroup("References"), SerializeField] private Image _image;

    [SerializeField, Tooltip("If Object ID = -1, it is not a normal item")] private int _itemId = -1;

    public void SetItem(Sprite itemSprite, int itemId) {

    }
}
