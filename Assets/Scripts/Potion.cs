using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Potion : XRGrabInteractable
{
    protected virtual void ApplyEffect(GameObject player)
    {
        // Base implementation (no effect)
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ApplyEffect(other.gameObject);
            Destroy(gameObject);
        }
    }
}
