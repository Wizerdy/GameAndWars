using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopMultiSpriteLed : MonoBehaviour {
    [SerializeField] MultiSpriteLed _spriteLed;
    [SerializeField] float _timer = 1f;

    public void StartGame() {
        StartCoroutine(Loop());
    }

    IEnumerator Loop() {
        while (true) {
            yield return new WaitForSeconds(1f);
            _spriteLed.Next();
        }
    }
}
