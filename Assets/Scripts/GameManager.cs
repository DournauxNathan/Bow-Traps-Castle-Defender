using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Transform goal { get; private set; }
    public Gate gate { get; private set; }
    public Pouch pouch { get; private set; }
    public Transform XRRig { get; private set; }

    private List<BreakableActivator> activators = new List<BreakableActivator>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void InitilazeLevel(Transform goal, Gate gate)
    {
        this.goal = goal;
        this.gate = gate;
        pouch.currentCurrency = 0;
    }

    public void GetPouchInfo(Pouch pouch)
    {
        this.pouch = pouch;
    }

    public void SubscribeActivators(BreakableActivator activator)
    {
        activators.Add(activator);
    }

    // Accessor method for getting the list
    public List<BreakableActivator> GetActivators()
    {
        return activators;
    }
}
