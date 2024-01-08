using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue")]
public class DialogueData : ScriptableObject
{
    public int id;
    public string[] dialogueLines;
}
