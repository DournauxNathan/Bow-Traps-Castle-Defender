using System.Collections;
using UnityEngine;

public class IceArrow : Arrow
{
    public float freezeDuration = 3f;

    protected override void DealDamage(Collision collision)
    {
        base.DealDamage(collision);

        // Ice arrow-specific logic
        if (collision.collider.CompareTag("Critter"))
        {
            if (collision.collider.TryGetComponent<Critter>(out Critter critter))
            {
                // Apply additional freeze effect
                StartCoroutine(FreezeCritter(critter, freezeDuration));
            }
        }
    }

    private IEnumerator FreezeCritter(Critter critter, float duration)
    {
        critter.StopMovement();
        yield return new WaitForSeconds(duration);
        critter.StartMovement();
    }
}
