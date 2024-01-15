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

    public AutoScaler autoScaler;

    [Header("Providers")]
    public SnapTurnProviderBase snapTurnProvider;
    public DynamicMoveProvider moveProvider;
    public TeleportationProvider teleportProvider;

    public MainMenu menuManager;
    private InputData _inputData;

    private void Awake()
    {
        // Get reference to InputData component
        _inputData = GetComponent<InputData>();

        XRSettingsManager.XRSettingsChange += UpdateXRSettings;
    }

    private void Start()
    {
        UpdateXRSettings();
        XRSettingsManager.XRRecalibrate += UpdateHeadSetHeight;
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

    private void UpdateXRSettings()
    {
        if(XRSettingsManager.Instance != null)
        {
            vignetteApplier.enabled = XRSettingsManager.Instance.isVignetteActive();
            RightControllerManager.smoothTurnEnabled = XRSettingsManager.Instance.isContinuousTurnActive();

            teleportProvider.enabled = XRSettingsManager.Instance.isTeleportActive();
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
