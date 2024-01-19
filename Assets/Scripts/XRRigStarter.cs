using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class XRRigStarter : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.XRRig.position = this.transform.position;
    }
}
