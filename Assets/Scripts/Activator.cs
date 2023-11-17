using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activator : MonoBehaviour, IActivatable
{
    public bool IsBroken { get; private set; }
    public bool Repairable { get; protected set; }
    public float RepairTime { get; protected set; }


    public float breakChance = 0.35f; // Chance of the trap breaking (e.g., 10%)

    public ParticleSystem m_particles;
    public AudioSource m_audioSource;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize properties and setup
        IsBroken = false;
        Repairable = true; 
        RepairTime = 3f; // Example: Time it takes to repair the trap
    }

    public bool GetRandomVazlue()
    {
        float value = Random.value;
        Debug.Log(value);
        if (value < breakChance)
        {
            Break();
            return true;
        }

        return false;
    }

    public void Break()
    {
        IsBroken = true;

        // Additional logic for when the trap is broken
        m_particles.Play();
        //m_audioSource.Stop();

        // Additional logic for when the trap is broken
        Debug.Log("Trap is broken!");
    }

    public void Repair()
    {
        IsBroken = false;
        // Additional logic for when the trap is repaired
        Debug.Log("Trap repaired!");       

    }
}
