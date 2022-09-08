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
    [SerializeField] MultiSpriteLed _showAllSprites;
    [SerializeField] float _timeShowAll = 1f;
    [SerializeField] float _grenade_timer = 5f;

    Coroutine _routine_LaunchGrenade;

    List<Grenade> _launching = new List<Grenade>();

    void Start() {
        if (_lifeManager != null) { _lifeManager.OnLose += _OnLoseGame; }
        for (int i = 0; i < _grenades.Count; i++) {
            if (_grenades[i] != null) {
                _grenades[i].OnStep += _Reflect;
                _grenades[i].OnExplode += _Explosion;
            }
        }
        _playerControls.CanMove = false;
        _showAllSprites.ShowAll(true);
        _scoreManager.Point("00000");
        StartCoroutine(Tools.Delay(() => _showAllSprites.ShowAll(false), _timeShowAll));
        StartCoroutine(Tools.Delay(() => _scoreManager.Point(""), _timeShowAll));
        StartCoroutine(Tools.Delay(() => StartGame(), _timeShowAll + _timeShowAll / 2f));
    }

    private void StartGame() {
        _playerControls.StartGame();
        _scoreManager.StartGame();
        _lifeManager.ShowHealth();
        _routine_LaunchGrenade = StartCoroutine(LaunchTimer());
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
        if (nade.Launched) { return; }

        _launching.Add(nade);
        AudioManager.instance.Play("American");
        StartCoroutine(Tools.Delay(() => { nade?.Launch(); _launching.Remove(nade); AudioManager.instance.Play("Send_Grenade"); }, 1f));
    }

    void _Reflect(Grenade grenade) {
        if (!grenade.LastStep) { return; }
        int index = _grenades.IndexOf(grenade);
        Vector2Int position = IntToVector(index);
        if (_playerControls.Position == position) {
            grenade.Reflect();
            _playerControls.Anim();
            _scoreManager.Point(1);
            AudioManager.instance.Play("Reflect_Grenade");
            if (_grenade_timer > 0.9f) {
                _grenade_timer -= 0.1f;
            }
        }
    }

    void _Explosion() {
        _explosion.ActiveFor(1f);
        _grenade_timer += 1f;
        AudioManager.instance.Play("Explosion");
        LoseLife();
    }

    void LoseLife() {
        for (int i = 0; i < _grenades.Count; i++) { _grenades[i].ResetMe(); }
        _lifeManager?.LoseLife();
    }

    void _OnLoseGame() {
        if (_routine_LaunchGrenade != null) { StopCoroutine(_routine_LaunchGrenade); }
        _playerControls.CanMove = false;
        StartCoroutine(Tools.Delay(() => SceneManager.LoadScene(0), 2f));
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
