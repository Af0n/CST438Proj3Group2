using UnityEngine;

public class PauseMenu : MonoBehaviour {
    public GameObject menu;
    public bool isActive = false;
    public void togglePause() {
        isActive = !isActive;
        menu.SetActive(isActive);
        Time.timeScale = isActive ? 0 : 1;
    }
}