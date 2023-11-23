// InverseGravity.cs
using UnityEngine;

public class GravityArrow : Arrow
{
    public float inverseGravityForce = 10f;

    public LayerMask critterLayer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Critter"))
        {
            if (other.TryGetComponent<Critter>(out Critter critter))
            {
                critter.StopMovement();
                critter.InverseGravity();
            }
        }
    }
}
