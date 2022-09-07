using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GyroscopeInput : MonoBehaviour {
    [SerializeField] UnityEvent<Quaternion> _onRotation = new UnityEvent<Quaternion>();

    Gyroscope _gyroscope = null;
    Quaternion? _lastAttitude = null;

    #region Properties

    public Quaternion Rotation => GetRotation();
    public event UnityAction<Quaternion> OnRotation { add => _onRotation.AddListener(value); remove => _onRotation.RemoveListener(value); }

    #endregion

    void Awake() {
        if (SystemInfo.supportsGyroscope) {
            _gyroscope = Input.gyro;
            _gyroscope.enabled = true;
        } else {
            Debug.LogWarning("Gyroscope not supported");
        }
    }

    void Update() {
        if (_gyroscope == null) { return; }

        if (_lastAttitude == null || _lastAttitude.Value != _gyroscope.attitude) {
            _onRotation?.Invoke(Rotation);
            _lastAttitude = _gyroscope.attitude;
        }
    }

    Quaternion GetRotation() {
        return _gyroscope?.attitude ?? Quaternion.identity;
    }
}
