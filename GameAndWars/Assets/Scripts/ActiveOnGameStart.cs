using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveOnGameStart : MonoBehaviour {
    [SerializeField] SpriteLed _spriteLed;

    public void GameStart() {
        _spriteLed.Active = true;
    }
}
