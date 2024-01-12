using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
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

    private void Awake()
    {
        XRSettingsManager.XRSettingsChange += UpdateXRSettings;
        XRSettingsManager.XRRecalibrate += UpdateHeadSetHeight;
    }

    private void Start()
    {
        UpdateXRSettings();
    }

    private void OnDestroy()
    {
        XRSettingsManager.XRSettingsChange -= UpdateXRSettings;
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
