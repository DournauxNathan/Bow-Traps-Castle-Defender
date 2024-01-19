using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [Header("REFS")]
    [SerializeField] private Animator m_Animator;
    private WaveManager waveManager;

    [Header("PROPERTIES")]
    public CritterType type;
    public int maxHealth;
    public int damagePerHit;
    private float timeBeforeReleaseCast;
    private int critterToSpawn;
    private int currentWave;

    [Header("PHASE")]
    public int phase;
    public List<Weakness> weaknesses;
    public bool hasPhaseBegin = false;

    [Header("PROJECTILE PROPERTIES")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint; // The position where the projectile will be spawned
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private float growthSpeed;
   
    private int health = 1; 
    private GameObject projectile;
    private Vector3 targetPosition;

    [Header("Load Data")]
    [SerializeField] private BossData data;

    public void Init(BossData data, WaveManager waveManager)
    {
        this.type = CritterType.Boss;
        this.maxHealth = data.maxHealth;
        this.health = maxHealth;
        this.damagePerHit = data.damagePerHit;
        this.critterToSpawn = data.waves[0].critterToSpawn;
        this.currentWave = data.waves[0].waveId;

        this.phase = data.phase;
        this.timeBeforeReleaseCast = data.timeBeforeReleaseCast;

        m_Animator = GetComponent<Animator>();
        this.waveManager = waveManager;

        if (waveManager.currentBoss == null)
        {
            waveManager.SetBoss(this);
        }

        foreach (Weakness weakness in weaknesses)
        {
            weakness.bossData = this;
        }

        Invoke("OnBeginPhase", 5f);
    }



#if UNITY_EDITOR
    private void Start()
    {
        Init(data, GameObject.Find("Wave Manager").GetComponent<WaveManager>());
    }
#endif

    public void CancelCast()
    {
        if (projectile != null)
        {
            projectile.GetComponent<Projectile>().TooglePhysics(true);
            OnEndPhase();
            Invoke("StartNewWave", 3f);
        }
        projectile = null;
    }

    public void OnBeginPhase()
    {
        if (phase == 0)
        {
            NewPhase(1);
        }
    }

    public void OnEndPhase()
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
                    if (projectile == null)
                    {
                        PrepareCast();                    
                    }
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
        List<BreakableActivator> activators = GameManager.Instance.GetActivators();

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
        
        yield return new WaitForSeconds(timeBeforeReleaseCast);
        
        m_Animator.SetTrigger("Release");

        while (Vector3.Distance(projectile.transform.position, position) >= .5f)
        {
            projectile.transform.position = Vector3.MoveTowards(projectile.transform.position, position, projectileSpeed * Time.deltaTime);
            yield return null;
        }

        Destroy(projectile);
        projectile = null;

        yield return new WaitForSeconds(timeBeforeReleaseCast);

        float rand = UnityEngine.Random.Range(0f, 1f);
        
        if (rand < 0.5f)
        {
            NewPhase(1);
        }
        else
        {
            if (!waveManager.isSpawning)
            {
                NewPhase(2);
            }
        }

        yield return null;
    }

    internal void UpdateWeaknessCount()
    {
        if (phase == 1)
        {
            UpdatePhase(2);

            if (health == (maxHealth / 2) )
            {
                this.critterToSpawn = data.waves[1].critterToSpawn;
                this.currentWave = data.waves[1].waveId;
            }
            else
            {
                this.critterToSpawn = data.waves[0].critterToSpawn;
                this.currentWave = data.waves[0].waveId;
            }
        }     
    }

    private void StartNewWave()
    {
        m_Animator.SetTrigger("StartWave");
       
        waveManager.StartWave(critterToSpawn);
    }

    public void TakeDamage()
    {
        health -= damagePerHit;

        if (health <= 0f)
        {
            m_Animator.SetBool("isDead", true);

            Invoke("Defeat", 10f);
        }
    }

    public void Defeat()
    {
        LevelManager.Instance?.LoadSceneAsync("Crédits");
    }

    internal void UpdatePhase(int phase)
    {
        this.phase = phase;
    }

    internal void NewPhase(int phase)
    {
        this.phase = phase;
        hasPhaseBegin = true;
    }
}
