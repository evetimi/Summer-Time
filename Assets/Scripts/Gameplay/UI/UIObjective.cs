using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIObjective : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;

    public void OnGenerateLevelCompleted(MainGameplayController controller) {
        _scoreText.text = controller.ScoreObjective.ToString();
    }
}
