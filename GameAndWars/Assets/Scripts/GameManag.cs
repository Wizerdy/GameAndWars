using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsBoxEngine;

public class GameManag : MonoBehaviour {
    [SerializeField] PlayerControls _playerControls;
    [SerializeField] List<Grenade> _grenades;
    [SerializeField] SpriteLed _explosion;
    [SerializeField] LifeManager _lifeManager;
    [SerializeField] int _life = 3;
    [SerializeField] float _grenade_timer = 5f;

    void Start() {
        for (int i = 0; i < _grenades.Count; i++) {
            if (_grenades[i] != null) {
                _grenades[i].OnStep += _Reflect;
                _grenades[i].OnExplode += _Explosion;
            }
        }
        StartCoroutine(LaunchTimer());
    }

    IEnumerator LaunchTimer() {
        while (true) {
            Grenade nade = _grenades.Random();
            if (nade != null) { Debug.Log(nade.name); }
            nade?.Launch();
            yield return new WaitForSeconds(_grenade_timer);
        }
    }

    void _Reflect(Grenade grenade) {
        if (!grenade.LastStep) { return; }
        int index = _grenades.IndexOf(grenade);
        Vector2Int position = IntToVector(index);
        if (_playerControls.Position == position) {
            grenade.Reflect();
        }
    }

    void _Explosion() {
        _explosion.ActiveFor(1f);
        LoseLife();
    }

    void LoseLife() {
        --_life;
        _lifeManager?.LoseLife();
    }

    int VectorToInt(Vector2Int position) {
        int output = 0;
        output += position.x;
        output += position.y * 2;
        return output;
    }

    Vector2Int IntToVector(int position) {
        Vector2Int output = Vector2Int.zero;
        output.x = position % 2;
        output.y = position > 1 ? 1 : 0;
        return output;
    }
}
