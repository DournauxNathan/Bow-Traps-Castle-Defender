using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class XRRigStarter : MonoBehaviour
{
    private void Awake()
    {
#if UNITY_EDITOR
        SceneManager.LoadSceneAsync("AutoLoad", LoadSceneMode.Additive);
#endif
        GameManager.Instance.XRRig.position = this.transform.position;
    }
}
