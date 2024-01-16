using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Credits : MonoBehaviour
{
    public Button restartButton;

    public void Awake()
    {
        restartButton.onClick.AddListener(() => LevelManager.Instance?.LoadSceneAsync("Tutorial_1"));
    }
}
