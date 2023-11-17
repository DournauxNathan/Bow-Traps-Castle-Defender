using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Trap), true)] // The 'true' parameter enables inspecting children
public class TrapEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Trap trap = target as Trap;

        if (GUILayout.Button("Activate Trap"))
        {
            trap.Activate();
        }
    }
}
