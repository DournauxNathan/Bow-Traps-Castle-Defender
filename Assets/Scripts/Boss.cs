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
    private int phase;

    public int damagePerHit;

    public List<Weakness> weaknesses;

    public Animator m_Animator;
    internal WaveManager waveManager;

    public GameObject projectilePrefab;
    public Transform firePoint; // The position where the projectile will be spawned
    public float projectileSpeed = 10f;
    private float growthSpeed;

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

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (phase)
        {
            case 1:

                break;
            case 2:
                StartNewWave();
                break;
        }
    }

    void PrepareCast()
    {
        int rand = UnityEngine.Random.Range(0, GameManager.Instance.activators.Count);
        Vector3 targetPosition = GameManager.Instance.activators[rand].transform.position;

        // Instantiate the growing projectile prefab
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        // Calculate the direction towards the target position
        Vector3 direction = (targetPosition - firePoint.position).normalized;

        // Set the rotation of the growing projectile to face the target position
        projectile.transform.rotation = Quaternion.LookRotation(direction);

        // Set the initial scale to be very small
        projectile.transform.localScale = Vector3.one * 0.01f;

        m_Animator.SetTrigger("Cast");

        // Start the growth coroutine
        StartCoroutine(CastProjectile(projectile));
    }


    IEnumerator CastProjectile(GameObject projectile)
    {
        // Continue growing the projectile until it reaches its maximum scale
        while (projectile.transform.localScale.x < 1.5f)
        {
            float growthAmount = growthSpeed * Time.deltaTime;
            projectile.transform.localScale += Vector3.one * growthAmount;

            yield return null;
        }


        m_Animator.SetTrigger("Release");

        // Once the projectile has grown, throw it
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();
        if (projectileRb != null)
        {
            projectileRb.velocity = projectile.transform.forward * projectileSpeed;
        }
    }

    internal void UpdateWeaknessCount()
    {
        
    }

    private void StartNewWave()
    {
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
