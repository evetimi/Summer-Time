using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameplayController : MonoBehaviourSingleton<MainGameplayController>
{
    [BoxGroup("Setup"), SerializeField] private GameItem _itemPrefab;
    [BoxGroup("Setup"), SerializeField] private GameSkin _usedGameSkin;
    [BoxGroup("Setup"), SerializeField] private GameItemContainer[] _topBoxes;
    [BoxGroup("Setup"), SerializeField] private int _amountOfUniqueItem;
    [BoxGroup("Setup"), SerializeField] private float _itemMoveTime = 0.2f;
    [BoxGroup("Setup"), SerializeField] private float _pointerClickDelay = 0.2f;

    [BoxGroup("Board"), SerializeField] private Transform _boardItemContainer;
    [BoxGroup("Board"), SerializeField] private Transform _itemContainer;
    [BoxGroup("Board"), SerializeField] private Vector2Int _gridAmount = new Vector2Int(7, 7);

    private Sprite[] _itemSpriteInUse;
    private GameItemContainer[,] _gameItemContainers;

    public bool CanDragItem => GameItem.UnreadyItemAmount == 0;
    public float ItemMoveTime => _itemMoveTime;
    public bool CanClickPointer => _currentPointerClickTimer <= 0f;

    private float _currentPointerClickTimer;

    private void Start() {
        _itemSpriteInUse = _usedGameSkin.RandomChooseSprite(_amountOfUniqueItem);

        GenerateGameItemContainer();

        StartCoroutine(GenerateBoard());
    }

    private void Update() {
        if (_currentPointerClickTimer > 0f) {
            _currentPointerClickTimer -= Time.deltaTime;
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

        foreach (var topBox in _topBoxes) {
            GenerateNewGameItem(topBox);
        }
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
        bool item1Result = CheckItemFinish(item1.GridPosition.x, item1.GridPosition.y);
        bool item2Result = CheckItemFinish(item2.GridPosition.x, item2.GridPosition.y);

        if (!item1Result && !item2Result) {
            GameItem.Swap(item1, item2, false);
        }
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
            for (int i = 0; i < _gridAmount.x; i++) {
                for (int j = 0; j < _gridAmount.y; j++) {
                    CheckItem(i, j);
                }
            }

            return true;
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

        RecursiveCheckHorizontal(x, y, item, false);
        if (_successAmount >= 2) { // >= 2 because it starts from -1 (means it needs to be >= 3)
            horizontalAmount = _successAmount;
        } else {
            _successAmount = 0;
        }

        RecursiveCheckVertical(x, y, item, false);

        if (horizontalAmount >= 0) {
            RecursiveCheckHorizontal(x, y, item, true);

            if (_successAmount - horizontalAmount >= 3) {
                RecursiveCheckVertical(x, y, item, true);
            }
        } else if (_successAmount >= 3) {
            RecursiveCheckVertical(x, y, item, true);
        } else {
            // no items matched
            return false;
        }

        return true;
    }

    private void RecursiveCheckHorizontal(int x, int y, GameItem itemToCheck, bool doFinishItem) {
        var currentGameItem = GetGameItemContainer(x, y).ContainItem;

        if (!doFinishItem) {
            _successAmount++;
            _alreadyChecked[x, y] = true;
        } else {
            currentGameItem.FinishItem();
        }

        GameItemContainer right = GetGameItemContainer(x + 1, y);
        GameItemContainer left = GetGameItemContainer(x - 1, y);

        if (right != null && (doFinishItem || !_alreadyChecked[x + 1, y]) && right.ContainItem != null && right.ContainItem.ItemId == currentGameItem.ItemId) {
            RecursiveCheckHorizontal(x + 1, y, currentGameItem, doFinishItem);
        }
        if (left != null && (doFinishItem || !_alreadyChecked[x - 1, y]) && left.ContainItem != null && left.ContainItem.ItemId == currentGameItem.ItemId) {
            RecursiveCheckHorizontal(x - 1, y, currentGameItem, doFinishItem);
        }
    }

    private void RecursiveCheckVertical(int x, int y, GameItem itemToCheck, bool doFinishItem) {
        var currentGameItem = GetGameItemContainer(x, y).ContainItem;

        if (!doFinishItem) {
            _successAmount++;
            _alreadyChecked[x, y] = true;
        } else {
            currentGameItem.FinishItem();
        }

        GameItemContainer up = GetGameItemContainer(x, y + 1);
        GameItemContainer down = GetGameItemContainer(x, y - 1);

        if (up != null && (doFinishItem || !_alreadyChecked[x, y + 1]) && up.ContainItem != null && up.ContainItem.ItemId == currentGameItem.ItemId) {
            RecursiveCheckHorizontal(x, y + 1, currentGameItem, doFinishItem);
        }
        if (down != null && (doFinishItem || !_alreadyChecked[x, y - 1]) && down.ContainItem != null && down.ContainItem.ItemId == currentGameItem.ItemId) {
            RecursiveCheckHorizontal(x, y - 1, currentGameItem, doFinishItem);
        }
    }
}
