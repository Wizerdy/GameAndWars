using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SwitchScenes : MonoBehaviour {
    public static SwitchScenes _instance;

    [SerializeField] float _pressTime = 5f;
    [SerializeField, HideInInspector] UnityEvent<int> _onChangeGame;

    public event UnityAction<int> OnChangeGame { add => _onChangeGame.AddListener(value); remove => _onChangeGame.RemoveListener(value); }
    public int GameIndex => _sceneIndex;

    int _sceneIndex = 1;

    int _currentScene = 0;
    bool[] _inputPressed = new bool[2];
    Coroutine _routine_InputPressed;

    void Awake() {
        if (_instance != null) {
            Destroy(gameObject);
        } else {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        for (int i = 0; i < _inputPressed.Length; i++) {
            _inputPressed[i] = false;
        }
    }

    void Update() {
        _inputPressed[0] = Input.GetKey(KeyCode.Q);
        _inputPressed[1] = Input.GetKey(KeyCode.S);

        if (SceneManager.GetActiveScene().buildIndex == 0) { // ON the game
            if (_inputPressed[0] && _inputPressed[1] && (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.S))) {
                ChangeScene(_sceneIndex);
            }

            if (Input.GetKeyDown(KeyCode.Z)) {
                _sceneIndex = 1;
                _onChangeGame?.Invoke(_sceneIndex);
            } else if (Input.GetKeyDown(KeyCode.D)) {
                _sceneIndex = 2;
                _onChangeGame?.Invoke(_sceneIndex);
            }
        } else { // OFF the game
            if (_inputPressed[0] && _inputPressed[1] && (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.S))) {
                _routine_InputPressed = StartCoroutine(ChangeSceneWithTime(0));
            }

            if (Input.GetKeyUp(KeyCode.Q) || Input.GetKeyUp(KeyCode.S)) {
                if (_routine_InputPressed != null) StopCoroutine(_routine_InputPressed);
            }
        }

        //if (Input.GetKeyDown(KeyCode.Q)) {
        //    _inputPressed[0] = true;
        //}
        //if (Input.GetKeyDown(KeyCode.S)) {
        //    _inputPressed[1] = true;
        //}

        //if (Input.GetKeyUp(KeyCode.Q)) {
        //    _inputPressed[0] = false;
        //}
        //if (Input.GetKeyUp(KeyCode.S)) {
        //    _inputPressed[1] = true;
        //}
    }

    IEnumerator ChangeSceneWithTime(int index) {
        yield return new WaitForSeconds(_pressTime);
        ChangeScene(index);
    }

    void ChangeScene(int index) {
        AudioManager.instance.Play("Scene_Change");
        //SceneManager.LoadScene(++_currentScene % SceneManager.sceneCountInBuildSettings);
        SceneManager.LoadScene(index);
    }
}
