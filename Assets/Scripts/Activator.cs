using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Activator : MonoBehaviour, IActivatable
{
    public bool IsBroken { get; private set; }
    public bool Repairable { get; protected set; }
    public float RepairTime { get; protected set; }
    public float _repairTime;

    public float breakChance = 0.35f; // Chance of the trap breaking (e.g., 10%)
    
    public BoxCollider m_Collider;
    public ParticleSystem m_particles;
    public AudioSource m_audioSource;

    public UnityEvent onBreak, onRepair;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize properties and setup
        IsBroken = false;
        Repairable = true; 
        RepairTime = 3f; // Example: Time it takes to repair the trap
    }

    public void GetRandomValue()
    {
        float randomProbability = Random.value;

        if (randomProbability < breakChance)
        {
            Break();
        }
    }

    public void Break()
    {
        IsBroken = true;

        // Additional logic for when the trap is broken
        onBreak?.Invoke();

        m_Collider.enabled = true;
        m_particles?.Play();
        m_audioSource?.Play();
    }

    public void Repair(float amount)
    {
        if (IsBroken)
        {
            RepairTime -= amount;  // Decrease repair progress

            _repairTime = RepairTime;

            if (RepairTime <= 0f)
            {
                IsBroken = false;
                RepairTime = 3f; // Reset repair time for the next repair
                _repairTime = RepairTime;

                onRepair?.Invoke();

                m_Collider.enabled = false;
                m_particles?.Stop();
                m_audioSource?.Play();
            }
        }

        // If the trap is not broken, you can choose to handle this case separately or ignore it.
        //MAYBE UPDATE ACTIVATOR ? 
    }

}
