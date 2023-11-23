using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;
using UnityEngine.XR.Interaction.Toolkit;

public class Lever : XRBaseInteractable
{
    public Transform leverHandle, leverCap;
    public float rotationThreshold;
    public float minThreshold;
    public float maxThreshold;

    public UnityEvent onPulled, onReleased;

    private float maxDistance = 0.5f; // Maximum distance for the lever interaction
    public float smoothFactor = 5f; // Maximum distance for the lever interaction

    private Vector3 initialToInteractor;
    private IXRSelectInteractor pullingInteractor = null;

    protected override void Awake()
    {
        base.Awake(); 
    }

    public void SetPullInteractor(SelectEnterEventArgs args)
    {
        pullingInteractor = args.interactorObject;


        initialRotation = transform.rotation;
        initialGrabLocalPosition = pullingInteractor.transform.localPosition;

        pullingInteractor.transform.position = leverCap.position ;
    }


    private Quaternion initialRotation;
    private Vector3 initialGrabLocalPosition;

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
                    /* Joytsick movement
                    // Get the rotation of the controller
                    Quaternion controllerRotation = pullingInteractor.transform.rotation;

                    // Apply the controller rotation to the lever with the initial rotation offset
                    leverHandle.rotation = Quaternion.Euler(Vector3.forward) * controllerRotation * initialRotation;
                    */

                    float angle = GetLeverAngle(pullingInteractor);
                    angle = Mathf.Clamp(angle, -rotationThreshold, rotationThreshold);

                    Debug.Log(angle);

                    /* Joytsick movement
                   // Get the rotation of the controller
                   Quaternion controllerRotation = pullingInteractor.transform.rotation;

                   // Apply the controller rotation to the lever with the initial rotation offset
                   leverHandle.rotation = Quaternion.Euler(Vector3.forward) * controllerRotation * initialRotation;
                   */

                    ActionBasedController controller = pullingInteractor.transform.gameObject.GetComponent<ActionBasedController>();
                    
                    Quaternion.Euler(angle, 0f, 0f);

                    // Calculate the target rotation based on the desired angle
                    Quaternion targetRotation = Quaternion.Euler(angle, 0f, 0f);

                    // Use Quaternion.Lerp to smoothly interpolate between the current rotation and the target rotation
                    Quaternion newRotation = Quaternion.Lerp(leverHandle.localRotation, targetRotation, Time.deltaTime * 5f);

                    // Assign the interpolated rotation to the leverHandle
                    leverHandle.localRotation = newRotation;

                    float normalizedLeverPosition = Mathf.InverseLerp(-rotationThreshold, rotationThreshold, angle);
                    //Debug.Log(normalizedLeverPosition);

                    // Haptic Feedback
                    if (normalizedLeverPosition <= 0.55f && normalizedLeverPosition >= 0.45f)
                    {
                        controller.SendHapticImpulse(angle, 0.1f);
                    }

                    if (normalizedLeverPosition >= maxThreshold)
                    {
                        onPulled?.Invoke();
                    }
                    else if (normalizedLeverPosition <= minThreshold)
                    {
                        onReleased?.Invoke();
                    }
                }
            }
        }
    }


    private float GetLeverAngle(IXRSelectInteractor interactor)
    {
        // Convert controller's position to lever's local space
        Vector3 localPosition = leverHandle.InverseTransformPoint(interactor.transform.position);

        // Calculate the angle based on the projected position
        float angle = Vector3.Angle(interactor.transform.position, localPosition) * Mathf.Sign(Vector3.Dot(interactor.transform.position, localPosition));

        return angle * smoothFactor;
    }

    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.yellow;

        DrawArrow(leverCap.position, leverCap.right * 0.05f);

        Handles.color = Color.blue;

        DrawArrow(leverCap.position, -leverCap.right * 0.05f);
    }

    private void DrawArrow(Vector3 start, Vector3 direction)
    {
        Vector3 arrowStart = start;
        Vector3 arrowEnd = start + direction;

        Handles.ArrowHandleCap(0, arrowStart, Quaternion.LookRotation(direction), 0.2f, EventType.Repaint);
    }
}
