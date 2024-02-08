using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        bool[] chosen = new bool[amount];
        Sprite[] sprites = new Sprite[amount];
        int currentIndex = 0;

        for (int i = 0; i < amount; i++) {
            int rand = Random.Range(0, _itemSprites.Length);

            while (rand < _itemSprites.Length && !chosen[rand]) {
                rand++;
            }

            if (rand >= _itemSprites.Length) {
                rand = 0;
                while (rand < _itemSprites.Length && !chosen[rand]) {
                    rand++;
                }
            }

            sprites[currentIndex++] = _itemSprites[rand];
        }

        return sprites;
    }
}
