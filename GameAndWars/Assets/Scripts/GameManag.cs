using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsBoxEngine;
using UnityEngine.SceneManagement;

public class GameManag : MonoBehaviour {
    [SerializeField] PlayerControls _playerControls;
    [SerializeField] List<Grenade> _grenades;
    [SerializeField] SpriteLed _explosion;
    [SerializeField] LifeManager _lifeManager;
    [SerializeField] ScoreManager _scoreManager;
    [SerializeField] float _grenade_timer = 5f;

    List<Grenade> _launching = new List<Grenade>();

    void Start() {
        if (_lifeManager != null) { _lifeManager.OnLose += _OnLoseGame; }
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
            LaunchGrenade(nade);
            yield return new WaitForSeconds(_grenade_timer);
        }
    }

    void LaunchGrenade(Grenade nade) {
        if (nade == null) { return; }
        if (_launching.Contains(nade)) { return; }

        _launching.Add(nade);
        StartCoroutine(Tools.Delay(() => { nade?.Launch(); _launching.Remove(nade); }, 1f));
    }

    void _Reflect(Grenade grenade) {
        if (!grenade.LastStep) { return; }
        int index = _grenades.IndexOf(grenade);
        Vector2Int position = IntToVector(index);
        if (_playerControls.Position == position) {
            grenade.Reflect();
            _playerControls.Anim();
            _scoreManager.Point(1);
        }
    }

    void _Explosion() {
        _explosion.ActiveFor(1f);
        LoseLife();
    }

    void LoseLife() {
        for (int i = 0; i < _grenades.Count; i++) { _grenades[i].ResetMe(); }
        _lifeManager?.LoseLife();
    }

    void _OnLoseGame() {
        SceneManager.LoadScene(0);
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
