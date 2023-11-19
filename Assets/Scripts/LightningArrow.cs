using UnityEngine;

public class LightningArrow : Arrow
{
    [SerializeField] private float maxDistance;

    protected override void DealDamage(Collision collision)
    {
        base.DealDamage(collision);

        // Lightning arrow-specific logic
        if (collision.collider.CompareTag("Critter"))
        {
            if (collision.collider.TryGetComponent<Critter>(out Critter critter))
            {
                // Apply additional powerful damage
                critter.health -= 10;

                // Raycast forward to hit other critters
                RaycastHit[] hits = RaycastForCritters(transform.position, transform.forward);

                // Apply damage to all critters hit by the ray
                foreach (RaycastHit hit in hits)
                {
                    if (hit.collider.CompareTag("Critter"))
                    {
                        if (hit.collider.TryGetComponent<Critter>(out Critter hitCritter))
                        {
                            // Apply additional damage to other critters
                            hitCritter.health -= 10;
                        }
                    }
                }
            }
        }
    }

    private RaycastHit[] RaycastForCritters(Vector3 origin, Vector3 direction)
    {
        Ray ray = new Ray(origin, direction);
        RaycastHit[] hits = Physics.RaycastAll(ray, maxDistance); // You can adjust maxDistance as needed
        return hits;
    }
}
