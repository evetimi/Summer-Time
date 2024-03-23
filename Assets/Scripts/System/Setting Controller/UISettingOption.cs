using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISettingOption : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private CanvasGroup _hoverEffectCanvasGroup;
    [SerializeField] private float _effectTransition = 0.1f;

    public void OnPointerEnter(PointerEventData eventData) {
        _hoverEffectCanvasGroup.DOKill();
        _hoverEffectCanvasGroup.DOFade(1f, _effectTransition);
    }

    public void OnPointerExit(PointerEventData eventData) {
        _hoverEffectCanvasGroup.DOKill();
        _hoverEffectCanvasGroup.DOFade(0f, _effectTransition);
    }
}
