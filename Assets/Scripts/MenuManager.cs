using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    #region Singleton

    private static MenuManager _instance;

    public static MenuManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<MenuManager>();

                if (_instance == null)
                {
                    GameObject go = new GameObject("MenuManager");
                    _instance = go.AddComponent<MenuManager>();
                    DontDestroyOnLoad(go);
                }
            }
            return _instance;
        }
    }

    #endregion

    #region Main Menu Variables

    public GameObject mainMenuPanel;

    #endregion

    #region VR Settings Variables

    public GameObject vrSettingsPanel;

    #endregion

    #region Main Menu Functions

    public void PlayGame()
    {
        mainMenuPanel.SetActive(false);
        // Load the game scene
        SceneManager.LoadScene("GameScene");
    }

    public void QuitGame()
    {
        // Quit the application (Note: This does not work in the Editor)
        Application.Quit();
    }

    #endregion

    #region VR Settings Functions

    public void OpenVRSettings()
    {
        // Show VR settings panel and hide main menu panel
        mainMenuPanel.SetActive(false);
        vrSettingsPanel.SetActive(true);
    }

    public void CloseVRSettings()
    {
        // Hide VR settings panel and show main menu panel
        mainMenuPanel.SetActive(true);
        vrSettingsPanel.SetActive(false);
    }

    #endregion

    #region Back Buttons

    public void BackToMainMenu()
    {
        // Show main menu panel and hide settings and VR settings panels
        mainMenuPanel.SetActive(true);
        vrSettingsPanel.SetActive(false);
    }

    #endregion

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}