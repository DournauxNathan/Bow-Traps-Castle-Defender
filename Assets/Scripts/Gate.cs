using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    private Animator m_Animator;

    // Start is called before the first frame update
    void Start()
    {
        m_Animator = GetComponent<Animator>();        
    }

    public void Open()
    {
        m_Animator.SetTrigger("Open");
    }

    public void Close()
    {
        m_Animator.SetTrigger("Close");
    }
}
