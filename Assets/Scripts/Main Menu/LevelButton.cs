using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [BoxGroup("Properties"), SerializeField] private Image _buttonImage;
    [BoxGroup("Properties"), SerializeField] private Sprite _normalImage;
    [BoxGroup("Properties"), SerializeField] private bool _isLocked;

    [BoxGroup("Locked"), SerializeField] private Sprite _greyscaleButtonSprite;
    [BoxGroup("Locked"), RequiredIn(PrefabKind.PrefabAsset), SerializeField] private GameObject _chainPrefab;

    private GameObject _chainInstance;

    public bool IsLocked {
        get => _isLocked;
        set {
            _isLocked = value;

            if (_isLocked) {
                _buttonImage.sprite = _greyscaleButtonSprite;
                if (!_chainInstance) {
                    _chainInstance = Instantiate(_chainPrefab, transform);
                    _chainInstance.transform.localPosition = Vector3.zero;
                }
            } else {
                _buttonImage.sprite = _normalImage;
            }

            if (_chainInstance) {
                _chainInstance.SetActive(!_isLocked);
            }
        }
    }

    private void Reset() {
        _buttonImage = GetComponent<Image>();
    }
}
