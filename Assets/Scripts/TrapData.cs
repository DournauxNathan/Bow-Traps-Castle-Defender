using System;
using System.Collections;
using UnityEngine;

public class TrapData : MonoBehaviour
{
    public enum Mode
    {
        Toogle,
        Activate
    }

    public Mode enableMode;
    internal bool IsActive;

    public float trapEffectDuration = 5f; // Example: How long the trap effect lasts

    void Start()
    {
        // Initialize properties and setup
        IsActive = false;
    }

    public virtual bool CanActivate()
    {
        return !IsActive;
    }

    public virtual void Activate()
    {
        if (CanActivate())
        {
            StartCoroutine(ActivateTrapEffect());

            StartCoroutine(DeactivateTrapEffectAfterDelay(trapEffectDuration));
        }
    }

    protected virtual IEnumerator ActivateTrapEffect()
    {
        IsActive = true;

        yield return null;
    }

    protected virtual IEnumerator DeactivateTrapEffectAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        IsActive = false;
    }
}
