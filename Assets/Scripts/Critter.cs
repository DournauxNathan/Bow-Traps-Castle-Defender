// Critter.cs

using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.AI;

public class Critter : MonoBehaviour
{
    public enum CritterType
    {
        Weakling,
        Middling,
        Boss
    }

    public CritterType type; // Type of the critter
    public float health = 1; // Initial health
    public float maxHealth = 1; // Maximum health
    public float speed = 5f; // Critter movement speed

    public event Action OnDestinationReached;
    
    private NavMeshAgent m_NavMeshAgent;
    private Rigidbody m_Rigidbody;

    private bool isMoving = true; 
    private bool isEffectOn = false;
    
    public void Init(CritterData data)
    {
        maxHealth = data.health;
        this.health = maxHealth;
        this.speed = data.speed;
    }

    void Start()
    {
        m_NavMeshAgent = GetComponent<NavMeshAgent>();
        m_Rigidbody = GetComponent<Rigidbody>();

        if (m_NavMeshAgent != null && m_NavMeshAgent.isActiveAndEnabled)
        {
            // Set initial destination on start
            SetDestination(GameManager.Instance.goal.position);
        }
    }

    void FixedUpdate()
    {
        // Check if the critter is defeated
        if (health <= 0 || transform.position.y >= 30f)
        {
            Defeat();
        }
    }

    public void SetDestination(Vector3 position)
    {
        m_NavMeshAgent.SetDestination(position);
    }

    public void StopMovement()
    {
        // Stop the movement of the critter
        isMoving = false;
        m_NavMeshAgent.isStopped = true;
    }

    public void StartMovement()
    {
        // Stop the movement of the critter
        isMoving = true;
        m_NavMeshAgent.isStopped = false;
    }

    public void InverseGravity()
    {
        // Disable the rigidbody's useGravity while gravity is inverted
        m_Rigidbody.useGravity = false;
        m_Rigidbody.isKinematic = true;

        m_Rigidbody.AddForce(Vector3.up * 5f);
    }

    public void StartEffect(float force, float effectDuration, System.Action<float, Critter> effectAction)
    {
        if (!isEffectOn)
        {
            isEffectOn = true;

            // Start the effect
            StartCoroutine(ApplyEffectOverTime(effectDuration, effectAction));
        }
    }

    private IEnumerator ApplyEffectOverTime(float effectDuration, System.Action<float, Critter> effectAction)
    {
        float timer = 0f;

        while (timer < effectDuration)
        {
            // Apply the effect
            effectAction?.Invoke(Time.deltaTime, this);
            timer += Time.deltaTime;
            yield return null;
        }

        // Stop the effect after the specified duration
        StopEffect();
    }

    private void StopEffect()
    {
        isEffectOn = false;
        // Implement any logic needed when the effect stops
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
    }

    void Defeat()
    {
        // Handle defeat based on critter type
        switch (type)
        {
            case CritterType.Weakling:
                // Handle Weakling defeat
                break;
            case CritterType.Middling:
                // Handle Middling defeat
                break;
        }

        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Goal"))
        {
            OnDestinationReached?.Invoke();
            StopMovement();
        }
    }
}
