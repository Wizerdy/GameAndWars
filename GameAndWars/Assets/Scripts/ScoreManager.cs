using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour {
    [SerializeField] TextMeshProUGUI _text;

    int _points = 0;

    void Start() {
        Point(0);
    }

    public void Point(int point) {
        _points += point;
        string points = _points.ToString();
        while (points.Length < 5) {
            points = " " + points;
        }
        _text.text = points;
    }
}
