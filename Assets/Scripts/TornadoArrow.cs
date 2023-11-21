using UnityEngine;

public class TornadoArrow : Arrow
{
    public float tornadoRadius = 5f;
    public float tornadoForce = 10f;
    public float effectDuration = 5f;

    public LayerMask critterLayer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Critter"))
        {
            CreateTornado(other.transform.position);
            Destroy(gameObject);
        }
    }

    private void CreateTornado(Vector3 center)
    {
        Collider[] colliders = Physics.OverlapSphere(center, tornadoRadius, critterLayer);

        foreach (Collider collider in colliders)
        {
            Critter critter = collider.GetComponent<Critter>();
            if (critter != null)
            {
                // Pass the tornado effect parameters
                //critter.StartEffect(effectDuration, ApplyTornadoEffect);
            }
        }
    }

    private void ApplyTornadoEffect(float deltaTime, Critter critter)
    {
        critter.GetComponent<Rigidbody>().AddForce(Vector3.up * tornadoForce, ForceMode.Impulse);
    }
}