using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class XRSettingsListener : MonoBehaviour
{
    public VignetteApplier vignetteApplier;
    public ActionBasedControllerManager controllerManager;

    public AutoScaler autoScaler;

    [Header("Providers")]
    public SnapTurnProviderBase snapTurnProvider;
    public DynamicMoveProvider moveProvider;

    private void Awake()
    {
        XRSettingsManager.XRSettingsChange += UpdateXRSettings;
        XRSettingsManager.XRRecalibrate += UpdateHeadSetHeight;
    }

    private void Start()
    {
        UpdateXRSettings();
        //UpdateHeadSetHeight();
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
            controllerManager.smoothTurnEnabled = XRSettingsManager.Instance.isContinuousTurnActive();

            snapTurnProvider.turnAmount = XRSettingsManager.Instance.turnAmount();
            moveProvider.moveSpeed = XRSettingsManager.Instance.moveSpeed();
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
