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

    private CritterFactory currentFactory;

    void Start()
    {
        // Set the initial factory (Weakling)
        SetFactory(weaklingFactory);
    }

    void Update()
    {
        if (countdown <= 0f)
        {
            // Start a new wave
            StartCoroutine(SpawnWave());
            countdown = timeBetweenWaves;
        }

        countdown -= Time.deltaTime;
    }

    IEnumerator SpawnWave()
    {
        if (isSpawning)
        {
            Debug.Log("Wave " + waveNumber + " Incoming!");

            for (int i = 0; i < waveNumber; i++)
            {
                if (isSpawning)
                {
                    SpawnCritter();
                }
                yield return new WaitForSeconds(1f); // Time between spawning critters in a wave
            }

            // After spawning regular critters, spawn the Boss on the last wave
            if (waveNumber >= GetTotalWaves())
            {
                SetFactory(bossFactory);

                SpawnCritter();
            }

            waveNumber++;
        }
    }

    void SpawnCritter()
    {
        GameObject critter = currentFactory.CreateCritter();
        //GameObject spawnedCritter = Instantiate(critter, spawnPoint.position, spawnPoint.rotation);

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

    // Event handler for critter reaching its destination
    void OnCritterDestinationReached()
    {
        // Handle any logic when a critter reaches its destination
        Debug.Log("Your survive during " + waveNumber + "waves");

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
        return 10; // Example: 10 waves in total
    }

    void SetFactory(CritterFactory factory)
    {
        currentFactory = factory;
    }
}
