using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AutoLoad : MonoBehaviour
{
    private void Awake()
    {
        SceneManager.LoadSceneAsync("Main Menu");
    }
}
