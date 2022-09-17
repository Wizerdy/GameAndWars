using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using ToolsBoxEngine;

public class SwitchScenes : MonoBehaviour {
    public static SwitchScenes _instance;

    [SerializeField] float _pressTime = 2f;
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
        //_inputPressed[0] = Input.GetKey(PlayerControls.DOWNLEFT);
        //_inputPressed[1] = Input.GetKey(PlayerControls.DOWNRIGHT);

        //if (SceneManager.GetActiveScene().buildIndex == 0) { // ON the game
        //    if (_inputPressed[0] && _inputPressed[1]) {
        //        ChangeScene(_sceneIndex);
        //    }

        //    if (Input.GetKeyDown(PlayerControls.UPLEFT)) {
        //        _sceneIndex = 1;
        //        _onChangeGame?.Invoke(_sceneIndex);
        //    } else if (Input.GetKeyDown(PlayerControls.UPRIGHT)) {
        //        _sceneIndex = 2;
        //        _onChangeGame?.Invoke(_sceneIndex);
        //    }
        //} else { // OFF the game
        //    if (_inputPressed[0] && _inputPressed[1] && (Input.GetKeyDown(PlayerControls.DOWNLEFT) || Input.GetKeyDown(PlayerControls.DOWNRIGHT))) {
        //        _routine_InputPressed = StartCoroutine(ChangeSceneWithTime(0));
        //    }

        //    if (Input.GetKeyUp(PlayerControls.DOWNLEFT) || Input.GetKeyUp(PlayerControls.DOWNRIGHT)) {
        //        if (_routine_InputPressed != null) StopCoroutine(_routine_InputPressed);
        //    }
        //}

        for (int i = 0; i < Input.touchCount; i++) {
            Touch touch = Input.GetTouch(i);
            //if (touch.phase != TouchPhase.Began) { continue; }
            //Debug.Log("Touched : " + touch.fingerId);
            Ray touchRay = Camera.main.ScreenPointToRay(touch.position.To3D(0f));
            //Debug.DrawLine(Vector2.zero, Camera.main.ScreenToWorldPoint(touch.position).To2D(), Color.blue, 5f);
            //Debug.Log(Vector2.zero + " .. " + Camera.main.ScreenToWorldPoint(touch.position).To2D());
            RaycastHit2D hit = Physics2D.GetRayIntersection(touchRay);
            if (hit.collider != null) {
                Debug.LogWarning("Hitted ! : " + hit.collider.name);
                TouchCollider touchModule = hit.collider.GetComponent<TouchCollider>();
                if (touchModule != null) {
                    Touch(touchModule.Touch, touch);
                }
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

    void Touch(Touches key, Touch touch) {
        switch (key) {
            case Touches.DOWN_LEFT:
                _inputPressed[0] = touch.phase != TouchPhase.Ended;
                break;
            case Touches.UP_LEFT:
                if (SceneManager.GetActiveScene().buildIndex != 0) { return; }
                _sceneIndex = 1;
                _onChangeGame?.Invoke(_sceneIndex);
                break;
            case Touches.DOWN_RIGHT:
                _inputPressed[1] = touch.phase != TouchPhase.Ended;
                break;
            case Touches.UP_RIGHT:
                if (SceneManager.GetActiveScene().buildIndex != 0) { return; }
                _sceneIndex = 2;
                _onChangeGame?.Invoke(_sceneIndex);
                break;
        }

        if (SceneManager.GetActiveScene().buildIndex == 0) { // ON the game
            if (_inputPressed[0] && _inputPressed[1] && touch.phase == TouchPhase.Began && (key == Touches.DOWN_LEFT || key == Touches.DOWN_RIGHT)) {
                ChangeScene(_sceneIndex);
            }
        } else { // OFF the game
            if (_inputPressed[0] && _inputPressed[1] && touch.phase == TouchPhase.Began && (key == Touches.DOWN_LEFT || key == Touches.DOWN_RIGHT)) {
                _routine_InputPressed = StartCoroutine(ChangeSceneWithTime(0));
            }

            if (touch.phase == TouchPhase.Ended && (key == Touches.DOWN_LEFT || key == Touches.DOWN_RIGHT)) {
                if (_routine_InputPressed != null) StopCoroutine(_routine_InputPressed);
            }
        }
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
