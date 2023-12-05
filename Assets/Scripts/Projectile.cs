using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<BreakableActivator>(out BreakableActivator activator))
        {
            activator.RandomChanceToBreak();
        }

        // Destroy the projectile on collision
        Destroy(gameObject);
    }

}
