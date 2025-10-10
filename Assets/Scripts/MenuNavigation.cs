using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MenuNavigation : MonoBehaviour
{
    public static MenuNavigation Instance;

    [Header("Menu Panels")]
    public GameObject titlePanel;
    public GameObject optionsPanel;
    public GameObject controlsPanel;
    public GameObject audioPanel;
    public GameObject levelSelectPanel;
    public GameObject pausePanel;

    [Header("Canvas Root")]
    public GameObject canvasRoot;

    private bool isInGame = false;
    private static bool gameIsPaused = false;
    private CanvasGroup canvasGroup;
    private Canvas canvas;

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            if (canvasRoot != null)
            {
                DontDestroyOnLoad(canvasRoot);

                // Get or add CanvasGroup for controlling visibility
                canvasGroup = canvasRoot.GetComponent<CanvasGroup>();
                if (canvasGroup == null)
                    canvasGroup = canvasRoot.AddComponent<CanvasGroup>();

                // Get Canvas component
                canvas = canvasRoot.GetComponent<Canvas>();

                // Persist EventSystem across scenes
                var eventSystem = EventSystem.current;
                if (eventSystem != null)
                {
                    DontDestroyOnLoad(eventSystem.gameObject);
                }
            }
            else
                DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        isInGame = false;
        ShowTitle();
    }

    void Update()
    {
        // ESC pour pause/unpause en jeu (using new Input System)
        #if UNITY_EDITOR
        if (isInGame && UnityEngine.InputSystem.Keyboard.current != null && UnityEngine.InputSystem.Keyboard.current.escapeKey.wasPressedThisFrame)
        #else
        if (isInGame && InputManager.Instance != null && InputManager.Instance.PauseAction.WasPressedThisFrame())
        #endif
        {
            if (gameIsPaused)
                Resume();
            else
                Pause();
        }

        // Force l'alpha du PausePanel à chaque frame pendant la pause
        if (gameIsPaused && pausePanel != null && pausePanel.activeSelf)
        {
            var cg = pausePanel.GetComponent<CanvasGroup>();
            if (cg != null && cg.alpha < 1f)
            {
                cg.alpha = 1f;
                cg.interactable = true;
                cg.blocksRaycasts = true;
            }
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Si on charge le TitleScreen, afficher le titre
        if (scene.name == "TitleScreen")
        {
            isInGame = false;
            ShowCanvasUI();
            ShowTitle();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        // Si on charge un niveau, cacher tout
        else if (scene.name.Contains("Level"))
        {
            isInGame = true;
            HideCanvasUI();
            HideAll();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    // Navigation principale
    public void ShowTitle()
    {
        HideAll();
        if (titlePanel != null)
            titlePanel.SetActive(true);
    }

    public void ShowOptions()
    {
        HideAll();
        if (optionsPanel != null)
            optionsPanel.SetActive(true);
    }

    public void ShowControls()
    {
        HideAll();
        if (controlsPanel != null)
            controlsPanel.SetActive(true);
    }

    public void ShowAudio()
    {
        HideAll();
        if (audioPanel != null)
            audioPanel.SetActive(true);
    }

    public void ShowLevelSelect()
    {
        HideAll();
        HideCanvasUI(); // Désactive le canvas du menu
        if (levelSelectPanel != null)
        {
            SceneTransition.Instance.TransitionToScene("IntroCinematic");
        }
    }

    public void ShowPause()
    {
        HideAll();
        if (pausePanel != null)
        {
            pausePanel.SetActive(true);
            StartCoroutine(ForceShowPausePanel());
        }
    }

    private System.Collections.IEnumerator ForceShowPausePanel()
    {
        yield return null;

        var pausePanelCG = pausePanel.GetComponent<CanvasGroup>();
        if (pausePanelCG != null)
        {
            pausePanelCG.alpha = 1f;
            pausePanelCG.interactable = true;
            pausePanelCG.blocksRaycasts = true;
        }
    }

    // Pause/Resume pour le jeu
    public void Pause()
    {
        if (!isInGame) return;

        gameIsPaused = true;
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        ShowCanvasUI();
        ShowPause();
    }

    public void Resume()
    {
        if (!isInGame) return;

        HideAll();
        HideCanvasUI();
        gameIsPaused = false;
        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Back()
    {
        // Si on est en jeu, retourne au menu pause
        if (isInGame)
        {
            ShowPause();
        }
        // Sinon on est sur le titre, retourne au panel titre
        else
        {
            ShowTitle();
        }
    }

    public void BackToMainMenu()
    {
        HideAll();
        gameIsPaused = false;
        Time.timeScale = 1f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        SceneTransition.Instance.TransitionToScene("TitleScreen");
    }

    // Utilitaire pour montrer/cacher le Canvas
    private void ShowCanvasUI()
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }

        if (canvasRoot != null)
        {
            // Set all animators in menu to use unscaled time
            var animators = canvasRoot.GetComponentsInChildren<Animator>(true);
            foreach (var anim in animators)
            {
                anim.updateMode = AnimatorUpdateMode.UnscaledTime;
            }

            // Ensure GraphicRaycaster is enabled
            var raycaster = canvasRoot.GetComponent<UnityEngine.UI.GraphicRaycaster>();
            if (raycaster != null)
            {
                raycaster.enabled = true;
            }
        }
    }

    public void HideCanvasUI()
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }

    void HideAll()
    {
        if (titlePanel != null) titlePanel.SetActive(false);
        if (optionsPanel != null) optionsPanel.SetActive(false);
        if (controlsPanel != null) controlsPanel.SetActive(false);
        if (audioPanel != null) audioPanel.SetActive(false);
        if (levelSelectPanel != null) levelSelectPanel.SetActive(false);
        if (pausePanel != null) pausePanel.SetActive(false);
    }

    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
