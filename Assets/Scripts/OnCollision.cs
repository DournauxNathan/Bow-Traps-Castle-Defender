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
            if (LevelManager.Instance != null)
            {
                LevelManager.Instance.LoadSceneAsync("Level");
            }
            else
            {
                Debug.LogWarning("LevelManager was not found");
            }
        }
    }
}
