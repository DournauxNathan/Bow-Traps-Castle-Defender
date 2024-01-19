using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public TargetManager manager;

    public bool IsActivate = false;

    private Rigidbody m_Rigidbody;

    public bool debug = false;

    private void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        manager.SubscribeTarget(this);
    }

    private void LateUpdate()
    {
        if (debug && !IsActivate)
        {
            manager.IsTargetsActivated(this); 
            IsActivate = true;
            debug = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!IsActivate && collision.collider.TryGetComponent<Arrow>(out Arrow arrow))
        {
            IsActivate = true;
            manager.IsTargetsActivated(this);
        }
    }
}
