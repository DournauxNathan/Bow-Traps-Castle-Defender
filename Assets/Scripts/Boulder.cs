using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boulder : MonoBehaviour
{
    public BoulderTrap boulderManager;

    public Transform stockTransform;
    private Vector3 startPosition;

    private AudioSource m_AudioSource;
    private Rigidbody m_Rigibody;

    // Start is called before the first frame update
    void Start()
    {
        boulderManager.Subscribe(this);

        startPosition = transform.localPosition;

        m_AudioSource = GetComponent<AudioSource>();
        m_Rigibody = GetComponent<Rigidbody>();
    }

    public void Activate()
    {
        m_Rigibody.isKinematic = false;
        m_Rigibody.useGravity = true;

        transform.parent = null;
    }

    public void Deactivate()
    {
        ReputInStock();
    }

    public void ReputInStock()
    {
        transform.parent = stockTransform;

        transform.localPosition = startPosition;

        m_Rigibody.isKinematic = true;
        m_Rigibody.useGravity = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Detector"))
        {
            Deactivate();
        }

        if (other.TryGetComponent<Critter>(out Critter _critter))
        {
            Destroy(_critter.gameObject);
            _critter.TakeDamage(-999);
        }

    }
}
