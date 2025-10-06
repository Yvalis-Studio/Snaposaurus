using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Gère à la fois la pause du jeu ET la navigation entre les panels du menu pause
/// Remplace PauseMenu.cs + PauseMenuRouter.cs
/// </summary>
public class PauseMenuManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject pauseMenuUI; // Le Canvas entier ou root du menu

    [Header("Panels")]
    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject optionsPanel;
    [SerializeField] GameObject controlsPanel;
    [SerializeField] GameObject audioPanel;

    [Header("Gameplay à désactiver en pause")]
    [SerializeField] private Behaviour[] gameplayComponents;

    [Header("Options")]
    [SerializeField] private bool pauseAudio = true;

    // État
    public static bool gameIsPaused = false;
    private Stack<GameObject> panelStack = new Stack<GameObject>();
    private List<GameObject> allPanels = new List<GameObject>();
    private CanvasGroup canvasGroup;

    void Awake()
    {
        // Vérifie que le menu UI est assigné
        if (!pauseMenuUI)
        {
            Debug.LogError("[PauseMenuManager] 'pauseMenuUI' n'est pas assigné!");
            enabled = false;
            return;
        }

        // Setup du CanvasGroup pour afficher/cacher tout le menu
        pauseMenuUI.SetActive(true);
        canvasGroup = pauseMenuUI.GetComponent<CanvasGroup>();
        if (!canvasGroup)
            canvasGroup = pauseMenuUI.AddComponent<CanvasGroup>();

        // Collecte tous les panels
        if (pausePanel != null) allPanels.Add(pausePanel);
        if (optionsPanel != null) allPanels.Add(optionsPanel);
        if (controlsPanel != null) allPanels.Add(controlsPanel);
        if (audioPanel != null) allPanels.Add(audioPanel);

        // Cache tous les panels au démarrage
        HideAllPanels();
        HideMenuUI();

        // État initial : jeu non pausé
        Time.timeScale = 1f;
        gameIsPaused = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        SetGameplayEnabled(true);
        if (pauseAudio) AudioListener.pause = false;
    }

    void Update()
    {
        // Échap pour ouvrir/fermer le menu
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (gameIsPaused)
            {
                // Si on est dans un sous-menu, on revient en arrière
                if (panelStack.Count > 1)
                    Back();
                else
                    Resume(); // Sinon on ferme le menu
            }
            else
            {
                Pause();
            }
        }
    }

    // ========== GESTION PAUSE/RESUME ==========

    public void Pause()
    {
        Debug.Log("[PauseMenuManager] Pause appelé");
        gameIsPaused = true;
        Time.timeScale = 0f;

        SetGameplayEnabled(false);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        if (pauseAudio) AudioListener.pause = true;

        // Affiche le menu UI et le panel pause
        ShowMenuUI();
        Debug.Log($"[PauseMenuManager] CanvasGroup alpha après ShowMenuUI: {canvasGroup.alpha}");
        ShowPause();
    }

    public void Resume()
    {
        HideAllPanels();
        HideMenuUI();
        panelStack.Clear();

        gameIsPaused = false;
        Time.timeScale = 1f;

        SetGameplayEnabled(true);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        if (pauseAudio) AudioListener.pause = false;
    }

    // ========== NAVIGATION ENTRE PANELS ==========

    public void ShowPause()
    {
        ShowPanel(pausePanel, clearStack: true);
    }

    public void ShowOptions()
    {
        ShowPanel(optionsPanel);
    }

    public void ShowControls()
    {
        ShowPanel(controlsPanel);
    }

    public void ShowAudio()
    {
        ShowPanel(audioPanel);
    }

    public void Back()
    {
        if (panelStack.Count <= 1)
        {
            ShowPause();
            return;
        }

        // Retire le panel actuel
        var current = panelStack.Pop();
        if (current != null)
            current.SetActive(false);

        // Affiche le panel précédent
        if (panelStack.Count > 0)
        {
            var previous = panelStack.Peek();
            if (previous != null)
                previous.SetActive(true);
        }
    }

    // ========== UTILITAIRES ==========

    private void ShowPanel(GameObject panel, bool clearStack = false)
    {
        if (panel == null)
        {
            Debug.LogWarning("[PauseMenuManager] Panel non assigné!");
            return;
        }

        Debug.Log($"[PauseMenuManager] ShowPanel: {panel.name}");

        // Cache tous les panels
        HideAllPanels();

        // Réinitialise la stack si demandé
        if (clearStack)
            panelStack.Clear();

        // Ajoute et affiche le nouveau panel
        panelStack.Push(panel);
        panel.SetActive(true);
        Debug.Log($"[PauseMenuManager] Panel {panel.name} SetActive(true), isActive: {panel.activeSelf}");
    }

    private void HideAllPanels()
    {
        foreach (var panel in allPanels)
        {
            if (panel != null)
                panel.SetActive(false);
        }
    }

    private void SetGameplayEnabled(bool enabled)
    {
        if (gameplayComponents == null) return;
        foreach (var c in gameplayComponents)
            if (c != null) c.enabled = enabled;
    }

    private void ShowMenuUI()
    {
        if (canvasGroup == null) return;
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
    }

    private void HideMenuUI()
    {
        if (canvasGroup == null) return;
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
    }

    public void BackToMainMenu()
    {
        // Réinitialise l'état du jeu sans cacher le curseur
        HideAllPanels();
        HideMenuUI();
        panelStack.Clear();

        gameIsPaused = false;
        Time.timeScale = 1f;
        SetGameplayEnabled(true);
        if (pauseAudio) AudioListener.pause = false;

        // Garde le curseur visible pour le menu titre
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // Charge la scène titre
        SceneTransition.Instance.TransitionToScene("TitleScreen");
    }
}
