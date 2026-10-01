using UnityEngine;
using UnityEngine.SceneManagement;

public class mainmenu : MonoBehaviour
{
    [Header("UI Canvas Folders")]
    public GameObject mainButtonsPanel;   // Drag 'mainmenu' folder here in Unity
    public GameObject settingsPanel;      // Drag your Settings panel here
    public GameObject creditsPanel;       // Drag your Credits panel here

    // 1. PLAY BUTTON
    public void PlayGame()
    {
        SceneManager.LoadScene("game");
    }

    // 2. SETTINGS BUTTON (Turns off menu buttons, turns on Settings box)
    public void OpenSettings()
    {
        if (mainButtonsPanel != null) mainButtonsPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(true);
        if (creditsPanel != null) creditsPanel.SetActive(false);
    }

    // 3. CREDITS BUTTON (Turns off menu buttons, turns on Credits box)
    public void OpenCredits()
    {
        if (mainButtonsPanel != null) mainButtonsPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (creditsPanel != null) creditsPanel.SetActive(true);
    }

    // 4. BACK BUTTON (Put this inside Settings/Credits to return to the main panel)
    public void CloseSubPanels()
    {
        if (mainButtonsPanel != null) mainButtonsPanel.SetActive(true);
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (creditsPanel != null) creditsPanel.SetActive(false);
    }

    // 5. QUIT BUTTON
    public void QuitGame()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEngine.Object.FindAnyObjectByType<mainmenu>().StopPlayModeInEditor();
        #endif
    }

    private void StopPlayModeInEditor()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
