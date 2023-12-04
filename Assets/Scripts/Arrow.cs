using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ArrowType
{
    Base,
    Fire,
    Ice,
    Lightning,
    Explosive,
    Gravity,
    Tornado
}

public class Arrow : MonoBehaviour
{
    [Header("REFERENCES")]
    [SerializeField] protected Transform tip;
    [SerializeField] protected Rigidbody m_Rigidbody;
    [SerializeField] protected ParticleSystem m_ParticleSystem;
    [SerializeField] protected TrailRenderer m_TrailRenderer;
    [SerializeField] protected AudioSource m_AudioSource;
    
    [Header("PROPERTIES")]
    public LayerMask layerMask;
    public float speed = 10f;
    [Tooltip("Base damage of the arrow")] public float damage = 1; 
    [Tooltip("Base damage of the effect")] public float damageOverEffectDuration = 3f;
    [Tooltip("Duration of damage over time effect")] public float effectDuration = 3f;

    [Header("SFX")]
    [SerializeField] protected AudioClip onShootSound;
    [SerializeField] protected AudioClip onHitSound;

    [Header("SFX")]

    protected bool isInAir = false;
    protected Vector3 lastPos = Vector3.zero;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        PullInteraction.PullActionReleased += Release;
        Stop();

        if (m_AudioSource.clip != null)
        {
            m_AudioSource.Play();
        }
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
                critter.TakeDamage(damage);
                
                transform.parent = critter.transform;
                critter.transform.eulerAngles = Vector3.zero;
            }

            if (collision.collider.TryGetComponent<Target>(out Target weakness))
            {
                transform.parent = critter.transform;
            }
        }

        m_AudioSource?.PlayOneShot(onHitSound);

        Invoke("Destroy", 10f);
    }

    public void Destroy()
    {
        Destroy(this.gameObject);
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
