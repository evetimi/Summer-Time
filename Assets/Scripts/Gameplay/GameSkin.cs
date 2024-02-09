using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "Game Skin", menuName = "Gameplay Skin Collection")]
public class GameSkin : ScriptableObject
{
    [SerializeField] private Sprite[] _itemSprites;

    public Sprite[] RandomChooseSprite(int amount) {
        if (amount <= 0 || amount > _itemSprites.Length) {
            return null;
        }

        if (amount == _itemSprites.Length) {
            return (Sprite[])_itemSprites.Clone();
        }

        bool[] chosen = new bool[_itemSprites.Length];

        Sprite[] sprites = new Sprite[amount];

        for (int i = 0; i < amount; i++) {
            int rand = Random.Range(0, _itemSprites.Length);

            while (rand < _itemSprites.Length && chosen[rand]) {
                rand++;
            }

            if (rand >= _itemSprites.Length) {
                rand = 0;
                while (rand < _itemSprites.Length && chosen[rand]) {
                    rand++;
                }
            }

            chosen[rand] = true;
            
            sprites[i] = _itemSprites[rand];
        }

        return sprites;
    }
}
