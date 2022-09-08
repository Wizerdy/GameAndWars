using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsBoxEngine;

public class Enemy : MonoBehaviour {
    [SerializeField] SpriteLed _body;
    [SerializeField] List<SpriteLed> _exclamations;
    [SerializeField] List<SpriteLed> _arms;
    [SerializeField] List<SpriteLed> _explosion;

    [SerializeField] float _warningTime = 1f;

    public void StartGame() {
        _body.Active = true;
    }

    public void Warn(int index) {
        _exclamations[index].ActiveFor(_warningTime);
        _arms[index].ActiveFor(_warningTime);
        //StartCoroutine(Tools.Delay(() => _arms[index].ActiveFor(_warningTime/2f), _warningTime /2f));
    }

    public void Explode(int index) {
        _explosion[index].ActiveFor(1f);
    }
}
