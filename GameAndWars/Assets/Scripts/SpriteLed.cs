using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteLed : MonoBehaviour {
    [SerializeField] SpriteRenderer _sprite;
    [SerializeField] Image _image;
    [SerializeField] bool _isActive = false;
    [SerializeField] float _speed = 7f;
    [SerializeField] Color _activeColor = Color.black;
    [SerializeField] Color _deactiveColor = new Color(0f, 0f, 0f, 0.04f);

    float _percentage = 0f;

    Coroutine _routine_Display = null;

    public bool Active { get => _isActive; set => _isActive = value; }

    private void Reset() {
        _sprite = GetComponent<SpriteRenderer>();
        _image = GetComponent<Image>();
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
        if (_sprite != null) { _sprite.color = Color.Lerp(_deactiveColor, _activeColor, _percentage); }
        if (_image != null) { _image.color = Color.Lerp(_deactiveColor, _activeColor, _percentage); }
    }
}
