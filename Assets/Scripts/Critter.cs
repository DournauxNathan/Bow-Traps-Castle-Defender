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
    public int health = 1; // Initial health
    public int maxHealth = 1; // Maximum health
    public float speed = 5f; // Critter movement speed

    public event Action OnDestinationReached;
    
    private NavMeshAgent m_NavMeshAgent;

    private bool isMoving = true; // Flag to control movement

    public void Init(CritterData data)
    {
        this.health = data.health;
        this.speed = data.speed;
    }

    void Start()
    {
        m_NavMeshAgent = GetComponent<NavMeshAgent>();
        if (m_NavMeshAgent == null)
        {
            Debug.LogError("NavMeshAgent component not found on Critter GameObject.");
        }
        else
        {
            // Set initial destination on start
            SetDestination(GameManager.Instance.goal.position);
        }
    }

    void Update()
    {
        // Check if the critter is defeated
        if (health <= 0)
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
            case CritterType.Boss:
                // Handle Boss defeat
                BossDefeat();
                break;
        }

        Destroy(gameObject); // Destroy the critter
    }


    void BossDefeat()
    {
        // Handle Boss defeat logic, e.g., transition to the second phase
    }

    // Called when the critter is hit by an arrow
    public void TakeDamage(int damage)
    {
        health -= damage;
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
