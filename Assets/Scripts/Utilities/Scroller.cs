using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class Scroller : MonoBehaviour
{
    [SerializeField] private bool _doAnim;
    [SerializeField] private RawImage _img;
    [SerializeField] private float _x, _y;

    private void Reset() {
        _img = GetComponent<RawImage>();
    }

    void Update() {
        if (!_doAnim || _img == null)
            return;

        _img.uvRect = new Rect(_img.uvRect.position + new Vector2(_x, _y) * Time.deltaTime, _img.uvRect.size);
    }
}