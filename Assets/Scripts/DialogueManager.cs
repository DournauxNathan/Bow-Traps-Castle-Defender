using System.Collections;
using UnityEngine;
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

    private void Update()
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

        if (currentDialogueIndex < dialogue.dialogueLines.Length)
        {
            // If there are more dialogues, show the next one
            ShowDialogue();
        }
        else
        {
            // End of dialogue, close UI or perform other actions.
            Debug.Log("End of dialogue");
        }
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
                if (Abutton)
                {
                    // If A button is pressed, continue dialogue and activate info text
                    ContinueDialogue();
                    infoText.SetActive(false);
                    lastButtonPressTime = Time.time; // Update the last button press time
                    StartCoroutine(ActivateInfoTextAfterDelay());
                }
                Debug.Log("A button: " + Abutton);
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
}
