using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject settingsMenu;
    public string startingScene;

    // String ref to a target dev scene
    public string devScene;

    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AudioManager.Instance.getAudioModifiers();
        AudioManager.Instance.PlayMusic("MainMenu");
    }

    public void StartGame()
    {
        SceneManager.LoadScene(startingScene);
    }

    // Enters the aformentioned dev scene in the listed on the GameObject
    public void LoadDevScene()
    {
        SceneManager.LoadScene(devScene);
    }

    public void ActivateSettingsMenu()
    {
        if (settingsMenu == null)
        {   
            return;
        }
        if(settingsMenu.activeSelf) {
            settingsMenu.SetActive(false);
        } else {
            settingsMenu.SetActive(true);
        }
        
    }

    public void ExitGame()
    {
        Debug.Log("Exiting the game...");
        Application.Quit();
    }
    
    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
