using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoScaler : MonoBehaviour
{
    public Camera mainCam;
    public float defaultHeight = 1.75f;
    
    public  void Resize()
    {
        float headHeight = mainCam.transform.localPosition.y;

        // Check if headHeight is not zero to avoid division by zero
        if (Mathf.Approximately(headHeight, 0f))
        {
            Debug.LogWarning("Head height is zero. Cannot perform scaling.");
            return;
        }

        float scale = defaultHeight / headHeight;
        
        transform.localScale = Vector3.one * scale;
    }
}
