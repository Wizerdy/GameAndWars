using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsBoxEngine;
using UnityEngine.SceneManagement;

public class GameManag : MonoBehaviour {
    [SerializeField] PlayerControls _playerControls;
    [SerializeField] List<Grenade> _grenades;
    [SerializeField] List<Enemy> _enemies;
    [SerializeField] SpriteLed _explosion;
    [SerializeField] LifeManager _lifeManager;
    [SerializeField] ScoreManager _scoreManager;
    [SerializeField] MultiSpriteLed _showAllSprites;
    [SerializeField] MultiSpriteLed _disableOnExplode;
    [SerializeField] List<LoopMultiSpriteLed> _loops;
    [SerializeField] List<ActiveOnGameStart> _activeOnGameStart;
    [SerializeField] float _timeShowAll = 1f;
    [SerializeField] float _grenadeTimer = 5f;

    [Header("Difficulty")]
    [SerializeField] Vector2 _grenadesBounds = Vector2.one;
    [SerializeField] Vector2 _lostWinTime = Vector2.one;

    Coroutine _routine_LaunchGrenade;

    List<Grenade> _launching = new List<Grenade>();

    void Start() {
        if (_lifeManager != null) { _lifeManager.OnLose += _OnLoseGame; }
        for (int i = 0; i < _grenades.Count; i++) {
            if (_grenades[i] != null) {
                _grenades[i].OnStep += _Reflect;
                _grenades[i].OnExplode += _Explosion;
                _grenades[i].OnExplodeBack += _ExplosionBack;
            }
        }
        _playerControls.CanMove = false;
        _showAllSprites.ShowAll(true);
        _scoreManager.Point("000");
        StartCoroutine(Tools.Delay(() => _showAllSprites.ShowAll(false), _timeShowAll));
        StartCoroutine(Tools.Delay(() => _scoreManager.Point(""), _timeShowAll));
        StartCoroutine(Tools.Delay(() => StartGame(), _timeShowAll + _timeShowAll / 2f));
    }

    private void StartGame() {
        _playerControls.StartGame();
        _scoreManager.StartGame();
        _enemies.ForEach((Enemy e) => e?.StartGame());
        _loops.ForEach((LoopMultiSpriteLed l) => l?.StartGame());
        _activeOnGameStart.ForEach((ActiveOnGameStart l) => l?.GameStart());
        _lifeManager.ShowHealth();
        _routine_LaunchGrenade = StartCoroutine(LaunchTimer());
    }

    IEnumerator LaunchTimer() {
        while (true) {
            Grenade nade = _grenades.Random();
            LaunchGrenade(nade);
            yield return new WaitForSeconds(_grenadeTimer);
        }
    }

    void LaunchGrenade(Grenade nade) {
        if (nade == null) { return; }
        if (_launching.Contains(nade)) { return; }
        if (nade.Launched) { return; }

        _launching.Add(nade);
        AudioManager.instance.Play("American");
        _enemies[nade.Position.x]?.Warn(nade.Position.y);
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

            _grenadeTimer -= _lostWinTime.x;
            _grenadeTimer = Mathf.Max(_grenadesBounds.x, _grenadeTimer);
        }
    }

    void _Explosion() {
        _explosion.ActiveFor(1f);
        _disableOnExplode?.ShowAll(false);
        StartCoroutine(Tools.Delay(() => _disableOnExplode?.ShowAll(true), 1f));
        _grenadeTimer += _lostWinTime.y;
        _grenadeTimer = Mathf.Max(_grenadeTimer, _grenadesBounds.y);
        AudioManager.instance.Play("Explosion");
        LoseLife();
    }

    void _ExplosionBack(Grenade nade) {
        _enemies[nade.Position.x]?.Explode(nade.Position.y);
        AudioManager.instance.Play("ExplosionBack");
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
