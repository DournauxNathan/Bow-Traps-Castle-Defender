using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelInitializer : MonoBehaviour
{
    public Transform goal;
    public Gate gate;

    public UnityEvent onStart;

    void Awake()
    {
        XRSettingsManager.Instance?.enableBodyInventory(true);
        GameManager.Instance?.InitilazeLevel(goal, gate);

        onStart?.Invoke();
    }
}
