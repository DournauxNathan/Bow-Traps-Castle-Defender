// Critter.cs

using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.AI;
public enum CritterType
{
    Weakling,
    Middling,
    Boss
}
public class Critter : MonoBehaviour
{
    [Header("PROPERTIES")]
    public CritterType type; // Type of the critter
    public float health = 1; // Initial health
    public float maxHealth = 1; // Maximum health
    public int currencyValue;
    public Animator m_Animator;

    [Header("MOVEMENT SETTINGS")]
    public float speed = 5f; // Critter movement speed
    public event Action OnDestinationReached, OnKilled;

    [Header("VFX")]
    public ParticleSystem onFireEffect;

    [Header("DEBUG")]
    public bool goBackAndForth = false;
    public bool isKilled = false;

    private bool isEffectOn = false;
    private Vector3 startPosition;
    private Vector3 goalPosition;
    private NavMeshAgent m_NavMeshAgent;
    private Rigidbody m_Rigidbody;
    
    public void Init(CritterData data)
    {
        maxHealth = data.health;
        this.health = maxHealth;
        this.speed = data.speed;
        this.currencyValue = data.currencyValue;
    }

    void Start()
    {
        startPosition = transform.position;
        goalPosition = GameManager.Instance.goal.position;

        m_NavMeshAgent = GetComponent<NavMeshAgent>();
        m_NavMeshAgent.speed = speed;

        m_Rigidbody = GetComponent<Rigidbody>();
        onFireEffect.Stop();

        if (m_NavMeshAgent != null && m_NavMeshAgent.isActiveAndEnabled)
        {
            // Set initial destination on start
            SetDestination(goalPosition);
        }
    }

    void FixedUpdate()
    {
        // Check if the critter is defeated
        if (health <= 0 || transform.position.y >= 30f || isKilled)
        {
            Defeat();
        }

        if (m_NavMeshAgent.isActiveAndEnabled && m_NavMeshAgent != null && m_NavMeshAgent.remainingDistance < 1f)
        {
            SetDestination(GameManager.Instance.goal.position);
        }
    }

    public void SetDestination(Vector3 position)
    {
        m_NavMeshAgent.SetDestination(position);

        switch (type)
        {
            case CritterType.Weakling:
                m_Animator.SetBool("Run Forward", true);
                break;
            case CritterType.Middling:
                m_Animator.SetBool("Walk Forward", true);
                break;
        }
    }

    public void StopMovement()
    {
        m_NavMeshAgent.isStopped = true;
    }

    public void StartMovement()
    {
        m_NavMeshAgent.isStopped = false;

        m_NavMeshAgent.SetDestination(goalPosition);
    }

    public void InverseGravity()
    {
        // Disable the rigidbody's useGravity while gravity is inverted
        m_Rigidbody.useGravity = false;
        m_Rigidbody.isKinematic = true;

        m_Rigidbody.AddForce(Vector3.up * 5f);
    }

    public void StartEffect(float damage, float effectDuration, ParticleSystem effectVFX)
    {
        if (!isEffectOn)
        {
            isEffectOn = true;

            effectVFX.Play();

            StartCoroutine(ApplyEffectOverTime(damage, effectDuration, effectVFX));
        }
    }

    private IEnumerator ApplyEffectOverTime(float damage, float effectDuration, ParticleSystem effectVFX)
    {
        while (effectDuration > 0)
        {
            // Apply the effect
            TakeDamage(damage);
            effectDuration -= Time.deltaTime;
            yield return null;
        }

        StopEffect(effectVFX);
    }

    private void StopEffect(ParticleSystem effectVFX)
    {
        isEffectOn = false;

        effectVFX.Stop();
    }

    public void ToggleEffect(ParticleSystem currentEffect, bool playEffect)
    {
        if (playEffect)
        {
            currentEffect.Play();
        }
        else
        {
            currentEffect.Stop();
        }
    }

    public void TakeDamage(float damage)
    {
        m_Animator.SetTrigger("Take Damage");
        health -= damage;
    }

    void Defeat()
    {
        isKilled = true;

        StopMovement();

        // Handle defeat based on critter type
        switch (type)
        {
            case CritterType.Weakling:
                m_Animator.SetBool("Run Forward", false);
                // Handle Weakling defeat
                break;
            case CritterType.Middling:
                m_Animator.SetBool("Walk Forward", false);
                // Handle Middling defeat
                break;
        }

        m_Animator.SetTrigger("Die");

        
        if (isKilled)
        {
            GameManager.Instance.AddCurency(currencyValue);
            isKilled = false;
            OnKilled?.Invoke();
            
            GetComponent<BoxCollider>().enabled = false;
            UsePhysics(false);
            Destroy(this.gameObject, 3f);
        }


    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Goal"))
        {
            if (!goBackAndForth)
            {
                OnDestinationReached?.Invoke();
                StopMovement();
            }
            else
            {
                SetDestination(startPosition);
            }
        }
    }

    public void UsePhysics(bool usePhysics)
    {
        m_Rigidbody.useGravity = usePhysics;
        m_Rigidbody.isKinematic= !usePhysics;
    }
}
