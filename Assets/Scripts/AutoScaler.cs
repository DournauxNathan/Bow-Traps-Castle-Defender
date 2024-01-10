using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoScaler : MonoBehaviour
{
    public static event Action Recalibrate;

    public Camera mainCam;
    public float defaultHeight = 1.75f;
    
    public  void Resize()
    {
        float headHeight = mainCam.transform.localPosition.y;
        float scale = defaultHeight / headHeight;
        
        transform.localScale = Vector3.one * scale;
    }
}
