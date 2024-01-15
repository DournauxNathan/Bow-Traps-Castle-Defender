using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BodySocket
{
    public GameObject gameObject;
    [Range(0, 1)]
    public float heightRatio;
}

public class BodySocketInventory : MonoBehaviour
{
    public GameObject HMD;
    public GameObject secondCamera;
    public float speed;
    public List<BodySocket> bodySockets;

    private Vector3 _currentHMDPosition;
    private Quaternion _currentHMDRotation;
        
    public void UpdateWithHeigthCalibration()
    {
        Debug.Log("");
        foreach (var bodySocket in bodySockets)
        {
            UpdateBodySocketHeight(bodySocket);
        }
        UpdateSocketinventory();
    }


    // Update is called once per frame
    void Update()
    {
        _currentHMDPosition = HMD.transform.localPosition;
        _currentHMDRotation = HMD.transform.localRotation;

        
        foreach (var bodySocket in bodySockets)
        {
            UpdateBodySocketHeight(bodySocket);
        }
        UpdateSocketinventory();
    }

    public void SmoothCameraFollow()
    {
        float interpolation = speed * Time.deltaTime;

        Vector3 position = HMD.transform.localPosition;
        position.y = Mathf.Lerp(HMD.transform.localPosition.y, secondCamera.transform.localPosition.y, interpolation);
        position.x = Mathf.Lerp(HMD.transform.localPosition.x, secondCamera.transform.localPosition.x, interpolation);

        transform.localPosition = position;

        transform.localRotation = Quaternion.Slerp(transform.localRotation, secondCamera.transform.localRotation, interpolation);
    }

    private void UpdateBodySocketHeight(BodySocket bodySocket)
    {
        bodySocket.gameObject.transform.localPosition = new Vector3(bodySocket.gameObject.transform.localPosition.x, 
            _currentHMDPosition.y * bodySocket.heightRatio, 
            bodySocket.gameObject.transform.localPosition.z);
    }
    private void UpdateSocketinventory()
    {
        transform.localPosition = new Vector3(_currentHMDPosition.x, 0f, _currentHMDPosition.z);
        transform.localRotation = new Quaternion(transform.localRotation.x, _currentHMDRotation.y, transform.localRotation.z, _currentHMDRotation.w);
    }
}