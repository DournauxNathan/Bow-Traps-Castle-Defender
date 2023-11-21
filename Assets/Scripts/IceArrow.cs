using System.Collections;
using UnityEngine;

public class IceArrow : Arrow
{
    protected override void DealDamage(Collision collision)
    {
        // Ice arrow-specific logic
        if (collision.collider.CompareTag("Critter"))
        {
            if (collision.collider.TryGetComponent<Critter>(out Critter critter))
            {

                StartCoroutine(FreezeCritter(critter));
            }
        }
        base.DealDamage(collision);
    }

    private IEnumerator FreezeCritter(Critter critter)
    {
        while (effectDuration > 0)
        {
            critter.StopMovement();
            // Apply the effect
            effectDuration -= Time.deltaTime;
            yield return null;
        }

        critter.StartMovement();
    }
}
