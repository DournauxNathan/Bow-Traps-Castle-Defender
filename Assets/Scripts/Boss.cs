using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [Header("PROPERTIES")]
    public CritterType type; 
    public int health = 1; 
    public int maxHealth = 1;
    private int phase;

    public List<Weakness> weaknesses;

    public Animator m_Animator;
    internal WaveManager waveManager;

    public void Init(BossData data)
    {
        this.type = CritterType.Boss;
        this.maxHealth = data.health;
        this.health = maxHealth;
        this.phase = data.phase;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (phase)
        {
            case 1:
                break;
            case 2:
                StartNewWave();
                break;
        }
    }

    internal void UpdateWeaknessCount()
    {
        
    }

    private void StartNewWave()
    {
        foreach (Weakness weakness in weaknesses)
        {
            weakness.isEnable = false;
        }

        waveManager.StartWave();

        //Call Wave Manager
    }

    internal void UpdatePhase(int phase)
    {
        this.phase = phase;
    }
}
