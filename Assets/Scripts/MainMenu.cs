using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public string startButtonSceneName;
    
    public Button startButton;
    public Button optionsButton;
    public Button quitButton;
    public GameObject mainMenu;
    public GameObject vrSettingsPanel;

    private void Awake()
    {
        startButton.onClick.AddListener(StartGame);
        optionsButton.onClick.AddListener(OpenSettings);
        quitButton.onClick.AddListener(QuitGame);
    }

    private void OnDestroy()
    {
        startButton.onClick.RemoveListener(StartGame);
        optionsButton.onClick.RemoveListener(OpenSettings);
        quitButton.onClick.RemoveListener(QuitGame);
    }

    private void StartGame()
    {
        mainMenu.SetActive(false);
        LevelManager.Instance.LoadSceneAsync(startButtonSceneName);
    }

    public void QuitGame()
    {
        // Quit the application (Note: This does not work in the Editor)
        Application.Quit();
    }


    public void OpenSettings()
    {
        // Show VR settings panel and hide main menu panel
        mainMenu.SetActive(false);
        vrSettingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        if (LevelManager.Instance.GetCurrentScene() == 0)
        {
            // Hide VR settings panel and show main menu panel
            vrSettingsPanel.SetActive(false);
            mainMenu.SetActive(true);
        }
        else
        {
            LevelManager.Instance.LoadSceneAsync("MainMennu");
            vrSettingsPanel.SetActive(false);
        }
    }
}