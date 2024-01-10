using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class XRSettingsManager : MonoBehaviour
{
    public static event Action XRSettingsChange;
    public static event Action XRRecalibrate;
    public static XRSettingsManager Instance;

    private bool _continuousTurnActive = false;
    private float _moveSpeed = 1f;
    private int _turnAmount = 45;
    private bool _vignetteActive = false;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
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
    
    public int turnAmount()
    {
        return _turnAmount;
    }

    public float moveSpeed()
    {
        return _moveSpeed;
    }
    
    public bool isContinuousTurnActive()
    {
        return _continuousTurnActive;
    }
    
    public bool isVignetteActive()
    {
        return _vignetteActive;
    }

    public void recalibrate()
    {
        XRRecalibrate?.Invoke();
    }
}
