using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class QuiverInteraction : XRBaseInteractable
{
    public GameObject canvas;
    
    private IXRHoverInteractor hoverInteractor = null;

    private bool isActive = false;
    private bool isHovering = false;


    public float arrowSelectionCooldown = 0.5f; // Set your desired cooldown time
    float timeSinceLastSelection = 0f;

    private int currentArrowIndex = 0; // Assuming 0 is the default arrow type
    public List<Image> arrows;

    // Start is called before the first frame update
    void Start()
    {
        ActivateUI(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isActive)
        {
            ActionBasedController controller = hoverInteractor.transform.gameObject.GetComponent<ActionBasedController>();

            float value = controller.rotateAnchorAction.action.ReadValue<Vector2>().x;

            UpdateArrowSelection(value);
        }
    }

    public void SetHoverInteractor(HoverEnterEventArgs args)
    {
        hoverInteractor = args.interactorObject;
    }

    void UpdateArrowSelection(float horizontalInput)
    {
        // Check if enough time has passed since the last selection
        if (Time.time - timeSinceLastSelection > arrowSelectionCooldown)
        {
            // Determine the direction of arrow selection based on joystick input
            if (horizontalInput == 1)
            {
                // Update the current arrow index
                currentArrowIndex = (currentArrowIndex + 1 + arrows.Count) % arrows.Count;

            }
            else if (horizontalInput == -1)
            {
                currentArrowIndex = (currentArrowIndex + -1 + arrows.Count) % arrows.Count;
            }

            // Update the time of the last selection
            timeSinceLastSelection = Time.time;

            // Update the UI Image based on the current arrow selection
            UpdateSelectedArrowImage();
        }
    }

    void UpdateSelectedArrowImage()
    {
        for (int i = 0; i < arrows.Count; i++)
        {
            if (i == currentArrowIndex)
            {
                arrows[currentArrowIndex].enabled = true;
            }
            else
            {
                arrows[i].enabled = false;
            }
        }
    }
       

    public void ToggleUI(bool toggle)
    {
        isActive = !isActive;

        canvas.SetActive(isActive);

        ActionBasedController controller = hoverInteractor.transform.gameObject.GetComponent<ActionBasedController>();
        
        if (isActive)
        {
            controller.GetComponentInParent<SnapTurnProviderBase>().enabled = false;
        }
        else
        {
            controller.GetComponentInParent<SnapTurnProviderBase>().enabled = true;
        }
    }

    public void ActivateUI(bool active)
    {
        isActive = active;
        canvas.SetActive(active);
    }
}
