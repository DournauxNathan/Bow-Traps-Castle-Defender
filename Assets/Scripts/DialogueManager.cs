using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public GameObject infoText;

    public string[] dialogues;
    private int currentDialogueIndex = 0;

    public float buttonCooldown = 1.0f; // Adjust the cooldown time as needed
    private float lastButtonPressTime = 0.0f;

    private InputData _inputData;


    void Start()
    {
        _inputData = GetComponent<InputData>();
        ShowDialogue();
    }

    private void Update()
    {
        if (Time.time - lastButtonPressTime >= buttonCooldown)
        {
            if (_inputData._rightController.TryGetFeatureValue(CommonUsages.primaryButton, out bool Abutton))
            {
                if (Abutton)
                {
                    ContinueDialogue();
                    lastButtonPressTime = Time.time; // Update the last button press time
                }
                Debug.Log("A button: " + Abutton);
            }
        }
    }

    void ShowDialogue()
    {
        dialogueText.text = dialogues[currentDialogueIndex];
    }

    public void ContinueDialogue()
    {
        currentDialogueIndex++;

        if (currentDialogueIndex < dialogues.Length)
        {
            ShowDialogue();
        }
        else
        {
            // End of dialogue, close UI or perform other actions.
            Debug.Log("End of dialogue");
        }
    }
}
