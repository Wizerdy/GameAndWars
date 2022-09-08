using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Grenade : MonoBehaviour {
    [SerializeField] List<SpriteLed> _steps;
    [SerializeField] float _speed = 1f;

    [SerializeField] UnityEvent _onExplode;
    [SerializeField] UnityEvent<Grenade> _onStep;
    [SerializeField] UnityEvent _onReflect;
    [SerializeField] UnityEvent _onExplodeBack;

    int _currentStep = -1;
    int _direction = 1;

    public bool Launched => _currentStep != -1;
    public bool LastStep => _currentStep == _steps.Count - 1;

    public event UnityAction OnExplode { add => _onExplode.AddListener(value); remove => _onExplode.RemoveListener(value); }
    public event UnityAction<Grenade> OnStep { add => _onStep.AddListener(value); remove => _onStep.RemoveListener(value); }
    public event UnityAction OnReflect { add => _onReflect.AddListener(value); remove => _onReflect.RemoveListener(value); }
    public event UnityAction OnExplodeBack { add => _onExplodeBack.AddListener(value); remove => _onExplodeBack.RemoveListener(value); }

    public void Launch() {
        if (_currentStep != -1) { return; }
        StartCoroutine(ILaunch());
    }

    IEnumerator ILaunch() {
        _direction = 1;
        ChangeStep(0);
        WaitForSeconds timer = new WaitForSeconds(_speed);
        while (!(_currentStep >= _steps.Count || _currentStep <= -1)) {
            yield return timer;
            _onStep?.Invoke(this);
            ChangeStep(_currentStep + _direction);
        }

        if (_currentStep >= _steps.Count) {
            _onExplode?.Invoke();
        } else if (_currentStep <= 0) {
            _onExplodeBack?.Invoke();
        }
        ChangeStep(-1);
    }

    public void Reflect() {
        _direction = -1;
        _onReflect?.Invoke();
    }

    public void ResetMe() {
        _direction = 1;
        _currentStep = -1;
        StopAllCoroutines();
        for (int i = 0; i < _steps.Count; i++) {
            _steps[i].Active = false;
        }
    }

    void ChangeStep(int step) {
        if (!(_currentStep >= _steps.Count || _currentStep < 0)) {
            _steps[_currentStep].Active = false;
        }
        if (!(step >= _steps.Count || step < 0)) {
            _steps[step].Active = true;
        }
        _currentStep = step;
    }
}
