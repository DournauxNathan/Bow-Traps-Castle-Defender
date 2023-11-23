using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Transform goal;
    public Gate gate;

    private void Awake()
    {
        Instance = this;
    }

}
