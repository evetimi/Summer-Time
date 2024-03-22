using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MainGameplayController : MonoBehaviourSingleton<MainGameplayController>
{
    [BoxGroup("TEST"), SerializeField] private bool _enableTest;
    [BoxGroup("TEST"), SerializeField, EnableIf(nameof(_enableTest))] private LevelData _testLevelData;

    [TabGroup("Prefabs"), SerializeField] private GameItem _itemPrefab;
    [TabGroup("Prefabs"), SerializeField] private GameSkin _usedGameSkin;
    [TabGroup("Prefabs"), SerializeField] private GameItemContainer[] _topBoxes;
    [TabGroup("Prefabs"), SerializeField] private int _amountOfUniqueItem;
    [TabGroup("Prefabs"), SerializeField] private float _itemMoveTime = 0.2f;
    [TabGroup("Prefabs"), SerializeField] private float _pointerClickDelay = 0.2f;

    [TabGroup("Energy Path"), SerializeField] private EnergyPath _energyPathPrefab;
    [TabGroup("Energy Path"), SerializeField] private Transform _energyPathParent;
    [TabGroup("Energy Path"), SerializeField] private Transform _energyPathEndPoint;

    [TabGroup("Gear"), SerializeField] private float _rollSpeed = 20f;
    [TabGroup("Gear"), SerializeField] private RectTransform[] _gearsRollLeft;
    [TabGroup("Gear"), SerializeField] private RectTransform[] _gearsRollRight;

    [BoxGroup("Scoring"), SerializeField] private int _itemScoreAmount = 10;
    [BoxGroup("Scoring"), SerializeField] private int _energyScoreAmount = 10;

    [BoxGroup("Combo"), SerializeField] private UIComboPopup _comboPopup;
    [BoxGroup("Combo"), SerializeField] private UIComboText _comboText;

    [BoxGroup("Board"), SerializeField] private Transform _boardItemContainer;
    [BoxGroup("Board"), SerializeField] private Transform _itemContainer;
    [BoxGroup("Board"), SerializeField] private Vector2Int _gridAmount = new Vector2Int(7, 7);

    [BoxGroup("Ultimate"), SerializeField] private UltimateContainer _ultimateContainer;

    [BoxGroup("Tsunami Event"), SerializeField] private TsunamiEvent _tsunamiEvent;
    [BoxGroup("Tsunami Event"), SerializeField] private GameObject _tsunamiSpeechBubbleWarning;

    [BoxGroup("Events")] public UnityEvent<MainGameplayController> OnGenerateLevelCompleted;
    [BoxGroup("Events")] public UnityEvent<GameItem, int> OnScoreGotten;
    [BoxGroup("Events")] public UnityEvent<GameItem, int> OnEnergyGotten;
    [BoxGroup("Events")] public UnityEvent<int> OnComboChanged;
    [BoxGroup("Events")] public UnityEvent OnEventHas10sLeft;
    [BoxGroup("Events")] public UnityEvent OnEventHappened;

    [Button]
    public void ShowCurrentUnreadyItem() {
        Debug.Log($"Current Unready Item: {GameItem.UnreadyItemAmount}");
    }

    private LevelData _currentLevelData;
    private Sprite[] _itemSpriteInUse;
    private GameItemContainer[,] _gameItemContainers;

    public LevelData CurrentLevelData => _currentLevelData;
    public bool IsPlaying { get; private set; }
    public float GameTimer { get; private set; }
    public float EventHappensAt { get; private set; }
    public int ScoreObjective { get; private set; }
    public int CurrentScore { get; private set; }
    public int CurrentCombo { get; private set; }
    public bool CanDragItem => GameItem.UnreadyItemAmount == 0;
    public float ItemMoveTime => _itemMoveTime;

    private bool _lastUnreadyStatus;
    private bool _event10sNotified;
    private bool _gameEventCompleted;
    private Coroutine _resetComboCoroutine;

    private void Start() {
        _itemSpriteInUse = _usedGameSkin.RandomChooseSprite(_amountOfUniqueItem);

        GenerateGameItemContainer();

        StartCoroutine(GenerateBoard());
    }

    private void Update() {
        if (!IsPlaying) {
            return;
        }

        if (GameTimer > 0f) {
            GameTimer -= Time.deltaTime;
        } else {
            GameTimer = 0f;
        }

        if (EventHappensAt > 0f && !_gameEventCompleted) {
            GameEventUpdate();
        }

        if ((GameItem.UnreadyItemAmount > 0) && !_lastUnreadyStatus) {
            _lastUnreadyStatus = true;
        } else if ((GameItem.UnreadyItemAmount == 0) && _lastUnreadyStatus) {
            _lastUnreadyStatus = false;
            if (CheckItemFinish(-1, -1)) {
                CheckTopBoxes(0, 0);
            }
        }

        if (GameItem.UnreadyItemAmount > 0) {
            foreach (var gear in _gearsRollLeft) {
                gear.Rotate(Vector3.forward, _rollSpeed * Time.deltaTime);
            }

            foreach (var gear in _gearsRollRight) {
                gear.Rotate(Vector3.forward, -_rollSpeed * Time.deltaTime);
            }
        }
    }

    private void GameEventUpdate() {
        if (!_event10sNotified && GameTimer - EventHappensAt <= 10f) {
            if (_tsunamiSpeechBubbleWarning) {
                _tsunamiSpeechBubbleWarning.SetActive(true);
            }
            OnEventHas10sLeft?.Invoke();
            _event10sNotified = true;
        }

        if (GameTimer <= EventHappensAt && _tsunamiEvent) {
            _tsunamiEvent.SpawnTsunami();
            OnEventHappened?.Invoke();
            _gameEventCompleted = true;
        }
    }

    private void OnEnable() {
        GameItem.OnStartMoving += CheckTopBoxes;
        GameItem.OnItemsSwapped += OnItemsSwapped;
    }

    private void OnDisable() {
        GameItem.OnStartMoving -= CheckTopBoxes;
        GameItem.OnItemsSwapped -= OnItemsSwapped;
    }

    private IEnumerator GenerateBoard() {
        yield return null;

        if (_enableTest) {
            _currentLevelData = _testLevelData;
        } else {
            _currentLevelData = _testLevelData;
        }

        GameTimer = _currentLevelData.Timer;
        EventHappensAt = _currentLevelData.EventHappensAt;
        ScoreObjective = _currentLevelData.ScoreObjective;

        CurrentScore = 0;

        foreach (var topBox in _topBoxes) {
            GenerateNewGameItem(topBox);
        }

        IsPlaying = true;

        OnGenerateLevelCompleted?.Invoke(this);
    }

    private void GenerateGameItemContainer() {
        _gameItemContainers = new GameItemContainer[_gridAmount.x, _gridAmount.y];

        int x = 0;
        int y = 0;

        foreach (Transform child in _boardItemContainer) {
            if (x >= _gridAmount.x) {
                x = 0;
                y++;
            }

            _gameItemContainers[x, y] = child.GetComponent<GameItemContainer>();
            _gameItemContainers[x, y].GridPosition = new(x, y);

            x++;
        }
    }

    private void CheckTopBoxes(int x, int y) { // x and y is placeholder for the Action event
        foreach (var topBox in _topBoxes) {
            if (topBox.ContainItem == null) {
                GenerateNewGameItem(topBox);
            }
        }
    }

    private void GenerateNewGameItem(GameItemContainer aboveThisContainer) {
        GameItem newItem = Instantiate(_itemPrefab, _itemContainer);
        newItem.RegisterPosition(new Vector2Int(aboveThisContainer.GridPosition.x, aboveThisContainer.GridPosition.y + 1), true);
        aboveThisContainer.ContainItem = newItem;

        Vector3 position = new Vector3(
            aboveThisContainer.transform.localPosition.x,
            aboveThisContainer.transform.localPosition.y + aboveThisContainer.GetComponent<RectTransform>().rect.height,
            aboveThisContainer.transform.localPosition.z
        );

        newItem.transform.localPosition = position;

        int itemId = Random.Range(0, _itemSpriteInUse.Length);
        newItem.SetItem(_itemSpriteInUse[itemId], itemId);

        newItem.IsWorking = true;
    }

    public GameItemContainer GetGameItemContainer(int x, int y) {
        if (x < 0 || x >= _gridAmount.x) {
            return null;
        }

        if (y < 0 || y >= _gridAmount.y) {
            return null;
        }

        return _gameItemContainers[x, y];
    }

    private void OnItemsSwapped(GameItem item1, GameItem item2) {
        if (_resetComboCoroutine != null) {
            StopCoroutine(_resetComboCoroutine);
            SetCombo(0);
        }

        bool item1Result = CheckItemFinish(item1.GridPosition.x, item1.GridPosition.y);
        bool item2Result = CheckItemFinish(item2.GridPosition.x, item2.GridPosition.y);

        if (!item1Result && !item2Result) {
            GameItem.Swap(item1, item2, false);
        }

        CheckTopBoxes(0, 0);
    }

    private bool[,] _alreadyChecked;
    private int _successAmount;

    private void ResetChecking() {
        _successAmount = -1;

        if (_alreadyChecked == null) {
            _alreadyChecked = new bool[_gridAmount.x, _gridAmount.y];
        } else {
            for (int i = 0; i < _gridAmount.x; i++) {
                for (int j = 0; j < _gridAmount.y; j++) {
                    _alreadyChecked[i, j] = false;
                }
            }
        }
    }

    private bool CheckItemFinish(int startX, int startY) {
        if (startX < 0 || startY < 0 || startX >= _gridAmount.x || startY >= _gridAmount.y) {
            bool success = false;

            for (int i = 0; i < _gridAmount.x; i++) {
                for (int j = 0; j < _gridAmount.y; j++) {
                    if (CheckItem(i, j)) {
                        success = true;
                    }
                }
            }

            return success;
        } else {
            return CheckItem(startX, startY);
        }
    }

    private bool CheckItem(int x, int y) {
        GameItemContainer container = GetGameItemContainer(x, y);
        if (container == null) {
            return false;
        }

        GameItem item = container.ContainItem;
        if (item == null) {
            return false;
        }

        ResetChecking();

        int horizontalAmount = -1;
        int itemId = item.ItemId;

        RecursiveCheckHorizontal(x, y, itemId, false);
        if (_successAmount >= 2) { // >= 2 because it starts from -1 (means it needs to be >= 3)
            horizontalAmount = _successAmount;
        } else {
            _successAmount = 0;
        }

        RecursiveCheckVertical(x, y, itemId, false);

        Debug.Log($"Success: {_successAmount}");
        Debug.Log($"Horizontal: {horizontalAmount}");

        if (horizontalAmount >= 0) {
            RecursiveCheckHorizontal(x, y, itemId, true);

            if (_successAmount - horizontalAmount >= 3) {
                RecursiveCheckVertical(x, y, itemId, true);
            }
        } else if (_successAmount >= 3) {
            RecursiveCheckVertical(x, y, itemId, true);
        } else {
            // no items matched
            return false;
        }

        SetCombo(CurrentCombo + 1);

        // Do show match 4 or 5 here
        if (CurrentCombo == 1) {
            int matched = (_successAmount >= 3) ? _successAmount : horizontalAmount + 1;
            if (matched > 3) {
                _comboPopup.SpawnCombo(0);
                _comboText.PopMatch(matched);
            }
        } else {
            _comboPopup.SpawnCombo(CurrentCombo);
            _comboText.PopCombo(CurrentCombo);
        }

        _resetComboCoroutine = StartCoroutine(ResetComboCoroutine());

        return true;
    }

    private void RecursiveCheckHorizontal(int x, int y, int idToCheck, bool doFinishItem) {
        if (!doFinishItem) {
            _successAmount++;
            _alreadyChecked[x, y] = true;
        } else {
            FinishGameItem(GetGameItemContainer(x, y).ContainItem);
            _alreadyChecked[x, y] = false;
        }

        GameItemContainer right = GetGameItemContainer(x + 1, y);
        GameItemContainer left = GetGameItemContainer(x - 1, y);

        if (right != null && (doFinishItem == _alreadyChecked[x + 1, y]) && right.ContainItem != null && right.ContainItem.ItemId == idToCheck) {
            RecursiveCheckHorizontal(x + 1, y, idToCheck, doFinishItem);
        }
        if (left != null && (doFinishItem == _alreadyChecked[x - 1, y]) && left.ContainItem != null && left.ContainItem.ItemId == idToCheck) {
            RecursiveCheckHorizontal(x - 1, y, idToCheck, doFinishItem);
        }
    }

    private void RecursiveCheckVertical(int x, int y, int idToCheck, bool doFinishItem) {
        if (!doFinishItem) {
            _successAmount++;
            _alreadyChecked[x, y] = true;
        } else {
            FinishGameItem(GetGameItemContainer(x, y).ContainItem);
            _alreadyChecked[x, y] = false;
        }

        GameItemContainer up = GetGameItemContainer(x, y + 1);
        GameItemContainer down = GetGameItemContainer(x, y - 1);

        if (up != null && (doFinishItem == _alreadyChecked[x, y + 1]) && up.ContainItem != null && up.ContainItem.ItemId == idToCheck) {
            RecursiveCheckVertical(x, y + 1, idToCheck, doFinishItem);
        }
        if (down != null && (doFinishItem == _alreadyChecked[x, y - 1]) && down.ContainItem != null && down.ContainItem.ItemId == idToCheck) {
            RecursiveCheckVertical(x, y - 1, idToCheck, doFinishItem);
        }
    }

    public void FinishGameItem(GameItem gameItem) {
        if (gameItem == null)
            return;

        gameItem.FinishItem();
        CurrentScore += _itemScoreAmount;

        OnScoreGotten?.Invoke(gameItem, _itemScoreAmount);

        if (_ultimateContainer && !_ultimateContainer.IsUsingUltimate) {
            SpawnEnergyPath(gameItem.transform.position, _energyPathEndPoint.position);
            OnEnergyGotten?.Invoke(gameItem, _energyScoreAmount);
        }
    }

    public void SetCombo(int targetCombo) {
        Debug.Log("Combo: " + targetCombo);
        CurrentCombo = targetCombo;
        OnComboChanged?.Invoke(CurrentCombo);
    }

    private IEnumerator ResetComboCoroutine() {
        yield return new WaitUntil(() => GameItem.UnreadyItemAmount != 0);
        yield return new WaitUntil(() => GameItem.UnreadyItemAmount == 0);
        //SetCombo(0);
    }

    public void SpawnEnergyPath(Vector3 startPos, Vector3 endPos) {
        EnergyPath energyPath = Instantiate(_energyPathPrefab, _energyPathParent);
        energyPath.DoMovement(startPos, endPos);
    }

    [Button]
    public void ImmediatelyDestroyAllGameItem() {
        for (int x = 0; x < _gridAmount.x; x++) {
            for (int y = 0; y < _gridAmount.y; y++) {
                _gameItemContainers[x, y].DestroyItem();
            }
        }

        CheckTopBoxes(0, 0);
    }
}
