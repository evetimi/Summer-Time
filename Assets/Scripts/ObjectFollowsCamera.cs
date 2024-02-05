using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFollowsCamera : MonoBehaviour
{
    [SerializeField] private Transform _middlePoint;
    [SerializeField] private float _moveRange = 3.5f;
    [SerializeField] private bool _convertCurrentToScreenSpace;
    [ShowInInspector, ReadOnly] private Vector3 current;
    [ShowInInspector, ReadOnly] private Vector3 mouse;

    private void LateUpdate() {
        current = _middlePoint.position;
        mouse = Input.mousePosition;

        if (_convertCurrentToScreenSpace) {
            current = Camera.main.WorldToScreenPoint(current);
        }

        Vector3 direction = (mouse - current).normalized;

        transform.position = _middlePoint.position + direction * _moveRange;
    }
}
