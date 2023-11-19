// InverseGravity.cs
using UnityEngine;

public class InverseGravity : MonoBehaviour
{
    public float inverseGravityForce = 10f;
    public float effectDuration = 5f;

    public LayerMask critterLayer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Critter"))
        {
            ApplyInverseGravity(other.transform.position);
            Destroy(gameObject);
        }
    }

    private void ApplyInverseGravity(Vector3 center)
    {
        Collider[] colliders = Physics.OverlapSphere(center, inverseGravityForce, critterLayer);

        foreach (Collider collider in colliders)
        {
            Critter critter = collider.GetComponent<Critter>();
            if (critter != null)
            {
                // Pass the inverse gravity effect parameters
                critter.StartEffect(inverseGravityForce, effectDuration, ApplyInverseGravityEffect);
            }
        }
    }

    private void ApplyInverseGravityEffect(float deltaTime, Critter critter)
    {
        critter.InverseGravity();
    }
}
