using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class XRSettingsListener : MonoBehaviour
{
    public VignetteApplier vignetteApplier;
    public ActionBasedControllerManager RightControllerManager;
    public ActionBasedControllerManager LeftControllerManager;

    [Header("Interactors")]
    public GameObject rightTeleportInteractor;
    public GameObject leftTeleportInteractor;

    public AutoScaler autoScaler;

    [Header("Providers")]
    public SnapTurnProviderBase snapTurnProvider;
    public DynamicMoveProvider moveProvider;

    public MainMenu menuManager;

    public GameObject bodySocketInventory;

    private InputData _inputData;

    private void Awake()
    {
        // Get reference to InputData component
        _inputData = GetComponent<InputData>();

        XRSettingsManager.XRSettingsChange += UpdateXRSettings;
    }

    private void Start()
    {
        XRSettingsManager.XRRecalibrate += UpdateHeadSetHeight;
        XRSettingsManager.XRInventoryUpdate += EnableInventory;
        
        UpdateXRSettings();
        EnableInventory();
        
        GameManager.Instance.setRig(transform);
    }

    private void OnDestroy()
    {
        XRSettingsManager.XRSettingsChange -= UpdateXRSettings;
    }

    private void LateUpdate()
    {
        if (_inputData._leftController.TryGetFeatureValue(CommonUsages.menuButton, out bool Menubutton))
        {
            if (Menubutton)
            {
                menuManager.OpenSettings();
            }
        }
    }

    private void EnableInventory()
    {
        if (XRSettingsManager.Instance != null)
        {
            bodySocketInventory.SetActive(XRSettingsManager.Instance.isInventoryActive());
        }
    }

    private void UpdateXRSettings()
    {
        if(XRSettingsManager.Instance != null)
        {
            vignetteApplier.enabled = XRSettingsManager.Instance.isVignetteActive();
            RightControllerManager.smoothTurnEnabled = XRSettingsManager.Instance.isContinuousTurnActive();

            // Teleport Provider
            rightTeleportInteractor.SetActive(XRSettingsManager.Instance.isTeleportActive());
            leftTeleportInteractor.SetActive(XRSettingsManager.Instance.isTeleportActive());

            moveProvider.enabled = !XRSettingsManager.Instance.isTeleportActive();
            LeftControllerManager.smoothMotionEnabled = !XRSettingsManager.Instance.isTeleportActive();

            snapTurnProvider.turnAmount = XRSettingsManager.Instance.turnAmount();
            moveProvider.moveSpeed = XRSettingsManager.Instance.moveSpeed();

            switch (XRSettingsManager.Instance.mode())
            {
                case "A": // Standing Play 
                    moveProvider.enableStrafe = true;
                    break;
                case "B": // Seated Play
                    moveProvider.enableStrafe = true;
                    break;
                case "C": // Motion Sensibility
                    moveProvider.enableStrafe = false;
                    break;
            }
        }
        else
        {
            Debug.Log("No XRSettingsManager was found. XR Rig will use default settings");
        }
    }

    private void UpdateHeadSetHeight()
    {
        autoScaler.Resize();
    }
}
