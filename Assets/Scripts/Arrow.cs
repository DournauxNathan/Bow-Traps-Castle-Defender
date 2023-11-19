using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    // Common properties and methods for all arrows
    public int damage = 5; // Base damage of the arrow
    public float damageOverTimeDuration = 3f; // Duration of damage over time effect

    public float speed = 10f;
    public Transform tip;

    [SerializeField] protected Rigidbody m_Rigidbody;
    [SerializeField] protected ParticleSystem m_ParticleSystem;
    [SerializeField] protected TrailRenderer m_TrailRenderer;

    [SerializeField] protected AudioSource m_AudioSource;
    [SerializeField] protected AudioClip onShootSound, onHitSound;

    protected bool isInAir = false;
    protected Vector3 lastPos = Vector3.zero;

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

    protected virtual void Release(float value)
    {
        PullInteraction.PullActionReleased -= Release;
        gameObject.transform.parent = null;
        isInAir = true;
        SetPhysics(true);

        Vector3 force = transform.forward * value * speed;
        m_Rigidbody.AddForce(force, ForceMode.Impulse);

        StartCoroutine(RotateWithVelocity());

        lastPos = tip.position;

        m_AudioSource.PlayOneShot(onShootSound);
        m_ParticleSystem.Play();
        m_TrailRenderer.emitting = true;
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        // Common collision logic for all arrows
        Stop();

        // Deal damage to the target
        DealDamage(collision);
    }

    protected virtual void DealDamage(Collision collision)
    {
        if (collision.collider.CompareTag("Critter"))
        {
            if (collision.collider.TryGetComponent<Critter>(out Critter critter))
            {
                // Apply base damage
                critter.health -= damage;
            }
        }
    }

    protected virtual IEnumerator ApplyDamageOverTime(Critter critter)
    {
        float timer = 0f;
        while (timer < damageOverTimeDuration)
        {
            critter.health -= 1; // Example: Apply 1 damage per second
            timer += Time.deltaTime;
            yield return null;
        }
    }

    protected IEnumerator RotateWithVelocity()
    {
        yield return new WaitForFixedUpdate();
        while (isInAir)
        {
            Quaternion newRotation = Quaternion.LookRotation(m_Rigidbody.velocity, transform.up);
            transform.rotation = newRotation;
            yield return null;
        }
    }

    protected void Stop()
    {
        isInAir = false;
        SetPhysics(false);
        m_ParticleSystem.Stop();
        m_TrailRenderer.emitting = false;
    }

    protected void SetPhysics(bool usePhysics)
    {
        m_Rigidbody.useGravity = usePhysics;
        m_Rigidbody.isKinematic = !usePhysics;
    }
}
