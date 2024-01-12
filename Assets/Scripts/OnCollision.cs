using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class OnCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent<XROrigin>(out XROrigin rig))
        {
            LevelManager.Instance.LoadSceneAsync("Level");
        }
    }
}
