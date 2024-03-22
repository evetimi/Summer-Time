using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level Data", menuName = "Level Data")]
public class LevelData : ScriptableObject
{
    [SerializeField] private float _timer = 90f;
    [SerializeField] private float _eventHappensAt = 60f;
    [SerializeField] private int _scoreObjective = 2000;

    public float Timer => _timer;
    public float EventHappensAt => _eventHappensAt;
    public int ScoreObjective => _scoreObjective;
}
