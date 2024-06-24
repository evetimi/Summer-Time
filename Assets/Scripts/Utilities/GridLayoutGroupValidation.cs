using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// [ExecuteInEditMode]
public class GridLayoutGroupValidation : UIBehaviour
{
    public GridLayoutGroup layoutGroup;
    public Vector2Int gridChildAmount = new(7, 7);

#if UNITY_EDITOR
    protected override void Reset() {
        base.Reset();
        layoutGroup = GetComponent<GridLayoutGroup>();
    }

    protected override void OnValidate() {
        base.OnValidate();
        ValidateCellSize();
    }
#endif

    protected override void OnRectTransformDimensionsChange() {
        ValidateCellSize();
    }

    private void ValidateCellSize() {
        if (layoutGroup == null || gridChildAmount.x == 0 || gridChildAmount.y == 0) {
            return;
        }

        RectTransform rectTransform = GetComponent<RectTransform>();
        float width = rectTransform.rect.width;
        float height = rectTransform.rect.height;

        Vector2 cellSize = new Vector2(width / gridChildAmount.x, height / gridChildAmount.y);

        if (cellSize != layoutGroup.cellSize) {
            layoutGroup.cellSize = cellSize;
        }
    }
}
