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

    private bool isInteracting = false;

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
                    // Calculate the rotation based on the change in position of the grab point
                    Vector3 pullDirection = pullingInteractor.transform.localPosition - initialGrabLocalPosition;
                    float pullAmount = Mathf.Clamp(pullDirection.x, -rotationThreshold, rotationThreshold);

                    // Update Rotation
                    float angle = pullAmount;
                    Quaternion newRotation = initialRotation * Quaternion.AngleAxis(angle, transform.right);
                    transform.rotation = newRotation;



                    /*// Calculate the rotation based on the change in position of the grab point
                    float angle = Vector3.Dot(pullingInteractor.transform.localPosition - initialGrabLocalPosition, leverCap.forward) * 500f;

                    // Apply the rotation to the lever
                    transform.rotation = initialRotation * Quaternion.AngleAxis(angle, transform.right);*/

                    /* Joytsick movement
                    // Get the rotation of the controller
                    Quaternion controllerRotation = pullingInteractor.transform.rotation;

                    // Apply the controller rotation to the lever with the initial rotation offset
                    leverHandle.rotation = Quaternion.Euler(Vector3.forward) * controllerRotation * initialRotation;
                    */

                    /*
                    float angle = GetLeverAngle(pullingInteractor);
                    angle = Mathf.Clamp(angle, -rotationThreshold, rotationThreshold);

                    Quaternion newRotation = Quaternion.Euler(0f, 0f, angle);

                    leverHandle.localRotation = newRotation;

                    float normalizedLeverPosition = Mathf.InverseLerp(-rotationThreshold, rotationThreshold, angle);
                    Debug.Log(normalizedLeverPosition);

                    // Haptic Feedback
                    if (normalizedLeverPosition <= 0.55f && normalizedLeverPosition >= 0.45f)
                    {
                        ActionBasedController controller = pullingInteractor.transform.gameObject.GetComponent<ActionBasedController>();
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
                    */
                }
            }
        }
    }

    private float GetLeverAngle(IXRSelectInteractor interactor)
    {
        // Get the local position of the lever relative to its parent
        Vector3 deltaPosition = leverHandle.localPosition - interactor.transform.position;

        // Apply the inverse of the lever's rotation to the delta position
        Vector3 rotatedDeltaPosition = Quaternion.Inverse(leverHandle.localRotation) * deltaPosition * smoothFactor;

        // Calculate the angle using the rotated delta position
        float angle = Mathf.Atan(rotatedDeltaPosition.x) * Mathf.Rad2Deg * smoothFactor;

        return angle;
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
