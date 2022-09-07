using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteLed : MonoBehaviour {
    [SerializeField] SpriteRenderer _sprite;
    [SerializeField] bool _isActive = false;
    [SerializeField] float _speed = 10f;
    [SerializeField] Color _activeColor = Color.black;
    [SerializeField] Color _deactiveColor = Color.clear;

    float _percentage = 0f;

    Coroutine _routine_Display = null;

    public bool Active { get => _isActive; set => _isActive = value; }

    private void Reset() {
        _sprite = GetComponent<SpriteRenderer>();
    }

    public void ActiveFor(float time) {
        if (_routine_Display != null) { StopCoroutine(_routine_Display); }
        _routine_Display = StartCoroutine(DisplayFor(time, true));
    }

    IEnumerator DisplayFor(float time, bool state) {
        Active = state;
        yield return new WaitForSeconds(time);
        Active = !state;
    }

    void Update() {
        float delta = Time.deltaTime * _speed * (_isActive ? 1 : -1);
        _percentage += delta;
        _percentage = Mathf.Clamp01(_percentage);
        _sprite.color = Color.Lerp(_deactiveColor, _activeColor, _percentage);
    }
}
