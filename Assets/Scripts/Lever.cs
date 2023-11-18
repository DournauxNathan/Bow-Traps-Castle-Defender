using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Lever : XRBaseInteractable
{
    public static event Action<float> LeverPulled;

    public Transform leverHandle, leverCap;
    public float minRotation = -45f;
    public float maxRotation = 45f;
    public float maxDistance = 0.5f; // Maximum distance for the lever interaction
    public float smoothFactor = 35f; // Maximum distance for the lever interaction

    private bool isInteracting = false;

    private Quaternion initialRotation;
    private Vector3 initialControllerPosition;
    private IXRSelectInteractor pullingInteractor = null;

    protected override void Awake()
    {
        base.Awake();
    }

    public void SetPullInteractor(SelectEnterEventArgs args)
    {
        pullingInteractor = args.interactorObject;
        initialControllerPosition = pullingInteractor.transform.localPosition;
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);
        if (updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic)
        {
            if (isSelected)
            {
                // Check the distance between the controller and the lever cap
                float distance = Vector3.Distance(pullingInteractor.transform.position, leverCap.position);

                if (distance <= maxDistance)
                {
                    float angle = GetLeverAngle(pullingInteractor);
                    angle = Mathf.Clamp(angle, minRotation, maxRotation);

                    Quaternion newRotation = Quaternion.Euler(0f, angle, 0f);
                    leverHandle.localRotation = newRotation;

                    float normalizedLeverPosition = Mathf.InverseLerp(minRotation, maxRotation, angle);
                    LeverPulled?.Invoke(normalizedLeverPosition);

                    // Haptic Feedback
                    if (pullingInteractor != null)
                    {
                        ActionBasedController controller = pullingInteractor.transform.gameObject.GetComponent<ActionBasedController>();
                        controller.SendHapticImpulse(angle, .1f);
                    }
                }
            }
        }
    }

    private float GetLeverAngle(IXRSelectInteractor interactor)
    {
        Vector3 deltaPosition = interactor.transform.localPosition - initialControllerPosition;
        float angle = Mathf.Atan(deltaPosition.x) * Mathf.Rad2Deg * smoothFactor;

        return angle;
    }
}
