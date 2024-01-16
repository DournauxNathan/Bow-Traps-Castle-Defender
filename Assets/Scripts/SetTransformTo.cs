using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTransformTo : MonoBehaviour
{
    private void Awake()
    {
        GameManager.Instance.XRRig.position = this.transform.position;
    }
}
