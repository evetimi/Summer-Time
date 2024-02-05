using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonVisual : MonoBehaviour
{
    [BoxGroup("Decoration"), SerializeField] private Transform _decorationImage;
    [BoxGroup("Decoration"), SerializeField] private Vector2 _randomRangeIncludesNegative = new(400f, 600f);

    [Button]
    private void RandomizeDerationPosition() {
        Vector2 randPos = new Vector2(Random.Range(-_randomRangeIncludesNegative.x, _randomRangeIncludesNegative.x), Random.Range(-_randomRangeIncludesNegative.y, _randomRangeIncludesNegative.y));
        _decorationImage.localPosition = randPos;
    }

    void Start()
    {
        RandomizeDerationPosition();
    }
}
