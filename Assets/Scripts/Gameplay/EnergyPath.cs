using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyPath : MonoBehaviour
{
    //[SerializeField] private Vector2Int _jumpPowerRandom = new Vector2Int(-3, 3);
    [SerializeField] private Vector2 _moveDuration = new Vector2(0.5f, 1f);

    public void DoMovement(Vector3 startPos, Vector3 endPos) {
        transform.position = startPos;

        //int jumpPower = Random.Range(_jumpPowerRandom.x, _jumpPowerRandom.y);
        float moveDuration = Random.Range(_moveDuration.x, _moveDuration.y);
        //transform.DOLocalJump(endPos, jumpPower, 1, moveDuration).SetEase(Ease.InOutSine);

        //Vector3 moveDistance = endPos - startPos;

        //transform.DOBlendableMoveBy(moveDistance, moveDuration);
        transform.DOMove(endPos, moveDuration).SetEase(Ease.InSine);
    }
}
