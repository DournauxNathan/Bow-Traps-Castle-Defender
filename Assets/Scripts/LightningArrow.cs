using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningArrow : Arrow
{
    public float raycastDistance = 10f;
    public int maxChainCount = 3;

    protected override void DealDamage(Collision collision)
    {
        base.DealDamage(collision);

        FireLightningArrow();
    }

    private void FireLightningArrow()
    {
        // Raycast forward to hit other critters
        RaycastHit[] hits = RaycastForCritters(transform.position, -transform.forward);

        // Apply damage to the first critter hit by the ray
        if (hits.Length > 0 && hits[0].collider.CompareTag("Critter"))
        {
            if (hits[0].collider.TryGetComponent<Critter>(out Critter firstCritter))
            {
                Debug.DrawRay(hits[0].transform.position, -hits[0].transform.forward * raycastDistance, Color.green, 55f);
                
                // Apply initial damage to the first critter
                firstCritter.TakeDamage(10);
            }

            // Propagate the lightning effect to subsequent critters
            int chainCount = Mathf.Min(maxChainCount, hits.Length - 1);
            for (int i = 1; i <= chainCount; i++)
            {
                if (hits[i].collider.CompareTag("Critter"))
                {
                    if (hits[i].collider.TryGetComponent<Critter>(out Critter hitCritter))
                    {
                        // Apply damage to subsequent critters
                        hitCritter.TakeDamage(5); // Adjust the damage as needed
                    }
                }
            }
        }

        // Draw the rays for debugging
        foreach (RaycastHit hit in hits)
        {
            Debug.DrawRay(hit.transform.position, -hit.transform.forward * raycastDistance, Color.black, 55f);
        }

    }

    private RaycastHit[] RaycastForCritters(Vector3 origin, Vector3 direction)
    {
        // Perform raycast and return hits
        return Physics.RaycastAll(origin, direction, raycastDistance);
    }
}
