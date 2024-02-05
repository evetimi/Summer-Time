using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFollowsCamera : MonoBehaviour
{
    [SerializeField] private Transform _middlePoint;
    [SerializeField] private float _moveRange = 3.5f;

    public Vector3 current;
    public Vector3 mouse;

    private void LateUpdate() {
        current = _middlePoint.position;
        mouse = Input.mousePosition;

        Vector3 direction = (mouse - current).normalized;

        transform.position = _middlePoint.position + direction * _moveRange;
    }
}
