using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BowController : MonoBehaviour
{
    public GameObject arrowPrefab;
    public Transform arrowSpawnPoint;
    public float shootingForce = 10f;

    private XRGrabInteractable grabInteractable;

    void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        // Add logic for what happens when the bow is grabbed
    }

    void OnRelease(SelectExitEventArgs args)
    {
        // Add logic for what happens when the bow is released
        ShootArrow();
    }

    void ShootArrow()
    {
        GameObject arrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, arrowSpawnPoint.rotation);
        Rigidbody arrowRb = arrow.GetComponent<Rigidbody>();
        arrowRb.AddForce(arrowSpawnPoint.forward * shootingForce, ForceMode.Impulse);
    }
}
