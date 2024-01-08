using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR    
[CustomEditor(typeof(TrapData), true)] // The 'true' parameter enables inspecting children
public class TrapEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        TrapData trap = target as TrapData;

        if (GUILayout.Button("Activate Trap"))
        {
            trap.Activate();
        }

        if (GUILayout.Button("Toggle Trap"))
        {
            trap.Toggle();
        }
    }
}
#endif
