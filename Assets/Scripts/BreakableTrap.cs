using System;
using System.Collections;
using UnityEngine;

public class BreakableTrap : TrapData
{
    public BreakableActivator breakActivator;

    public override bool CanActivate()
    {
        return !IsActive && !breakActivator.IsBroken;
    }

    // Draw a line between the trap and its activator in the Scene view
    private void OnDrawGizmos()
    {
        if (breakActivator != null)
        {
            // Draw the first line segment
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, breakActivator.transform.position + new Vector3(0f,1f,0f));
            Gizmos.DrawWireSphere(breakActivator.transform.position + new Vector3(0f, 1f, 0f), .15f);

        }
    }
}
