using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody m_rigidBody;

    private void Start()
    {
        m_rigidBody = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<BreakableActivator>(out BreakableActivator activator))
        {
            activator.RandomChanceToBreak();
            // Destroy the projectile on collision
        }

        if (other.TryGetComponent<Weakness>(out Weakness boss))
        {
            boss.OnWeaknessDown();
            other.GetComponent<SimpleFlash>().Flash();

            // Destroy the projectile on collision
            Destroy(gameObject);
        }
    }

    public void TooglePhysics(bool usePhysics)
    {
        m_rigidBody.AddForce(Vector3.up * 10f, ForceMode.Impulse);
        m_rigidBody.useGravity= usePhysics;
        m_rigidBody.isKinematic = !usePhysics;
    }
}
