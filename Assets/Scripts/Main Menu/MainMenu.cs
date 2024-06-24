using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public static bool goDirectlyToGameBoardView;

    [BoxGroup("Start Spawn"), SerializeField] private float _spawnDelay = 0.05f;
    [BoxGroup("Start Spawn"), SerializeField] private int _spawnAmountOnEachDelay = 5;
    [BoxGroup("Start Spawn"), SerializeField] private GameObject[] _startSpawnRandomly;

    [BoxGroup("Canvas"), SerializeField] private GameObject _canvas;
    [BoxGroup("Canvas"), SerializeField] private float _delaySpawnCanvas = 3f;

    [TabGroup("Board"), SerializeField] private OpenCloseAnimTrigger _boardAnimTrigger;
    [TabGroup("Board"), SerializeField] private OpenCloseAnimTrigger _tapNextAnimTrigger;
    [TabGroup("Board"), SerializeField] private float _backToBoardTimer = 10f;

    [TabGroup("Title"), SerializeField] private Animator _titleAnim;
    [TabGroup("Title"), SerializeField] private OpenCloseAnimTrigger[] _treeAnimTriggers;

    [TabGroup("Game"), SerializeField] private OpenCloseAnimTrigger _gameAnimTrigger;
    [TabGroup("Game"), SerializeField] private Transform _levelContainer;
    [TabGroup("Game"), SerializeField] private float _hideYPosition = 4.565329551696777f;
    [TabGroup("Game"), SerializeField] private float _hideLevelTime = 0.4f;
    [TabGroup("Game"), SerializeField] private OpenCloseAnimTrigger[] _animTriggersToClose;

    private bool _isInChainButtons;
    private float _currentBackTimer;

    void Start() {
        PlayerData.Level = 3;
        LevelContainerSetup();
        StartCoroutine(StartMenuCoroutine());
        StartCoroutine(DelaySpawnCanvasCoroutine());
    }

    private void Update() {
        if (_isInChainButtons) {
            if (UIValidator.IsAnyUIOpen) {
                _currentBackTimer = _backToBoardTimer;
                return;
            }

            _currentBackTimer -= Time.deltaTime;

            if (_currentBackTimer <= 0f) {
                _isInChainButtons = false;
                ExitChainButtonsToTitle();
            }
        }
    }

    [Button]
    public void SetActiveTrueAll() {
        foreach (var obj in _startSpawnRandomly) {
            obj.SetActive(true);
        }
    }

    [Button]
    public void SetActiveFalseAll() {
        foreach (var obj in _startSpawnRandomly) {
            obj.SetActive(false);
        }
    }

    private IEnumerator StartMenuCoroutine() {
        SetActiveFalseAll();

        if (goDirectlyToGameBoardView) {
            foreach (GameObject objectSpawn in _startSpawnRandomly) {
                objectSpawn.SetActive(true);
            }

            yield break;
        }

        int rand = Random.Range(0, _startSpawnRandomly.Length);
        int index = rand;
        int currentAmount = 0;

        while (!_startSpawnRandomly[index].activeSelf) {
            _startSpawnRandomly[index].SetActive(true);

            index += rand;
            if (index >= _startSpawnRandomly.Length) {
                index -= _startSpawnRandomly.Length;
            }

            currentAmount++;

            if (currentAmount < _spawnAmountOnEachDelay) {
                continue;
            } else {
                currentAmount = 0;
            }

            yield return new WaitForSeconds(_spawnDelay);
        }

        for (int i = 0; i < _startSpawnRandomly.Length; i++) {
            if (_startSpawnRandomly[i].activeSelf) {
                continue;
            }

            _startSpawnRandomly[i].SetActive(true);

            currentAmount++;

            if (currentAmount < _spawnAmountOnEachDelay) {
                continue;
            } else {
                currentAmount = 0;
            }

            yield return new WaitForSeconds(_spawnDelay);
        }
    }

    private void LevelContainerSetup() {
        var levels = _levelContainer.GetComponentsInChildren<LevelButton>(true);
        int currentLevel = PlayerData.Level;

        for (int i = 0; i < levels.Length; i++) {
            levels[i].SetLevel(i);

            if (i <= currentLevel) {
                levels[i].IsLocked = false;
            } else {
                levels[i].IsLocked = true;
            }
        }
    }

    public void GoDirectlyToGamePanel() {
        SetTitle(false, true);
        //SetChainButtons(false);
        SetGamePanel(true);
    }

    private IEnumerator DelaySpawnCanvasCoroutine() {
        if (goDirectlyToGameBoardView) {
            _canvas.SetActive(true);
            GoDirectlyToGamePanel();
            yield break;
        }

        _canvas.SetActive(false);

        yield return new WaitForSeconds(_delaySpawnCanvas);

        _canvas.SetActive(true);
    }

    public void ExitTitleToChainButtons() {
        StartCoroutine(ExitTitleToChainButtonsCoroutine());
    }

    private IEnumerator ExitTitleToChainButtonsCoroutine() {
        SetTitle(false);

        yield return new WaitForSeconds(1f);

        SetChainButtons(true);
    }

    public void ExitChainButtonsToTitle() {
        StartCoroutine(ExitChainButtonsToTitleCoroutine());
    }

    private IEnumerator ExitChainButtonsToTitleCoroutine() {
        SetChainButtons(false);

        yield return new WaitForSeconds(1f);

        SetTitle(true);
    }

    public void ExitChainButtonsToGame() {
        StartCoroutine(ExitChainButtonsToGameCoroutine());
    }

    private IEnumerator ExitChainButtonsToGameCoroutine() {
        SetChainButtons(false);
        
        foreach (var trigger in _treeAnimTriggers) {
            trigger.Close();
        }

        yield return new WaitForSeconds(1f);

        SetGamePanel(true);
    }

    public void ExitGameToChainButtons() {
        StartCoroutine(ExitGameToChainButtonsCoroutine());
    }

    private IEnumerator ExitGameToChainButtonsCoroutine() {
        SetGamePanel(false);
        _levelContainer.DOMoveY(_hideYPosition, _hideLevelTime);

        yield return new WaitForSeconds(1f);

        SetChainButtons(true);

        foreach (var trigger in _treeAnimTriggers) {
            trigger.Open();
        }
    }

    public void SetTitle(bool enabled, bool immediateClose = false) {
        if (enabled) {
            _boardAnimTrigger.Open();
            _tapNextAnimTrigger.Open();
        } else {
            if (immediateClose) {
                _boardAnimTrigger.Anim.SetTrigger("immediateClose");
            } else {
                _boardAnimTrigger.Close();
            }
            _tapNextAnimTrigger.Close();
        }
    }

    public void SetChainButtons(bool enabled) {
        if (enabled) {
            _titleAnim.SetTrigger("enterButtons");
            _isInChainButtons = true;
            _currentBackTimer = _backToBoardTimer;
        } else {
            _titleAnim.SetTrigger("exitButtons");
            _isInChainButtons = false;
        }
    }

    public void SetGamePanel(bool enabled) {
        if (enabled) {
            _gameAnimTrigger.Open();
        } else {
            _gameAnimTrigger.Close();
            foreach (var trigger in _animTriggersToClose) {
                trigger.Close();
            }
        }
    }

    public void PressGameBoardBackButton() {
        if (ShopMenu.Instance.IsEnabled) {
            ShopMenu.Instance.Close();
        } else {
            ExitGameToChainButtons();
        }
    }

    public void OpenSettings() {
        SettingsController.Instance.Open();
    }

    public void OpenShop() {
        ShopMenu.Instance.Open();
    }

    public void ChangeGameScene() {
        SceneController.Instance.ChangeGameplayScene();
    }

    public void QuitGame() {
        Application.Quit();
    }
}
