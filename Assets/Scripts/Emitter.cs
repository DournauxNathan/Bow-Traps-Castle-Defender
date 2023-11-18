using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Emitter : MonoBehaviour
{
    public int damage { get; set; }
    public bool isActive { get; set; }

    public BoxCollider m_Collider;
    public ParticleSystem m_particles;
    public AudioSource m_audioSource;
    public AudioClip onActivate, onStay, onDeactivate;

    private void Start()
    {
        m_Collider.enabled = false;
        isActive = false;
    }

    public void Activate()
    {
        isActive = true;
        m_Collider.enabled = true;
        m_particles?.Play();
        m_audioSource?.PlayOneShot(onActivate);
        
        SwitchAudio(true);
    }

    public void Deactivate()
    {
        isActive = false;
        SwitchAudio(false);
        
        m_Collider.enabled = false;
        m_particles?.Stop();
        m_audioSource?.PlayOneShot(onDeactivate);
    }

    public void Toogle()
    {
        isActive = !isActive;

        if (isActive)
            Activate();
        else
            Deactivate();
    }

    public void SwitchAudio(bool _switch)
    {
        if (_switch)
        {
            m_audioSource.loop = true;
            m_audioSource.clip = onStay;
            m_audioSource?.Play();
        }
        else
        {
            m_audioSource.loop = false;
            m_audioSource.clip = null;
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Critter>(out Critter _critter))
        {
            _critter.TakeDamage(damage);
        }
    }
}
