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
        float originalSpeed = critter.speed;
        critter.speed = 0f; // Stop critter movement

        yield return new WaitForSeconds(duration);

        critter.speed = originalSpeed; // Restore critter movement
    }
}
