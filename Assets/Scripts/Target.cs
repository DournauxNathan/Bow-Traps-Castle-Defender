using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public TargetManager manager;

    public bool IsActivate = false;

    private Rigidbody m_Rigidbody;

    private void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        manager.SubscribeTarget(this);
    }

    public void Rotate(Vector3 force)
    {
        m_Rigidbody.AddTorque(Vector3.up + force);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent<Arrow>(out Arrow arrow))
        {
            IsActivate = true;
            Rotate(arrow.m_Rigidbody.velocity);

            manager.IsTargetsActivated();
        }
    }
}
