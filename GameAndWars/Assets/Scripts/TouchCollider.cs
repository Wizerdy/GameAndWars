using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Touches {
    DOWN_LEFT,
    UP_LEFT,
    DOWN_RIGHT,
    UP_RIGHT,
}

public class TouchCollider : MonoBehaviour {
    [SerializeField] Touches _touch;

    public Touches Touch => _touch;
}
