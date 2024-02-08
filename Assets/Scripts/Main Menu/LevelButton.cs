using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [BoxGroup("Properties"), SerializeField] private Image _buttonImage;
    [BoxGroup("Properties"), SerializeField] private Sprite _normalImage;
    [BoxGroup("Properties"), SerializeField] private bool _isLocked;

    [BoxGroup("Locked"), SerializeField] private Sprite _greyscaleButtonSprite;
    [BoxGroup("Locked"), RequiredIn(PrefabKind.PrefabAsset), SerializeField] private GameObject _chainPrefab;

    [BoxGroup("Level"), SerializeField] private TMP_Text _levelText;
    [BoxGroup("Level")] public int _levelIndex;

    private GameObject _chainInstance;

    public bool IsLocked {
        get => _isLocked;
        set {
            _isLocked = value;

            if (_isLocked) {
                _buttonImage.sprite = _greyscaleButtonSprite;
                if (_chainInstance == null) {
                    _chainInstance = Instantiate(_chainPrefab, transform);
                    _chainInstance.transform.localPosition = Vector3.zero;
                    _chainInstance.transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
                }
            } else {
                _buttonImage.sprite = _normalImage;
            }

            if (_chainInstance != null) {
                _chainInstance.SetActive(_isLocked);
            }
        }
    }

    private void Reset() {
        _buttonImage = GetComponent<Image>();
    }

    private void OnValidate() {
        SetLevel(_levelIndex);
    }

    public void SetLevel(int index) {
        if (_levelText != null) {
            _levelText.text = (index + 1).ToString();
        }
    }
}
