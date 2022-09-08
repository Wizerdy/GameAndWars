using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsBoxEngine;

public class MultiSpriteLed : MonoBehaviour {
    [SerializeField] List<SpriteLed> _sprites;
    [SerializeField] int _current = 0;
    [SerializeField] bool _isActive = false;

    public int Current => _current;
    public bool Active { get => _isActive; set { _sprites[_current].Active = value; _isActive = value; } }

    void Awake() {
        for (int i = 0; i < _sprites.Count; i++) {
            _sprites[i].Active = false;
        }
        _sprites[_current].Active = _isActive;
    }

    public void Next() {
        Switch((_current + 1) % _sprites.Count);
    }

    public void Switch(int index) {
        _sprites[_current].Active = false;
        _current = index;
        _sprites[_current].Active = _isActive;
    }

    public void ShowAll(bool state) {
        for (int i = 0; i < _sprites.Count; i++) {
            _sprites[i].Active = state;
        }
    }
}
