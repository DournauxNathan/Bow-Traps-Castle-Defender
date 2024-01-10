using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [Header("Screen Shake")]
    public Button shakeOff;
    public Button shakeOn;

    [Header("Vignette")]
    public Button vignetteOff;
    public Button vignetteOn;

    [Header("Volume")]
    public Slider musicSlider;
    public Slider sfxSlider;

    [Header("Mode")]
    public Button previous;
    public Text currentMode;
    public Button next;
    public Text info;
    public List<MovementMode> modes;

    [System.Serializable]
    public class MovementMode
    {
        public string id;
        public string infoText;
    }

    [Header("Speed")]
    public Button slow;
    public Button fast;

    [Header("Snap Turning")]
    public Button SnapOff;
    [Tooltip("30°")] public Button thirtyDegree;
    [Tooltip("45°")] public Button fortyFiveDegree;
    [Tooltip("60°")] public Button sixtyDegree;

    [Header("Height")]
    public Button recalibrate;

    private void Awake()
    {
        musicSlider.onValueChanged.AddListener(MusicVolumeChange);
        sfxSlider.onValueChanged.AddListener(SFXVolumeChange);

        shakeOff.onClick.AddListener(() => ToggleScreenShake(false));
        shakeOn.onClick.AddListener(() => ToggleScreenShake(true));

        vignetteOff.onClick.AddListener(() => ToggleVignette(false));
        vignetteOn.onClick.AddListener(() => ToggleVignette(true));

        previous.onClick.AddListener(() => UpdateControlMode(-1));
        next.onClick.AddListener(() => UpdateControlMode(1));
        currentMode.text = modes[0].id;
        info.text = modes[0].infoText;

        slow.onClick.AddListener(() => SetSpeed(1f));
        fast.onClick.AddListener(() => SetSpeed(2f));

        SnapOff.onClick.AddListener(() => SnapTurning(0));
        thirtyDegree.onClick.AddListener(() => SnapTurning(30));
        fortyFiveDegree.onClick.AddListener(() => SnapTurning(45));
        sixtyDegree.onClick.AddListener(() => SnapTurning(60));

        recalibrate.onClick.AddListener(Recalibrate);
    }


    private void OnDestroy()
    {
        musicSlider.onValueChanged.RemoveListener(MusicVolumeChange);
        sfxSlider.onValueChanged.RemoveListener(SFXVolumeChange);
    }

    private void MasterVolumeChange(float volume)
    {
        SoundManager.Instance.SetMasterVolume(volume);
    }

    private void MusicVolumeChange(float volume)
    {
        SoundManager.Instance.SetMusicVolume(volume);
    }

    private void SFXVolumeChange(float volume)
    {
        SoundManager.Instance.SetSFXVolume(volume);
    }

    private void ToggleScreenShake(bool toggle)
    {

    }

    private void ToggleVignette(bool toggle)
    {
        XRSettingsManager.Instance.setVignette(toggle);
    }

    private int index = 0;
    private void UpdateControlMode(int direction)
    {        
        // Use modulo to loop the index
        index = (index + direction + modes.Count) % modes.Count;

        // Display information based on the current index
        currentMode.text = modes[index].id;
        info.text = modes[index].infoText;
    }

    private void SetSpeed(float speed)
    {
        XRSettingsManager.Instance.setSpeed(speed);
    }

    private void SnapTurning(int degree)
    {
        XRSettingsManager.Instance.setTurn(degree);
    }

    private void Recalibrate()
    {
        XRSettingsManager.Instance.recalibrate();
    }
}
