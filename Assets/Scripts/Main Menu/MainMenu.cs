using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private float _spawnDelay = 0.05f;
    [SerializeField] private int _spawnAmountOnEachDelay = 5;
    [SerializeField] private GameObject[] _startSpawnRandomly;

    void Start() {
        StartCoroutine(StartMenuCoroutine());
    }

    [Button]
    public void SetActiveTrueAll() {
        foreach (var obj in _startSpawnRandomly) {
            obj.SetActive(true);
        }
    }

    [Button]
    public void SetActiveFalseAll() {
        foreach (var obj in _startSpawnRandomly) {
            obj.SetActive(false);
        }
    }

    private IEnumerator StartMenuCoroutine() {
        SetActiveFalseAll();

        int rand = Random.Range(0, _startSpawnRandomly.Length);
        int index = rand;
        int currentAmount = 0;

        while (!_startSpawnRandomly[index].activeSelf) {
            _startSpawnRandomly[index].SetActive(true);

            index += rand;
            if (index >= _startSpawnRandomly.Length) {
                index -= _startSpawnRandomly.Length;
            }

            currentAmount++;

            if (currentAmount < _spawnAmountOnEachDelay) {
                continue;
            } else {
                currentAmount = 0;
            }

            yield return new WaitForSeconds(_spawnDelay);
        }

        for (int i = 0; i < _startSpawnRandomly.Length; i++) {
            if (_startSpawnRandomly[i].activeSelf) {
                continue;
            }

            _startSpawnRandomly[i].SetActive(true);

            currentAmount++;

            if (currentAmount < _spawnAmountOnEachDelay) {
                continue;
            } else {
                currentAmount = 0;
            }

            yield return new WaitForSeconds(_spawnDelay);
        }
    }
}
