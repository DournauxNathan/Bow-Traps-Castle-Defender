using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class WaveManager : MonoBehaviour
{
    public Transform spawnPoint; // Where critters will spawn
    public Transform bossSpawnPoint; // Where critters will spawn
    public float timeBetweenWaves = 10f; // Time between waves
    public int startTimer = 3;
    private float countdown = 2f; // Initial countdown before the first wave

    private int waveNumber = 1; // Current wave number

    public CritterFactory weaklingFactory;
    public CritterFactory middlingFactory;
    public CritterFactory bossFactory;

    public int baseWaveCompletionBonus = 10; // Base bonus amount
    public float waveCompletionMultiplier = 1.2f; // Multiplier for each completed wave

    private bool isSpawning = false;
    private bool bossSpawned = false;

    private CritterFactory currentFactory;
    private Boss currentBoss;
    
    private int critterSpawned;
    private int waveNumberCrittersKilled;

    public AudioSource m_AudioSource;
    public AudioClip onWaveStartSFX, onWaveEndSFX;

    public UnityEvent onWaveStart, onWaveEnd,onFinalWave, onBossAppear, OnBossDead;

    void Start()
    {
        m_AudioSource.GetComponent<AudioSource>();

        // Set the initial factory (Weakling)
        SetFactory(weaklingFactory);
    }

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

    public void StartWave(int maxWave, int numberOfCritter)
    {
        if (!isSpawning)
        {
            waveNumberCrittersKilled = 0;
            critterSpawned = 0;

            StartCoroutine(SpawnWave(maxWave, numberOfCritter));

        }
        else
        {
            Debug.LogWarning("Can't start Wave ! Wave already started");
        }
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


    void Update()
    {
        /*if (bossSpawned)
        {
            if (countdown <= 0f)
            {
                StopCoroutine(SpawnWave());
                // Start a new wave
                countdown = timeBetweenWaves;
            }

            countdown -= Time.deltaTime;
        }
        else
        {
            StopCoroutine(SpawnWave());
        }*/
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

    IEnumerator SpawnWave(int numberOfWave, int numberOfCritter)
    {
        if (currentFactory != null)
        {
            while (critterSpawned != waveNumber)
            {
                if (waveNumber >= numberOfWave)
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

    void SpawnCritter(CritterFactory factory)
    {
        critterSpawned++;

        if (factory != null)
        {
            GameObject critter = factory.CreateCritter(spawnPoint);

            // Set the PlayerController reference for the critter
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
        //Debug.Log("The boss has appeared");
        
        if (currentFactory != null)
        {
            GameObject boss = currentFactory.SpawnBoss(bossSpawnPoint);

            // Set the PlayerController reference for the critter
            Boss bossComponent = boss.GetComponent<Boss>();

            if (bossComponent != null)
            {
                //Initialize Critter Data to Critter
                bossComponent.Init(currentFactory.bossData, this);
            }
            currentBoss = bossComponent;
        }
        
        currentFactory = null;
    }


    void OnCritterKilled()
    {
        waveNumberCrittersKilled++;

        // Check if all critters of the current wave are killed
        if (waveNumberCrittersKilled >= critterSpawned )
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
                // Spawn boss After Killing the all critter
                SetFactory(bossFactory);
                SpawnBoss();
                isSpawning = false;

                //Enter Boss Made + Manage Wave from it
                // Update Boss Phase
                //currentBoss.UpdatePhase(2);
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

    int GetTotalWaves()
    {
        // You can customize how the total number of waves is determined
        return 2; // Example: 10 waves in total
    }

    void SetFactory(CritterFactory factory)
    {
        currentFactory = factory;
    }
}
