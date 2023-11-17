using System;
using System.Collections;
using UnityEngine;

public class Trap : MonoBehaviour
{    
    public enum Mode
    {
        Toogle,
        Activate
    }

    public Mode enableMode;
    public bool IsActive { get; private set; }

    public Activator activator;

    // Additional properties specific to Trap
    public float trapEffectDuration = 5f; // Example: How long the trap effect lasts

    void Start()
    {
        // Initialize properties and setup
        IsActive = false;
    }

    public bool CanActivate()
    {
        return !IsActive && !activator.IsBroken;
    }

    public virtual void Activate()
    {
        

        /*if (CanActivate())
        {
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
        }*/
    }

    protected virtual IEnumerator ActivateTrapEffect()
    {
        IsActive = true;
        activator.GetRandomValue();

        yield return null; 
    }

    protected virtual IEnumerator DeactivateTrapEffectAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        IsActive = false;
    }
        
    // Draw a line between the trap and its activator in the Scene view
    private void OnDrawGizmos()
    {
        if (activator != null)
        {
            // Draw the first line segment
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, activator.transform.position);
        }
    }
}
