using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DialogueData))]
public class DialogueDataEditor : Editor
{
    private SerializedProperty idProp;
    private SerializedProperty dialogueLinesProp;

    private void OnEnable()
    {
        idProp = serializedObject.FindProperty("id");
        dialogueLinesProp = serializedObject.FindProperty("dialogueLines");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(idProp);
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Dialogue Lines", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;

        for (int i = 0; i < dialogueLinesProp.arraySize; i++)
        {
            SerializedProperty dialogueLine = dialogueLinesProp.GetArrayElementAtIndex(i);
            dialogueLine.stringValue = EditorGUILayout.TextArea(dialogueLine.stringValue, GUILayout.Height(EditorGUIUtility.singleLineHeight * 5));

            GUILayout.BeginHorizontal();
            GUILayout.Space(EditorGUIUtility.labelWidth);

            // Button to remove the dialogue line
            if (GUILayout.Button("Remove", GUILayout.Width(60f)))
            {
                dialogueLinesProp.DeleteArrayElementAtIndex(i);
                serializedObject.ApplyModifiedProperties();
                break;
            }

            GUILayout.EndHorizontal();

            GUILayout.Space(10f);
        }

        EditorGUI.indentLevel--;

        // Button to add a new dialogue line
        if (GUILayout.Button("Add Dialogue Line"))
        {
            dialogueLinesProp.arraySize++;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
