using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed = 10f;
    public Transform tip;

    private Rigidbody m_Rigidbody;
    [SerializeField] private ParticleSystem m_ParticleSystem;
    [SerializeField] private TrailRenderer m_TrailRenderer;

    private bool isInAir = false;
    private Vector3 lastPos = Vector3.zero;


    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();

        PullInteraction.PullActionReleased += Release;

        Stop();
    }

    private void OnDestroy()
    {
        PullInteraction.PullActionReleased -= Release;
    }

    private void Release(float value)
    {
        PullInteraction.PullActionReleased -= Release;
        gameObject.transform.parent = null;
        isInAir = true;
        SetPhysics(true);

        Vector3 force = transform.forward * value * speed;
        m_Rigidbody.AddForce(force, ForceMode.Impulse);

        StartCoroutine(RotateWithVelocity());

        lastPos = tip.position;

        m_ParticleSystem.Play();
        m_TrailRenderer.emitting = true;
    }

    private IEnumerator RotateWithVelocity()
    {
        yield return new WaitForFixedUpdate();
        while (isInAir)
        {
            Quaternion newRotation = Quaternion.LookRotation(m_Rigidbody.velocity, transform.up);
            transform.rotation = newRotation;
            yield return null;
        }
    }
        
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Critter"))
        {
            if (collision.collider.TryGetComponent<Critter>(out Critter critter))
            {
                Debug.Log(critter);
                critter.health -= 5;
            }

            if (collision.collider.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                //rb.interpolation = RigidbodyInterpolation.None;
                transform.parent = collision.transform;
                //rb.AddForce(m_Rigidbody.velocity, ForceMode.Impulse);
            }
           
            Stop();
        }
    }

    private void Stop()
    {
        isInAir = false;
        SetPhysics(false);

        m_ParticleSystem.Stop();
        m_TrailRenderer.emitting = false;
    }

    private void SetPhysics(bool usePhysics)
    {
        m_Rigidbody.useGravity = usePhysics;
        m_Rigidbody.isKinematic = !usePhysics;
    }
}
