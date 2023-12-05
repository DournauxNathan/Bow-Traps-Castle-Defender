using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [Header("PROPERTIES")]
    public CritterType type; 
    public int health = 1; 
    public int maxHealth = 1;
    public int phase;

    public bool hasPhaseBegin = false;
    public int damagePerHit;

    public List<Weakness> weaknesses;

    private Vector3 targetPosition;

    public Animator m_Animator;
    internal WaveManager waveManager;

    public GameObject projectilePrefab;
    private GameObject projectile;
    public Transform firePoint; // The position where the projectile will be spawned
    public float projectileSpeed = 10f;
    public float growthSpeed;

    public void Init(BossData data)
    {
        this.type = CritterType.Boss;
        this.maxHealth = data.health;
        this.health = maxHealth;
        this.phase = data.phase;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void CancelCast()
    {
        projectile = null;
        ResetAction();
        projectile.GetComponent<Projectile>().TooglePhysics(true);
    }

    public void ResetAction()
    {
        phase = 0;
        m_Animator.SetTrigger("Reset");
        StopAllCoroutines();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (hasPhaseBegin)
        {
            hasPhaseBegin = false;
            switch (phase)
            {
                case 1:
                    PrepareCast();                    
                    break;
                case 2:
                    StartNewWave();
                    break;
            }
        }
    }

    public bool GetRandomActivatorPosition()
    {
        // Pick a random active activator
        List<BreakableActivator> activators = GameManager.Instance.activators;

        if (activators.Count > 0)
        {
            int maxAttempts = 50; // Set a maximum number of attempts to avoid an infinite loop
            int rand = -1;

            // Try to find an active activator within a limited number of attempts
            for (int i = 0; i < maxAttempts; i++)
            {
                rand = UnityEngine.Random.Range(0, activators.Count);

                if (!activators[rand].IsBroken)
                {
                    // Found an broken activator, break out of the loop
                    targetPosition = activators[rand].transform.position;
                    return true;
                }
            }
        }
        return false;
    }

    void PrepareCast()
    {
        if (GetRandomActivatorPosition())
        {
            projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity, this.transform);
            // Instantiate the growing projectile prefab

            // Calculate the direction towards the target position
            Vector3 direction = (targetPosition - projectile.transform.position).normalized;

            // Set the rotation of the growing projectile to face the target position
            projectile.transform.rotation = Quaternion.LookRotation(direction);

            // Set the initial scale to be very small
            projectile.transform.localScale = Vector3.one * 0.01f;

            m_Animator.SetTrigger("Cast");

            // Start the growth coroutine
            StartCoroutine(CastProjectile(projectile, targetPosition));
        }
        else
        {
            Debug.LogWarning("No activator was found");
        }
    }


    IEnumerator CastProjectile(GameObject projectile, Vector3 position)
    {
        // Continue growing the projectile until it reaches its maximum scale
        while (projectile.transform.localScale.x <= 1.5f)
        {
            float growthAmount = growthSpeed * Time.deltaTime;
            projectile.transform.localScale += Vector3.one * growthAmount;

            yield return null;
        }

        projectile.transform.parent = null;
        m_Animator.SetTrigger("Release");

        while (Vector3.Distance(projectile.transform.position, position) >= .5f)
        {
            projectile.transform.position = Vector3.MoveTowards(projectile.transform.position, position, projectileSpeed * Time.deltaTime);
            yield return null;
        }

        Destroy(projectile);
        projectile = null;
    }

    internal void UpdateWeaknessCount()
    {
        
    }

    private void StartNewWave()
    {
        m_Animator.SetTrigger("StartWave");

        foreach (Weakness weakness in weaknesses)
        {
            weakness.isEnable = false;
        }

        //waveManager.StartWave();

        //Call Wave Manager
    }

    internal void UpdatePhase(int phase)
    {
        this.phase = phase;
    }
}
