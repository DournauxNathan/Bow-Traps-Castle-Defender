using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Item : MonoBehaviour
{
    [Header("REFS")]
    public Shop shopManager;
    public XRSimpleInteractable XRSimple;
    public XRGrabInteractable XRGrab;
    private Rigidbody m_Rigidbody;
    private MeshCollider m_Collider;

    [Header("PROPERTIES")]
    public Type type;
    public int iD;
    public new string name;
    public int value;
    public bool isSold = false;
    public int hitCount;

    public enum Type
    {
        Weapon,
        Consumable,
        Upgrade
    }

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Collider = GetComponent<MeshCollider>();
        /*XRSimple.colliders[0] = m_Collider;
        XRGrab.colliders[0] = m_Collider;*/
    }

    public void Purchase()
    {
        shopManager.BuyItem(iD);
    }

    public void Sold()
    {
        isSold = true;
        transform.parent = null;
        SetPhysics(true);

        XRSimple.enabled = false;
        XRGrab.enabled = true;
    }

    public void SetPhysics(bool usePhysics)
    {
        m_Rigidbody.useGravity = usePhysics;
        m_Rigidbody.isKinematic = !usePhysics;
        m_Collider.isTrigger = !usePhysics;
    }
}
