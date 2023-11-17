using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTrap : TrapData
{
    public Activator activator;

    public override bool CanActivate()
    {
        return !IsActive;
    }

    // Draw a line between the trap and its activator in the Scene view
    private void OnDrawGizmos()
    {
        if (activator != null)
        {
            // Draw the first line segment
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, activator.transform.position);
        }
    }
}
