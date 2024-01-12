using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [Header("UI Components")]
    public TextMeshProUGUI dialogueText;
    public GameObject infoText;

    [Header("Dialogue Data")]
    public DialogueData dialogue;
    private int currentDialogueIndex = 0;

    [Header("Button Cooldown")]
    public float buttonCooldown = 1.0f;
    private float lastButtonPressTime = 0.0f;

    [Header("Activation Delay")]
    public float activationDelay = 2.0f;

    public UnityEvent onDialogueEnd;

    private InputData _inputData;

    void Start()
    {
        // Get reference to InputData component
        _inputData = GetComponent<InputData>();

        // Show initial dialogue
        ShowDialogue();

        // Activate info text after a delay
        StartCoroutine(ActivateInfoTextAfterDelay());
    }

    private void FixedUpdate()
    {
        // Check button press with cooldown
        CheckButtonPress();
    }

    #region Dialogue Handling

    void ShowDialogue()
    {
        // Display the current dialogue line
        dialogueText.text = dialogue.dialogueLines[currentDialogueIndex];
    }

    public void ContinueDialogue()
    {
        // Move to the next dialogue line
        currentDialogueIndex++;
        ShowDialogue();
        
    }
    #endregion

    #region Button Press Handling

    void CheckButtonPress()
    {
        // Check button press with cooldown
        if (Time.time - lastButtonPressTime >= buttonCooldown)
        {
            if (_inputData._rightController.TryGetFeatureValue(CommonUsages.primaryButton, out bool Abutton))
            {
                if (Abutton && currentDialogueIndex < dialogue.dialogueLines.Length)
                {
                    // If A button is pressed, continue dialogue and activate info text
                    ContinueDialogue();
                    infoText.SetActive(false);
                    lastButtonPressTime = Time.time; // Update the last button press time
                    StartCoroutine(ActivateInfoTextAfterDelay());
                }
                else if (currentDialogueIndex == dialogue.dialogueLines.Length - 1)
                {
                    infoText.SetActive(false);
                    onDialogueEnd?.Invoke();
                }
            }
        }
    }
    #endregion

    #region Activation Delay Handling

    IEnumerator ActivateInfoTextAfterDelay()
    {
        // Coroutine to activate info text after a delay
        yield return new WaitForSeconds(activationDelay);
        infoText.SetActive(true);
    }

    #endregion

    public void LoadNewDialogue(DialogueData data)
    {
        if (this.dialogue != data)
        {
            currentDialogueIndex = 0;
        
            this.dialogue = data;

            ShowDialogue(); 
            StartCoroutine(ActivateInfoTextAfterDelay());
        }
    }
}
