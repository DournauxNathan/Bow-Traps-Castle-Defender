using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Hammer : XRGrabInteractable
{
    [Header("SETTINGS")]
    public float repairRate = 1f; // Rate at which the hammer repairs traps

    public AudioSource m_AudioSource;
    public AudioClip onHitSound;


    private IXRSelectInteractor pullingInteractor = null;

    // OnTriggerEnter is called when the Collider other enters the trigger.
    private void OnTriggerEnter(Collider other)
    {
        Activator activator = other.GetComponent<Activator>();

        if (activator != null && activator.IsBroken) 
        {
            m_AudioSource.PlayOneShot(onHitSound);

            // Haptic Feedback
            if (pullingInteractor != null)
            {
                ActionBasedController controller = pullingInteractor.transform.gameObject.GetComponent<ActionBasedController>();
                controller.SendHapticImpulse(5, .1f);

            }

            Repair(activator);
        }
    }

    private void Repair(Activator activator)
    {
        if (activator.Repairable && activator.IsBroken)
        {
            float repairAmount = repairRate * Time.deltaTime + .5f;
            activator.Repair(repairAmount);
        }
    }
}
