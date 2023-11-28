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

    // Update is called once per frame
    void Update()
    {
        _currentHMDPosition = HMD.transform.position;
        _currentHMDRotation = HMD.transform.rotation;

        

        foreach (var bodySocket in bodySockets)
        {
            UpdateBodySocketHeight(bodySocket);
        }
        UpdateSocketinventory();
    }

    public void SmoothCameraFollow()
    {
        float interpolation = speed * Time.deltaTime;

        Vector3 position = HMD.transform.position;
        position.y = Mathf.Lerp(HMD.transform.position.y, secondCamera.transform.position.y, interpolation);
        position.x = Mathf.Lerp(HMD.transform.position.x, secondCamera.transform.position.x, interpolation);

        transform.position = position;

        transform.rotation = Quaternion.Slerp(transform.rotation, secondCamera.transform.rotation, interpolation);
    }

    private void UpdateBodySocketHeight(BodySocket bodySocket)
    {
        bodySocket.gameObject.transform.position = new Vector3(bodySocket.gameObject.transform.position.x, 
            _currentHMDPosition.y * bodySocket.heightRatio, 
            bodySocket.gameObject.transform.position.z);
    }
    private void UpdateSocketinventory()
    {
        transform.position = new Vector3(_currentHMDPosition.x, 0f, _currentHMDPosition.z);
        transform.rotation = new Quaternion(transform.rotation.x, _currentHMDRotation.y, transform.rotation.z, _currentHMDRotation.w);
    }
}