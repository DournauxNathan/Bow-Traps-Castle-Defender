// WavesManager.cs

using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour
{
    public Transform spawnPoint; // Where critters will spawn
    public float timeBetweenWaves = 10f; // Time between waves
    private float countdown = 2f; // Initial countdown before the first wave

    private int waveNumber = 1; // Current wave number

    public CritterFactory weaklingFactory;
    public CritterFactory middlingFactory;
    public CritterFactory bossFactory;

    private bool isSpawning = true; // Flag to control spawning
    private bool bossSpawned = false;

    private CritterFactory currentFactory;

    void Start()
    {
        // Set the initial factory (Weakling)
        SetFactory(weaklingFactory);
    }

    void Update()
    {
        if (isSpawning && !bossSpawned)
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
