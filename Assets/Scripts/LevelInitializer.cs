using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInitializer : MonoBehaviour
{
    public Transform goal;
    public Gate gate;

    void Awake()
    {
        GameManager.Instance.InitilazeLevel(goal, gate);
    }
}
