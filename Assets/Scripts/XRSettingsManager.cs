using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class XRSettingsManager : MonoBehaviour
{
    public static event Action XRSettingsChange;
    public static event Action XRRecalibrate;
    public static event Action XRInventoryUpdate;
    public static XRSettingsManager Instance;

    private bool _continuousTurnActive = false;
    private float _moveSpeed = 1f;
    private int _turnAmount = 45;
    private string _mode;
    private bool _vignetteActive = false;
    private bool _teleportActive = false;
    private bool _inventoryActive = false;

    private void Awake()
    {
        Instance = this;

        XRSettingsChange?.Invoke();
        XRRecalibrate?.Invoke();
        XRInventoryUpdate?.Invoke();
    }


    //Yeah... I hate this too
    public void setTurn(int degree)
    {
        if (degree == 0)
        {
            _continuousTurnActive = true;
        }
        else
        {
            _continuousTurnActive = false; 
            
            if (degree == 30)
            {
                _turnAmount = 30;
            }
            else if (degree == 45)
            {
                _turnAmount = 45;
            }
            else if (degree == 60)
            {
                _turnAmount = 60;
            }
        }

        XRSettingsChange?.Invoke();
    }

    public void setMode(string mode)
    {
        _mode = mode;
        XRSettingsChange?.Invoke();
    }

    public void setSpeed(float speed)
    {
        _moveSpeed = speed;
        XRSettingsChange?.Invoke();
    }

    public void setVignette(bool vignetteValue)
    {
        _vignetteActive = vignetteValue;
        XRSettingsChange?.Invoke();
    }

    public void setTeleport(bool enableTeleport)
    {
        _teleportActive = enableTeleport;
        XRSettingsChange?.Invoke();
    }

    public int turnAmount()
    {
        return _turnAmount;
    }

    public float moveSpeed()
    {
        return _moveSpeed;
    }

    public string mode()
    {
        return _mode;
    }
    
    public bool isContinuousTurnActive()
    {
        return _continuousTurnActive;
    }
    
    public bool isVignetteActive()
    {
        return _vignetteActive;
    }

    public bool isTeleportActive()
    {

        return _teleportActive;
    }

    public void recalibrate()
    {
        XRRecalibrate?.Invoke();
    }

    public void enableBodyInventory(bool isActive)
    {
        _inventoryActive = isActive;
        XRInventoryUpdate?.Invoke();
    }

    public bool isInventoryActive()
    {
        return _inventoryActive;
    }
}
