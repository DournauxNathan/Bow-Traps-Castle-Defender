using System;
using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Trap : XRSimpleInteractable, IActivatable
{
    public bool IsBroken { get; private set; }
    public bool Repairable { get; protected set; }
    public float RepairTime { get; protected set; }

    // Additional properties specific to Trap
    public float activationDelay = 2f; // Example: Activation delay in seconds
    public float trapEffectDuration = 5f; // Example: How long the trap effect lasts

    // Placeholder references for visual/audio effects
    public GameObject activationEffect;
    public AudioSource activationSound;

    void Start()
    {
        // Initialize properties and setup
        IsBroken = false;
        Repairable = true; // Example: Assume traps are repairable by default
        RepairTime = 3f; // Example: Time it takes to repair the trap
    }

    public void Activate()
    {
        if (!IsBroken)
        {
            // Trigger activation effect
            if (activationEffect != null)
            {
                Instantiate(activationEffect, transform.position, Quaternion.identity);
            }

            // Play activation sound
            if (activationSound != null)
            {
                activationSound.Play();
            }

            // Activate the trap effect (example: damage critters, apply status)
            StartCoroutine(ActivateTrapEffect());

            // Optional: Add any other trap-specific activation logic

            // Set a cooldown or duration for the trap effect
            StartCoroutine(DeactivateTrapEffectAfterDelay(trapEffectDuration));
        }
        else
        {
            // Handle broken state
            Debug.LogWarning("Cannot activate a broken trap!");
        }
    }

    IEnumerator ActivateTrapEffect()
    {
        // Placeholder logic for trap effect
        Debug.Log("Trap effect activated!");

        yield return null; // Add your trap effect logic here
    }

    IEnumerator DeactivateTrapEffectAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Deactivate the trap effect (example: stop damaging critters)
        Debug.Log("Trap effect deactivated!");
    }

    public void Break()
    {
        IsBroken = true;
        // Additional logic for when the trap is broken
    }

    public void Repair()
    {
        IsBroken = false;
        // Additional logic for when the trap is repaired
        Debug.Log("Trap repaired!");
    }
}
