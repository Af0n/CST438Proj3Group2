using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {
    public GameObject menu;
    public GameObject settingsMenu;
    public GameObject recipeBook;
    public bool isActive = false;
    public bool isSettingsActive = false;
    public bool isRecipeBookActive = false;

    public void togglePause() {
        isActive = !isActive;
        menu.SetActive(isActive);
        Time.timeScale = isActive ? 0 : 1;
    }

    public void toMainMenu () {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void toggleSettings() {
        isSettingsActive = !isSettingsActive;
        settingsMenu.SetActive(isSettingsActive);
    }

    public void toggleRecipeBook()
    {
        if (!isRecipeBookActive)
        {
            isRecipeBookActive = !isRecipeBookActive;
            recipeBook.SetActive(isRecipeBookActive);
        }    
    }

    public void closeRecipeBook()
    {
        isRecipeBookActive = !isRecipeBookActive;
        recipeBook.SetActive(isRecipeBookActive);
    }
}