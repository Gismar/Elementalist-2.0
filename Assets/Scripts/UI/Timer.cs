using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Text _timerText;

    void Update()
    {
        _timerText.text = $"{Mathf.FloorToInt(Time.timeSinceLevelLoad / 60f)}m {Mathf.FloorToInt(Time.timeSinceLevelLoad % 60)}s";
    }
}
