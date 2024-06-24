using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISound : MonoBehaviourService<UISound>
{
    [SerializeField] private AudioSource _audioSourceToPlayOnChangeSelectable;
    [SerializeField] private string _click1Tag;
    [SerializeField] private AudioClip _click1Clip;
    [SerializeField] private string _click2Tag;
    [SerializeField] private AudioClip _click2Clip;

    private GameObject _lastSelect;
    private Button _lastButton;

    void Start()
    {
        _lastSelect = EventSystem.current.currentSelectedGameObject;
    }

    void Update()
    {
        GameObject current = EventSystem.current.currentSelectedGameObject;
        if (current == null || !current.activeSelf) {
            _lastSelect = null;
            return;
        }

        if (_lastSelect != current) {
            if (_lastButton) {
                _lastButton.onClick.RemoveListener(ButtonClick);
            }

            _lastSelect = current;

            if (current.TryGetComponent<Button>(out var button)) {
                _lastButton = button;
                button.onClick.AddListener(ButtonClick);
            }
        }
    }

    private void ButtonClick() {
        PlaySound(_lastButton.tag);
    }

    public void PlaySound(string tag) {
        if (tag == _click1Tag) {
            _audioSourceToPlayOnChangeSelectable.clip = _click1Clip;
        } else if (tag == _click2Tag) {
            _audioSourceToPlayOnChangeSelectable.clip = _click2Clip;
        } else {
            _audioSourceToPlayOnChangeSelectable.clip = _click1Clip;
        }

        _audioSourceToPlayOnChangeSelectable.Play();
    }
}
