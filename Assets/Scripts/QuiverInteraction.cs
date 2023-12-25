using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class QuiverInteraction : XRBaseInteractable
{
    public static Action<Arrow> AddNewArrow;

    public ArrowSpawner spawner;

    public GameObject canvas;
    public Image icon;
    
    private IXRHoverInteractor hoverInteractor = null;

    private bool isActive = false;
    private bool isHovering = false;


    public float arrowSelectionCooldown = 0.5f; // Set your desired cooldown time
    float timeSinceLastSelection = 0f;

    private int currentArrowIndex = 0; // Assuming 0 is the default arrow type
    public List<Slot> arrows;
    private Slot currentArrowSelected;

    // Start is called before the first frame update
    void Start()
    {
        ActivateUI(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (arrows.Count != 0 && isActive)
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
            int direction = 0;

            // Determine the direction of arrow selection based on joystick input
            if (horizontalInput == 1)
            {
                direction = 1;
            }
            else if (horizontalInput == -1)
            {
                direction = -1;
            }

            // Update the current arrow index
            currentArrowIndex = (currentArrowIndex + direction + arrows.Count) % arrows.Count;

            // Update the time of the last selection
            timeSinceLastSelection = Time.time;

            // Update the UI Image based on the current arrow selection
            UpdateSelectedArrowImage();
        }
    }

    void UpdateSelectedArrowImage()
    {
        icon.sprite = arrows[currentArrowIndex].icon;
        currentArrowSelected = arrows[currentArrowIndex];
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<UIArrow>(out UIArrow arrow))
        {
            Unlock(arrow);
        }
    }

    private void Unlock(UIArrow newArrow)
    {
        Slot newSlot = new Slot {
            name = newArrow.gameObject.name,
            type = newArrow.type,
            icon = newArrow.m_Sprite,
            isUnlock = true
        };

        if (!arrows.Contains(newSlot))
        {
            arrows.Add(newSlot);
        }

        newArrow.gameObject.SetActive(false);
    }

    public void SelectArrow()
    {
        spawner.UpdateArrowPrefab(currentArrowSelected.name);
    }
}

[System.Serializable]
public class Slot
{
    public string name;
    public ArrowType type;
    public Sprite icon;
    public bool isUnlock;
}