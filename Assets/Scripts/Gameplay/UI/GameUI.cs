using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviourSingleton<GameUI>
{
    [BoxGroup("Finish Game UI"), SerializeField] private OpenCloseAnimTrigger _finishGameUI;
    [BoxGroup("Finish Game UI"), SerializeField] private Animator _timeOutAnim;
    [BoxGroup("Finish Game UI"), SerializeField] private float _timeOutDuration = 1f;
    [BoxGroup("Finish Game UI"), SerializeField] private float _finishPanelWaitTime = 1.5f;

    [TabGroup("Score"), SerializeField] private TMP_Text _scoreText;
    [TabGroup("Score"), SerializeField] private TMP_Text _objectiveText;
    [TabGroup("Score"), SerializeField] private float _scoreIncreaseStartDelay = 1f;
    [TabGroup("Score"), SerializeField] private float _scoreIncreaseDelay = 0.1f;
    [TabGroup("Score"), SerializeField] private int _scoreIncreaseRate = 50;
    [TabGroup("Score"), SerializeField] private CanvasGroup _scoreBackgroundCanvasGroup;
    [TabGroup("Score"), SerializeField] private Image _scoreBackgroundImage;
    [TabGroup("Score"), SerializeField] private Color _scoreBackgroundPassedColor = Color.green;
    [TabGroup("Score"), SerializeField] private Color _scoreBackgroundFailedColor = Color.red;
    [TabGroup("Score"), SerializeField] private float _scoreBackgroundFadeInDuration = 1f;

    [TabGroup("Stamp"), SerializeField] private Animator _stampAnim;
    [TabGroup("Stamp"), SerializeField] private Image _certificateStamp;
    [TabGroup("Stamp"), SerializeField] private Sprite _passedStamp;
    [TabGroup("Stamp"), SerializeField] private Sprite _failedStamp;
    [TabGroup("Stamp"), SerializeField] private float _stampStartDelay = 1f;
    [TabGroup("Stamp"), SerializeField] private float _stampEndDelay = 2f;

    [TabGroup("Left Panel"), SerializeField] private UIStatContainer _usedUltimate;
    [TabGroup("Left Panel"), SerializeField] private UIStatContainer _longestCombo;
    [TabGroup("Left Panel"), SerializeField] private UIStatContainer _highestMatch;
    [TabGroup("Left Panel"), SerializeField] private float _leftPanelStartDelay = 1f;
    [TabGroup("Left Panel"), SerializeField] private float _leftPanelAmountIncreaseDelay = 0.1f;
    [TabGroup("Left Panel"), SerializeField] private int _leftPanelAmountIncreaseRate = 2;

    [TabGroup("Right Panel"), SerializeField] private Transform _rightPanel;
    [TabGroup("Right Panel"), SerializeField] private UIGameItemResult _gameItemResultPrefab;
    [TabGroup("Right Panel"), SerializeField] private float _gameItemResultStartDelay = 1f;
    [TabGroup("Right Panel"), SerializeField] private float _gameItemResultSpawnDelay = 0.2f;
    [TabGroup("Right Panel"), SerializeField] private float _gameItemResultIncreaseDelay = 0.1f;
    [TabGroup("Right Panel"), SerializeField] private int _gameItemResultIncreaseRate = 3;

    [BoxGroup("Navigate Buttons"), SerializeField] private GameObject _navigateButtons;
    [BoxGroup("Navigate Buttons"), SerializeField] private Button _forwardButton;

    private bool _isWin;
    private bool _finishMark;
    private UIGameItemResult[] _gameItemResults;

    public void OnGameFinish(bool isWin) {
        _isWin = isWin;

        _forwardButton.interactable = _isWin;
        _scoreBackgroundCanvasGroup.alpha = 0f;
        _scoreBackgroundImage.color = _isWin ? _scoreBackgroundPassedColor : _scoreBackgroundFailedColor;
        _certificateStamp.sprite = _isWin ? _passedStamp : _failedStamp;
        _scoreText.text = "";
        _objectiveText.text = MainGameplayController.Instance.ScoreObjective.ToString();

        StartCoroutine(GameFinishCoroutine());
    }

    private IEnumerator GameFinishCoroutine() {
        _timeOutAnim.SetTrigger("open");

        yield return new WaitForSeconds(_timeOutDuration);

        _finishGameUI.Open();

        yield return new WaitForSeconds(_finishPanelWaitTime);

        IEnumerator enumerator(Coroutine coroutine, Action finishAction) {
            while (coroutine != null && !_finishMark && !Input.GetKeyDown(KeyCode.Mouse0)) {
                yield return null;
            }

            if (coroutine != null) {
                StopCoroutine(coroutine);
            }

            finishAction?.Invoke();
        }

        yield return StartCoroutine(enumerator(StartCoroutine(ShowScoreCoroutine()), ShowScoreFinish));
        Debug.Log("Complete score");
        yield return StartCoroutine(enumerator(StartCoroutine(StampCoroutine()), StampFinish));
        Debug.Log("Complete stamp");

        _finishMark = false;

        //StartCoroutine(enumerator(StartCoroutine(LeftPanelCoroutine()), LeftPanelFinish));
        Coroutine leftCoroutine = StartCoroutine(LeftPanelCoroutine());
        //StartCoroutine(enumerator(StartCoroutine(RightPanelCoroutine()), RightPanelFinish));
        Coroutine rightCoroutine = StartCoroutine(RightPanelCoroutine());

        _navigateButtons.SetActive(true);

        while (!Input.GetKeyDown(KeyCode.Mouse0)) {
            yield return null;
        }

        if (leftCoroutine != null) {
            StopCoroutine(leftCoroutine);
            LeftPanelFinish();
        }

        if (rightCoroutine != null) {
            StopCoroutine(rightCoroutine);
            RightPanelFinish();
        }

        //coroutine = StartCoroutine(StampCoroutine());
        //while (coroutine != null && !Input.GetKeyDown(KeyCode.Mouse0)) {
        //    yield return null;
        //}

        //coroutine = StartCoroutine(LeftPanelCoroutine());
        //while (coroutine != null && !Input.GetKeyDown(KeyCode.Mouse0)) {
        //    yield return null;
        //}
    }

    private IEnumerator IncreaseAmountCoroutine(TMP_Text text, int current, int target, int increaseRate, float delay) {
        do {
            current += increaseRate;
            if (current > target) {
                current = target;
            }

            text.text = current.ToString();

            yield return new WaitForSeconds(delay);
        } while (current < target);
    }

    private IEnumerator ShowScoreCoroutine() {
        _finishMark = false;
        yield return new WaitForSeconds(_scoreIncreaseDelay);

        _objectiveText.text = MainGameplayController.Instance.ScoreObjective.ToString();

        yield return StartCoroutine(IncreaseAmountCoroutine(_scoreText, 0, MainGameplayController.Instance.CurrentScore, _scoreIncreaseRate, _scoreIncreaseDelay));

        yield return _scoreBackgroundCanvasGroup.DOFade(1f, _scoreBackgroundFadeInDuration).SetEase(Ease.Linear).WaitForCompletion();

        _finishMark = true;
    }

    private void ShowScoreFinish() {
        _objectiveText.text = MainGameplayController.Instance.ScoreObjective.ToString();
        _scoreText.text = MainGameplayController.Instance.CurrentScore.ToString();
    }

    private IEnumerator StampCoroutine() {
        _finishMark = false;
        yield return new WaitForSeconds(_stampStartDelay);

        _stampAnim.SetTrigger("active");

        yield return new WaitForSeconds(_stampEndDelay);
        _finishMark = true;
    }

    private void StampFinish() {
        _stampAnim.SetTrigger("finish");
    }

    private IEnumerator LeftPanelCoroutine() {
        yield return new WaitForSeconds(_leftPanelStartDelay);

        Debug.Log($"Ultimate: {MainGameplayController.Instance.NumberOfUltimateUsed}");

        _usedUltimate.SetEnable(true);
        yield return StartCoroutine(IncreaseAmountCoroutine(_usedUltimate.AmountText, 0, MainGameplayController.Instance.NumberOfUltimateUsed, _leftPanelAmountIncreaseRate, _leftPanelAmountIncreaseDelay));
        _longestCombo.SetEnable(true);
        yield return StartCoroutine(IncreaseAmountCoroutine(_longestCombo.AmountText, 0, MainGameplayController.Instance.LongestCombo, _leftPanelAmountIncreaseRate, _leftPanelAmountIncreaseDelay));
        _highestMatch.SetEnable(true);
        yield return StartCoroutine(IncreaseAmountCoroutine(_highestMatch.AmountText, 0, MainGameplayController.Instance.HighestMatch, _leftPanelAmountIncreaseRate, _leftPanelAmountIncreaseDelay));

        LeftPanelFinish();
    }

    private void LeftPanelFinish() {
        Debug.Log($"Ultimate: {MainGameplayController.Instance.NumberOfUltimateUsed}");
        _usedUltimate.SetEnable(true);
        _usedUltimate.AmountText.text = MainGameplayController.Instance.NumberOfUltimateUsed.ToString();
        _longestCombo.SetEnable(true);
        _longestCombo.AmountText.text = MainGameplayController.Instance.LongestCombo.ToString();
        _usedUltimate.SetEnable(true);
        _usedUltimate.AmountText.text = MainGameplayController.Instance.HighestMatch.ToString();
    }

    private IEnumerator RightPanelCoroutine() {
        yield return new WaitForSeconds(_gameItemResultStartDelay);

        var itemsCollected = MainGameplayController.Instance.ItemsCollected;
        Coroutine[] coroutines = new Coroutine[itemsCollected.Length];
        _gameItemResults = new UIGameItemResult[itemsCollected.Length];
        int index = 0;

        foreach (var item in itemsCollected ) {
            _gameItemResults[index] = Instantiate(_gameItemResultPrefab, _rightPanel);
            var container = _gameItemResults[index];
            container.SetEnable(true);
            container.IconImage.sprite = item.itemSprite;
            coroutines[index++] = StartCoroutine(IncreaseAmountCoroutine(container.Text, 0, item.amount, _gameItemResultIncreaseRate, _gameItemResultIncreaseDelay));

            yield return new WaitForSeconds(_gameItemResultSpawnDelay);
        }

        foreach (var coroutine in coroutines ) {
            if (coroutine != null) {
                yield return coroutine;
            }
        }

        RightPanelFinish();
    }

    private void RightPanelFinish() {
        var itemsCollected = MainGameplayController.Instance.ItemsCollected;
        _gameItemResults ??= new UIGameItemResult[itemsCollected.Length];

        for (int i = 0; i < itemsCollected.Length; i++) {
            if (_gameItemResults[i] == null) {
                _gameItemResults[i] = Instantiate(_gameItemResultPrefab, _rightPanel);
            }

            _gameItemResults[i].SetEnable(true);
            _gameItemResults[i].IconImage.sprite = itemsCollected[i].itemSprite;
            _gameItemResults[i].Text.text = itemsCollected[i].amount.ToString();
        }
    }
}
