using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class WaveManager : MonoBehaviour
{
    #region Public Variables

    [Header("Spawn Settings")]
    public Transform spawnPoint; // Where critters will spawn
    public Transform bossSpawnPoint; // Where the boss will spawn

    [Header("Wave Settings")]
    public float timeBetweenWaves = 10f; // Time between waves
    public int startTimer = 3; // Countdown before the first wave

    [Header("Critter Factories")]
    public CritterFactory weaklingFactory;
    public CritterFactory middlingFactory;
    public CritterFactory bossFactory;

    [Header("Wave Completion")]
    public int baseWaveCompletionBonus = 10; // Base bonus amount
    public float waveCompletionMultiplier = 1.2f; // Multiplier for each completed wave

    [Header("Wave Control")]
    public UnityEvent onWaveStart;
    public UnityEvent onWaveEnd;
    public UnityEvent onFinalWave;
    public UnityEvent onBossAppear;
    public UnityEvent OnBossDead;

    public bool isSpawning { get; private set; } // Indicates whether spawning is in progress

    #endregion

    #region Private Variables
    
    private int waveNumber = 1; // Current wave number
    private bool bossSpawned = false;

    private CritterFactory currentFactory;
    public Boss currentBoss { get; private set; }

    private int critterSpawned;
    private int waveNumberCrittersKilled;

    public AudioSource m_AudioSource;
    public AudioClip onWaveStartSFX, onWaveEndSFX;

    #endregion

    #region Initialization

    void Start()
    {
        m_AudioSource.GetComponent<AudioSource>();

        // Set the initial factory (Weakling)
        SetFactory(weaklingFactory);
    }

    #endregion

    #region Wave Management

    public void StartTimer()
    {
        StartCoroutine(Countdown(startTimer));
    }

    IEnumerator Countdown(int seconds)
    {
        int counter = seconds;
        while (counter > 0)
        {
            Debug.Log("Wave begin : " + counter);
            yield return new WaitForSeconds(1);
            counter--;
        }

        onWaveStart?.Invoke();
        m_AudioSource.PlayOneShot(onWaveStartSFX);
        Debug.Log("Wave " + waveNumber + " Incoming!");
        yield return new WaitForSeconds(onWaveEndSFX.length);
        StartWave();
    }

    public void StartWave()
    {
        if (!isSpawning)
        {
            waveNumberCrittersKilled = 0;
            critterSpawned = 0;

            StartCoroutine(SpawnWave());

            isSpawning = true;

            if (bossSpawned)
            {
                onBossAppear?.Invoke();
            }
        }
        else
        {
            Debug.LogWarning("Can't start Wave ! Wave already started");
        }
    }

    public void StartWave(int numberOfCritter)
    {
        if (!isSpawning)
        {
            waveNumberCrittersKilled = 0;
            critterSpawned = 0;

            isSpawning = true;
            StartCoroutine(SpawnWave(numberOfCritter));

        }
        else
        {
            Debug.LogWarning("Can't start Wave ! Wave already started");
        }
    }

    public void StopWave()
    {
        // Stop spawning
        isSpawning = false;
        GameManager.Instance.gate.Close();

        onWaveEnd?.Invoke();

        if (waveNumber == GetTotalWaves() - 1)
        {
            onFinalWave?.Invoke();
        }

        if (!m_AudioSource.isPlaying)
        {
            m_AudioSource.PlayOneShot(onWaveEndSFX);
        }
    }

    #endregion

    #region Coroutine Methods

    IEnumerator SpawnWave()
    {
        if (currentFactory != null)
        {
            while (critterSpawned != waveNumber)
            {
                if (waveNumber >= 3)
                {
                    // Decide whether to spawn middlingCritter or a regular Critter
                    if (Random.Range(0f, 1f) < 0.5f)
                    {
                        SpawnCritter(middlingFactory);
                        yield return new WaitForSeconds(2f);
                    }
                    else
                    {
                        SpawnCritter(weaklingFactory);
                    }
                }
                else
                {
                    // Spawn regular Critter
                    SpawnCritter(weaklingFactory);
                }
                yield return new WaitForSeconds(1f); // Time between spawning critters in a wave
            }
        }
    }

    IEnumerator SpawnWave(int numberOfCritter)
    {
        if (currentFactory != null)
        {
            while (critterSpawned != numberOfCritter)
            {
                // Decide whether to spawn middlingCritter or a regular Critter
                if (Random.Range(0f, 1f) < 0.5f)
                {
                    SpawnCritter(middlingFactory);
                    yield return new WaitForSeconds(1f);
                }
                else
                {
                    SpawnCritter(weaklingFactory);

                    currentBoss.NewPhase(1);
                }
                yield return new WaitForSeconds(1f); // Time between spawning critters in a wave
            }
        }
    }

    #endregion

    #region Critter and Boss Spawning

    void SpawnCritter(CritterFactory factory)
    {
        critterSpawned++;

        if (factory != null)
        {
            GameObject critter = factory.CreateCritter(spawnPoint);

            Critter critterComponent = critter.GetComponent<Critter>();
            if (critterComponent != null)
            {
                //Initialize Critter Data to Critter
                critterComponent.Init(factory.critterData);

                // Subscribe to the OnDestinationReached event
                critterComponent.OnDestinationReached += OnCritterDestinationReached;
                critterComponent.OnKilled += OnCritterKilled;
            }
        }
    }

    void SpawnBoss()
    {
        bossSpawned = true;

        if (currentFactory != null)
        {
            GameObject boss = currentFactory.SpawnBoss(bossSpawnPoint);

            Boss bossComponent = boss.GetComponent<Boss>();

            if (bossComponent != null)
            {
                //Initialize Critter Data to Critter
                bossComponent.Init(currentFactory.bossData, this);
            }
            SetBoss(bossComponent);
        }

        currentFactory = null;
    }

    #endregion

    #region Boss Handling

    public void SetBoss(Boss boss)
    {
        currentBoss = boss;
    }

    #endregion

    #region Event Handlers

    void OnCritterKilled()
    {
        waveNumberCrittersKilled++;

        // Check if all critters of the current wave are killed
        if (waveNumberCrittersKilled >= critterSpawned)
        {
            if (!bossSpawned)
            {
                StopWave();

                // Calculate wave completion bonus based on the multiplier and the number of completed waves
                int waveBonus = Mathf.RoundToInt(baseWaveCompletionBonus * Mathf.Pow(waveCompletionMultiplier, waveNumber - 1));

                // Award wave completion bonus
                GameManager.Instance.currentCurrency += waveBonus;

                waveNumber++;

                //Spawn Boss at the final Wave
                if (waveNumber == GetTotalWaves())
                {
                    Debug.Log("Active boss");
                    bossSpawned = true;
                }
            }
            else
            {
                isSpawning = false;
                // Spawn boss After Killing all critters from last waves
                if (currentBoss == null)
                {
                    SetFactory(bossFactory);
                    SpawnBoss();
                }
                else // Boss is already spawned
                {
                    currentBoss.UpdatePhase(1);
                    currentBoss.hasPhaseBegin = true;
                }
            }
        }
    }

    // Event handler for critter reaching its destination
    void OnCritterDestinationReached()
    {
        // Handle any logic when a critter reaches its destination
        //Debug.Log("Your survive during " + (waveNumber - 1) + "waves");

        // Stop spawning and movement
        isSpawning = false;

        // Iterate through all spawned critters and stop their movement
        Critter[] critters = FindObjectsOfType<Critter>();
        foreach (var critter in critters)
        {
            critter.StopMovement();
        }
    }

    #endregion

    #region Utility Methods

    int GetTotalWaves()
    {
        // You can customize how the total number of waves is determined
        return 3; // Example: 10 waves in total
    }

    void SetFactory(CritterFactory factory)
    {
        currentFactory = factory;
    }

    #endregion
}
