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
        HideAll();
        titlePanel.SetActive(true);
    }

    public void ShowOptions()
    {
        HideAll();
        optionsPanel.SetActive(true);
    }

    public void ShowControls()
    {
        HideAll();
        controlsPanel.SetActive(true);
    }

    public void ShowAudio()
    {
        HideAll();
        audioPanel.SetActive(true);
    }

    public void ShowLevelSelect()
    {
        HideAll();
        levelSelectPanel.SetActive(true);
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