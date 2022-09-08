using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LifeManager : MonoBehaviour {
    [SerializeField] List<SpriteLed> _healthIcons;
    [SerializeField] UnityEvent _onLose;

    public event UnityAction OnLose { add => _onLose.AddListener(value); remove => _onLose.RemoveListener(value); }

    int _count;

    void Start() {
        _count = _healthIcons.Count;
        for (int i = 0; i < _healthIcons.Count; i++) {
            if (_healthIcons != null) {
                _healthIcons[i].Active = true;
            }
        }
    }

    public void LoseLife() {
        --_count;
        _healthIcons[_count].Active = false;
        if (_count <= 0) {
            _count = Mathf.Max(_count, 0);
            _onLose?.Invoke();
        }
    }
}
