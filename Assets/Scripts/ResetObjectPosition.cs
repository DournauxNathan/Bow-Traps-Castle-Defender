using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetObjectPosition : MonoBehaviour
{
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Rigidbody m_Rigidbody;

    void Start()
    {
        // Save the initial position of the object
        initialPosition = transform.position;
        initialRotation = transform.rotation;

        TryGetComponent<Rigidbody>(out m_Rigidbody);
    }

    void FixedUpdate()
    {
        // Check if the object is far from the player
        if (m_Rigidbody.linearVelocity.y < -10)
        {
            // If it's far, return it to the initial position
            ReturnToInitialPosition();
        }
    }

    void ReturnToInitialPosition()
    {
        // Move the object back to its initial position
        transform.position = initialPosition;
        transform.rotation = initialRotation;

        m_Rigidbody.linearVelocity = Vector3.zero;
    }
}
