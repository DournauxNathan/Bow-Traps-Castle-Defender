using System.Collections;
using UnityEngine;

public class FireArrow : Arrow
{
    protected override void DealDamage(Collision collision)
    {
        base.DealDamage(collision);

        // Fire arrow-specific logic
        if (collision.collider.CompareTag("Critter"))
        {
            if (collision.collider.TryGetComponent<Critter>(out Critter critter))
            {
                // Apply additional damage over time
                StartCoroutine(ApplyDamageOverTime(critter));
            }
        }
    }
}
