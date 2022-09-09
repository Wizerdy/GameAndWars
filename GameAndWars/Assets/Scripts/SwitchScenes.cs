using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScenes : MonoBehaviour {
    static SwitchScenes _instance;

    [SerializeField] float _pressTime = 5f;

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

        if (_inputPressed[0] && _inputPressed[1] && (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.S))) {
            _routine_InputPressed = StartCoroutine(InputPressed());
        }

        if (Input.GetKeyUp(KeyCode.Q) || Input.GetKeyUp(KeyCode.S)) {
            StopCoroutine(_routine_InputPressed);
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

    IEnumerator InputPressed() {
        yield return new WaitForSeconds(_pressTime);
        AudioManager.instance.Play("Scene_Change");
        SceneManager.LoadScene(++_currentScene % SceneManager.sceneCountInBuildSettings);
    }
}
