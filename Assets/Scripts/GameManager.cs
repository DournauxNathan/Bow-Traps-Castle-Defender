using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

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
        Instance = this;
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

    public void setRig(Transform transform)
    {
        XRRig = transform;
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
