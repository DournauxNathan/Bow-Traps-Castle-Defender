using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Transform goToPosition;

    public void Teleport()
    {
        GameManager.Instance.XRRig.position = goToPosition.position;
        GameManager.Instance.XRRig.LookAt(goToPosition.position, transform.forward);
    }
}
