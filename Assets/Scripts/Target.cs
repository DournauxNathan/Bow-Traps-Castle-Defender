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

    private void LateUpdate()
    {
        if (IsActivate)
        {
            manager.IsTargetsActivated(); 
            IsActivate = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!IsActivate && collision.collider.TryGetComponent<Arrow>(out Arrow arrow))
        {
            IsActivate = true;
            manager.IsTargetsActivated();
        }
    }
}
