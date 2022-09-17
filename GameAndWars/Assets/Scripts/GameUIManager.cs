using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class GameUIManager : MonoBehaviour {
    [SerializeField] GameObject _gameText;
    [SerializeField] GameObject _aText;
    [SerializeField] GameObject _bText;

    int _index = 0;

    void Start() {
        SwitchScenes._instance.OnChangeGame += _OnChangeGame;
        _OnChangeGame(SwitchScenes._instance.GameIndex);
    }

    private void OnDestroy() {
        SwitchScenes._instance.OnChangeGame -= _OnChangeGame;
    }

    void _OnChangeGame(int index) {
        if (index == _index) { return; }
        AudioManager.instance.Play("Send_Grenade");
        _index = index;
        switch (index) {
            case 1:
                _aText.SetActive(true);
                _bText.SetActive(false);
                break;
            case 2:
                _aText.SetActive(false);
                _bText.SetActive(true);
                break;
            default:
                break;
        }
    }
}
