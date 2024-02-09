using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameplayController : MonoBehaviourSingleton<MainGameplayController>
{
    [BoxGroup("Setup"), SerializeField] private GameSkin _usedGameSkin;
    [BoxGroup("Setup"), SerializeField] private Transform[] _topBoxes;
    [BoxGroup("Setup"), SerializeField] private int _amountOfUniqueItem;
}
