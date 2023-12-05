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

    public void CancelAction()
    {
        StopAllCoroutines();
        phase = 0;
        m_Animator.SetTrigger("Reset");
        Destroy(projectile);
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

    void PrepareCast()
    {
        int rand = UnityEngine.Random.Range(0, GameManager.Instance.activators.Count);
        Vector3 targetPosition = GameManager.Instance.activators[rand].transform.position;

        // Instantiate the growing projectile prefab
        projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        projectile.transform.parent = null;
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


    IEnumerator CastProjectile(GameObject projectile, Vector3 position)
    {
        // Continue growing the projectile until it reaches its maximum scale
        while (projectile.transform.localScale.x <= 1.5f)
        {
            float growthAmount = growthSpeed * Time.deltaTime;
            projectile.transform.localScale += Vector3.one * growthAmount;

            yield return null;
        }

        m_Animator.SetTrigger("Release");

        while (Vector3.Distance(projectile.transform.position, position) >= .5f)
        {
            projectile.transform.position = Vector3.Lerp(projectile.transform.position, position, projectileSpeed * Time.deltaTime);

            yield return null;
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
