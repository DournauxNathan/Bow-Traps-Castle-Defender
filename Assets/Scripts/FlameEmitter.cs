using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FlameEmitter : Trap
{
    // Additional properties specific to FireWall
    public int fireDamage = 20; // Example: Fire damage specific to FireWall

    public List<Emitter> emitters;

    private void Start()
    {
        foreach (Emitter emitter in emitters)
        {
            emitter.damage = fireDamage;
        }
    }

    // Override the Activate method to implement FireWall-specific activation
    public override void Activate()
    {
        base.Activate();

        if (CanActivate())
        {
            // Activate the FireWall effect
            StartCoroutine(ActivateTrapEffect());

            // Set a cooldown or duration for the FireWall effect
            StartCoroutine(DeactivateTrapEffectAfterDelay());
        }
        /*else
        {
            // Handle broken state specific to FireWall
            Debug.LogWarning("Cannot activate a broken FireWall!");
        }*/
    }

    // Override the ActivateTrapEffect method to implement FireWall-specific effect
    protected override IEnumerator ActivateTrapEffect()
    {
        // Placeholder logic for common trap effect
        yield return base.ActivateTrapEffect();
                
        // Add your FireWall effect logic here
        foreach (Emitter emitter in emitters)
        {
            switch (enableMode)
            {
                case Mode.Toogle:
                    emitter.Toogle();
                    break;
                case Mode.Activate:
                    emitter.Activate();
                    break;
                default:
                    break;
            }

            yield return new WaitForSeconds(0.8f);
        }
    }

    protected IEnumerator DeactivateTrapEffectAfterDelay()
    {
        // Placeholder logic for common trap effect
        yield return base.DeactivateTrapEffectAfterDelay(trapEffectDuration);

        // Add your FireWall effect logic here
        foreach (Emitter emitter in emitters)
        {
            emitter.Deactivate();
        }
    }
}
