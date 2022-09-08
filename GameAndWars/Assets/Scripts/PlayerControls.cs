using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsBoxEngine;

public class PlayerControls : MonoBehaviour {
    [SerializeField] List<MultiSpriteLed> _playerPositions; // 0 : 0, 0 / 1 : 1, 0 / 2 : 0, 1 : 3 : 1, 1
    [SerializeField, HideInInspector] float _jumpTime = 1f;
    [SerializeField] Vector2 _controlDelayTime = Vector2.one;
    [SerializeField] float _animationTime = 0.3f;
    [SerializeField] bool _canMove = true;

    KeyCode _lastKeyPressed;
    Vector2Int _lastInput = Vector2Int.zero;
    Vector2Int _playerPosition = Vector2Int.zero;

    bool _jumping = false;

    public Vector2Int Position => _playerPosition;
    public bool CanMove { get => _canMove; set => _canMove = true; }

    private Coroutine _delayCoroutine;

    void Start() {
        ActivePlayerPosition(_playerPosition);
    }

    public void StartGame() {
        ActivePlayerPosition(_playerPosition);
        CanMove = true;
    }

    void Update() {
        if (!_canMove) { return; }

        if (Input.GetKeyDown(KeyCode.Z)) {
            ButtonPressed(KeyCode.Z);
        } else if (Input.GetKeyDown(KeyCode.Q)) {
            ButtonPressed(KeyCode.Q);
        } else if (Input.GetKeyDown(KeyCode.D)) {
            ButtonPressed(KeyCode.D);
        } else if (Input.GetKeyDown(KeyCode.S)) {
            ButtonPressed(KeyCode.S);
        }

        if (Input.GetKeyUp(_lastKeyPressed)) {
            if (_delayCoroutine != null) { StopCoroutine(_delayCoroutine); }
        }

        //Vector2Int direction = Vector2Int.FloorToInt(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));

        //if (direction.x != 0 && _lastInput.x != direction.x) {
        //    ActivePlayerPosition(_playerPosition.Override(Mathf.Clamp(_playerPosition.x + direction.x, 0, 1), Axis.X));
        //} else if (/*!_jumping &&*/ direction.y != 0 && _lastInput.y != direction.y) {
        //    ActivePlayerPosition(_playerPosition.Override(Mathf.Clamp(_playerPosition.y + direction.y, 0, 1), Axis.Y));
        //    //Jump();
        //}

        //_lastInput = direction;
    }

    void ActivePlayerPosition(Vector2Int position) {
        _playerPositions[VectorToInt(_playerPosition)].Active = false;
        _playerPositions[VectorToInt(position)].Active = true;
        _playerPosition = position;
    }

    public void Anim() {
        int index = VectorToInt(_playerPosition);
        _playerPositions[index].Next();
        StartCoroutine(Tools.Delay(() => _playerPositions[index].Next(), _animationTime));
    }

    private void ButtonPressed(KeyCode key) {
        if (_delayCoroutine != null) { StopCoroutine(_delayCoroutine); }
        _delayCoroutine = StartCoroutine(PushedDelay(key));
        _lastKeyPressed = key;
    }

    int VectorToInt(Vector2Int position) {
        int output = 0;
        output += position.x;
        output += position.y * 2;
        return output;
    }

    void Jump() {
        _jumping = true;
        StartCoroutine(Tools.Delay(() => { _jumping = false; ActivePlayerPosition(_playerPosition.Override(0, Axis.Y)); }, _jumpTime));
    }

    private IEnumerator PushedDelay(KeyCode key) {
        yield return new WaitForSeconds(Random.Range(_controlDelayTime.x, _controlDelayTime.y));
        switch (key) {
            case KeyCode.Z:
                ActivePlayerPosition(new Vector2Int(0, 1));
                break;

            case KeyCode.S:
                ActivePlayerPosition(new Vector2Int(1, 0));
                break;

            case KeyCode.Q:
                ActivePlayerPosition(new Vector2Int(0, 0));
                break;

            case KeyCode.D:
                ActivePlayerPosition(new Vector2Int(1, 1));
                break;
        }
    }
}
