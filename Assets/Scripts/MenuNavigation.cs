using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuNavigation : MonoBehaviour
{
    [Header("Menu Panels")]
    public GameObject titlePanel;
    public GameObject optionsPanel;
    public GameObject controlsPanel;
    public GameObject audioPanel;
    public GameObject levelSelectPanel;

    void Start()
    {
        ShowTitle();
    }

    // Navigation principale
    public void ShowTitle()
    {
        Debug.Log("ShowTitle called");
        HideAll();
        if (titlePanel != null)
            titlePanel.SetActive(true);
        else
            Debug.LogError("titlePanel is null!");
    }

    public void ShowOptions()
    {
        Debug.Log("ShowOptions called");
        HideAll();
        if (optionsPanel != null)
            optionsPanel.SetActive(true);
        else
            Debug.LogError("optionsPanel is null!");
    }

    public void ShowControls()
    {
        Debug.Log("ShowControls called");
        HideAll();
        if (controlsPanel != null)
            controlsPanel.SetActive(true);
        else
            Debug.LogError("controlsPanel is null!");
    }

    public void ShowAudio()
    {
        Debug.Log("ShowAudio called");
        HideAll();
        if (audioPanel != null)
            audioPanel.SetActive(true);
        else
            Debug.LogError("audioPanel is null!");
    }

    public void ShowLevelSelect()
    {
        Debug.Log("ShowLevelSelect called");
        HideAll();
        if (levelSelectPanel != null)
            levelSelectPanel.SetActive(true);
        else
            Debug.LogError("levelSelectPanel is null!");
    }

    // Utilitaire
    void HideAll()
    {
        titlePanel.SetActive(false);
        optionsPanel.SetActive(false);
        controlsPanel.SetActive(false);
        audioPanel.SetActive(false);
        levelSelectPanel.SetActive(false);
    }

    // Charger un niveau
    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}