using UnityEngine;

[CreateAssetMenu(fileName = "New Trap", menuName = "Traps/Trap")]
public class Trap : ScriptableObject
{
    public GameObject trapPrefab; // The visual representation of the trap in the game world
    public float cooldownDuration = 10f; // Cooldown time in seconds
    public float trapHealth = 100f; // Initial health of the trap
    public float maxHealth = 100f; // Maximum health of the trap
    public float breakDuration = 30f; // Time it takes for the trap to repair itself after breaking

    private float cooldownTimer = 0f; // Timer for cooldown
    private float breakTimer = 0f; // Timer for trap break

    public bool CanActivate()
    {
        return cooldownTimer <= 0f && trapHealth > 0f;
    }

    public void Activate()
    {
        if (CanActivate())
        {
            // Perform trap activation logic here

            // Start cooldown timer
            cooldownTimer = cooldownDuration;
        }
    }

    public void UpdateTimers()
    {
        // Update cooldown timer
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }

        // Update break timer
        if (trapHealth <= 0f)
        {
            if (breakTimer < breakDuration)
            {
                breakTimer += Time.deltaTime;
            }
            else
            {
                // Repair the trap after the break duration
                trapHealth = maxHealth;
                breakTimer = 0f;
            }
        }
    }

    public void Break()
    {
        // Simulate the trap breaking
        trapHealth = 0f;
    }

    public void Repair()
    {
        // Repair the trap if it's broken and the player has a hammer or other repair tool
        if (trapHealth <= 0f)
        {
            trapHealth = maxHealth;
        }
    }
}
