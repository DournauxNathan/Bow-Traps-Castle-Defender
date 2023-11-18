using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PullInteraction : XRBaseInteractable
{
    public static event Action<float> PullActionReleased;
    public Transform start, end;
    public GameObject notch;
    public float pullAmount { get; private set; } = 0.0f;

    private LineRenderer m_LineRenderer;
    private IXRSelectInteractor pullingInteractor = null;

    private AudioSource m_AudioSource;
    public AudioClip onLoading, onRelease;

    protected override void Awake()
    {
        base.Awake();
        m_LineRenderer = GetComponent<LineRenderer>();
        m_AudioSource = GetComponent<AudioSource>();
    }

    public void SetPullInteractor(SelectEnterEventArgs args)
    {
        pullingInteractor = args.interactorObject;
    }

    public void Release()
    {
        PullActionReleased?.Invoke(pullAmount);
        pullingInteractor = null;
        pullAmount = 0;
        notch.transform.localPosition = new Vector3(notch.transform.localPosition.x, notch.transform.localPosition.y, 0f);
        
        UpdateString();

        m_AudioSource.PlayOneShot(onRelease);
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);
        if (updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic)
        {
            if (isSelected)
            {
                Vector3 pullPosition = pullingInteractor.transform.position;
                Debug.Log("", pullingInteractor.transform.gameObject);
                pullAmount = CaculatePull(pullPosition);

                // Haptic Feedback
                if (pullingInteractor != null)
                {
                    ActionBasedController controller = pullingInteractor.transform.gameObject.GetComponent<ActionBasedController>();
                    controller.SendHapticImpulse(pullAmount, .1f);
                    
                }

                UpdateString();
            }
        }
    }

    private float CaculatePull(Vector3 pullPosition)
    {
        Vector3 pullDirection = pullPosition - start.position;
        Vector3 targetDirection = end.position - start.position;
        float maxLength = targetDirection.magnitude;

        targetDirection.Normalize();
        float pullValue = Vector3.Dot(pullDirection, targetDirection) / maxLength;

        return Mathf.Clamp(pullValue, 0, 1);
    }

    private void UpdateString()
    {
        Vector3 linePos = Vector3.forward * Mathf.Lerp(start.localPosition.z, end.localPosition.z, pullAmount);
        notch.transform.localPosition = new Vector3(notch.transform.localPosition.x, notch.transform.localPosition.y, linePos.z + 0.2f);
        m_LineRenderer.SetPosition(1, linePos);
    }

    private void OnDrawGizmos()
    {
        // Draw a sphere at the position of the 'start' transform
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(start.position, 0.015f);

        // Draw a sphere at the position of the 'end' transform
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(end.position, 0.015f);

        // Draw a line between 'start' and 'end' for better visualization
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(start.position, end.position);
    }
}
