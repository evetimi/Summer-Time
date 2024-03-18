using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UICurrentScore : MonoBehaviour
{
    [SerializeField] private Animator _anim;
    [SerializeField] private TMP_Text _scoreText;

    public void OnScoreGotten() {
        _scoreText.text = MainGameplayController.Instance.CurrentScore.ToString();
        if (_anim) {
            _anim.SetTrigger("score");
        }
    }
}
