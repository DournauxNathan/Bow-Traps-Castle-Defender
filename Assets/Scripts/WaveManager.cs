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
    public AudioClip onWaveStart, onWaveEnd;

    public UnityEvent onFinalWave, onBossAppear, OnBossDead;

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
            yield return new WaitForSeconds(1);
            counter--;

            if (counter <= 3 && counter != 0)
            {
                Debug.Log("Wabe begin : " + counter);
            }
        }

        m_AudioSource.PlayOneShot(onWaveStart);
        yield return new WaitForSeconds(onWaveEnd.length);
        StartWave();
    }
    
    public void StartWave()
    {
        Debug.Log("Wave " + waveNumber + " Incoming!");

        if (waveNumber == GetTotalWaves())
        {
            onBossAppear?.Invoke();
        }

        if (!isSpawning)
        {
            waveNumberCrittersKilled = 0;
            critterSpawned = 0;

            StartCoroutine(SpawnWave());

            isSpawning = true;
            bossSpawned = false;
        }
        else
        {
            Debug.LogWarning("Can't start Wave ! Wave already started");
        }
    }


    void Update()
    {
        if (bossSpawned)
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
        }
    }

    public void StopWave()
    {
        // Stop spawning
        isSpawning = false;
        GameManager.Instance.gate.Close();

        if (waveNumber == GetTotalWaves() - 1)
        {
            onFinalWave?.Invoke();
        }

        if (!m_AudioSource.isPlaying)
        {
            m_AudioSource.PlayOneShot(onWaveEnd);
        }
    }

    IEnumerator SpawnWave()
    {
        if (currentFactory != null)
        {
            //Spawn Boss at the final Wave
            if (!bossSpawned && waveNumber == GetTotalWaves())
            {
                bossSpawned = true;
                isSpawning = false;

                SetFactory(bossFactory);
                SpawnBoss();
            }

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
                bossComponent.Init(currentFactory.bossData);
                bossComponent.waveManager = this;
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
            }
            else
            {
                // Update Boss Phase
                currentBoss.UpdatePhase(2);

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
