using UnityEngine;

public class TimeSlowPotion : Potion
{
    public float timeScale = 0.5f;
    public float duration = 5f;

    protected override void ApplyEffect(GameObject player)
    {
        base.ApplyEffect(player);

        // Apply time-slow effect
        Time.timeScale = timeScale;

        // Find all objects that need to be affected by time scale (exclude arrows)
        GameObject[] objectsToSlow = GameObject.FindGameObjectsWithTag("YourTag");

        foreach (GameObject obj in objectsToSlow)
        {
            if (!obj.CompareTag("Arrow")) // Exclude arrows from time slowdown
            {
                Rigidbody rb = obj.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.velocity *= timeScale;
                }

                // Add any other components or properties that need adjustment based on time scale
            }
        }

        Invoke("RestoreTimeScale", duration);
        Debug.Log("Time Slow Activated!");
    }

    private void RestoreTimeScale()
    {
        Time.timeScale = 1f;
        Debug.Log("Time Slow Deactivated!");
    }
}
