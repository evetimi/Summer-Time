using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAnimationRandomly : MonoBehaviour
{
    public Animator anim;
    public string animationName;
    public int animationLayer = -1;

    private void Reset() {
        anim = GetComponent<Animator>();
    }

    void OnEnable() {
        if (anim == null) {
            return;
        }

        anim.Play(animationName, animationLayer, Random.Range(0f, 1f));
    }
}
