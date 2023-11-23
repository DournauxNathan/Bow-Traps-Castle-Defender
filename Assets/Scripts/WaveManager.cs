// WavesManager.cs

using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour
{
    public Transform spawnPoint; // Where critters will spawn
    public float timeBetweenWaves = 10f; // Time between waves
    public int startTimer = 3;
    private float countdown = 2f; // Initial countdown before the first wave

    private int waveNumber = 1; // Current wave number

    public CritterFactory weaklingFactory;
    public CritterFactory middlingFactory;
    public CritterFactory bossFactory;

    private bool isSpawning = false;
    private bool bossSpawned = false;

    private CritterFactory currentFactory;
    
    private int critterSpawned;
    private int waveNumberCrittersKilled;

    void Start()
    {
        // Set the initial factory (Weakling)
        SetFactory(weaklingFactory);
    }
    void Update()
    {
        if (isSpawning)
        {
            if (countdown <= 0f)
            {
                // Start a new wave
                StartCoroutine(SpawnWave());
                countdown = timeBetweenWaves;
            }

            countdown -= Time.deltaTime;
        }
        else
        {
            StopCoroutine(SpawnWave());
        }
    }

    public void StartTimer()
    {
        StartCoroutine(Countdown(startTimer));
    }

    int counter;
    IEnumerator Countdown(int seconds)
    {
        counter = seconds;
        while (counter > 0)
        {
            yield return new WaitForSeconds(1);
            counter--;

            if (counter <= 3 && counter != 0)
            {
                Debug.Log("Wabe begin : " + counter);
            }
        }
        Debug.Log("Go!");
        StartWave();
    }
    
    public void StartWave()
    {
        if (!isSpawning)
        {
            waveNumberCrittersKilled = 0;

            isSpawning = true;
            bossSpawned = false;
        }
        else
        {
            Debug.LogWarning("Can't Start Wave ! Wave already started");
        }
    }

    public void StopWave()
    {
        // Stop spawning and movement
        isSpawning = false;
        GameManager.Instance.gate.Close();
    }

    

    IEnumerator SpawnWave()
    {
        if (!bossSpawned && currentFactory != null)
        {
            //Debug.Log("Wave " + waveNumber + " Incoming!");

            for (int i = 0; i < waveNumber; i++)
            {
                SpawnCritter();
                yield return new WaitForSeconds(1f); // Time between spawning critters in a wave
            }

            // After spawning regular critters, spawn the Boss on the last wave
            if (waveNumber >= GetTotalWaves() && isSpawning && !bossSpawned)
            {
                SetFactory(middlingFactory);
                SpawnCritter();
            }

            if (waveNumber >= GetTotalWaves() + 10 && isSpawning && !bossSpawned)
            {
                SetFactory(bossFactory);
                SpawnBoss();
                bossSpawned = true; // Set the flag to indicate that the boss has been spawned
                isSpawning = false; // Stop spawning regular waves after the boss is spawned
            }

            waveNumber++;
        }
    }

    void SpawnCritter()
    {
        critterSpawned++;

        if (currentFactory != null)
        {
            GameObject critter = currentFactory.CreateCritter(spawnPoint);

            // Set the PlayerController reference for the critter
            Critter critterComponent = critter.GetComponent<Critter>();
            if (critterComponent != null)
            {
                //Initialize Critter Data to Critter
                critterComponent.Init(currentFactory.critterData);

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
            GameObject critter = currentFactory.CreateCritter(spawnPoint);
        }
        
        currentFactory = null;
    }


    void OnCritterKilled()
    {
        waveNumberCrittersKilled++;

        // Check if all critters of the current wave are killed
        if (waveNumberCrittersKilled >= critterSpawned)
        {
            StopWave();
            waveNumber++;
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
