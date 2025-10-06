using UnityEngine;
using UnityEngine.InputSystem;

[DisallowMultipleComponent]
public class PauseMenu : MonoBehaviour
{

    public GameObject backToTheLobby;


    public static bool gameIsPaused = false;

    [Header("UI")]
    [SerializeField] private GameObject pauseMenuUI; // Canvas/root du menu

    [Header("Gameplay à désactiver en pause")]
    // Glisse ici tes scripts de gameplay (mouvement, look, tir, etc.)
    [SerializeField] private Behaviour[] gameplayComponents;

    [Header("Optionnel audio global")]
    [SerializeField] private bool pauseAudio = true; // mute global en pause (UI: AudioSource.ignoreListenerPause = true)

    private CanvasGroup cg;

    void Awake()
    {
        if (!pauseMenuUI)
        {
            Debug.LogError("[PauseMenu] 'pauseMenuUI' n'est pas assigné.");
            enabled = false;
            return;
        }

        // On garde l'objet actif et on pilote via CanvasGroup (pas de SetActive en pause)
        pauseMenuUI.SetActive(true);
        cg = pauseMenuUI.GetComponent<CanvasGroup>();
        if (!cg) cg = pauseMenuUI.AddComponent<CanvasGroup>();
        HideUIImmediate();

        // État propre au démarrage
        Time.timeScale = 1f;
        gameIsPaused = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        SetGameplayEnabled(true);
        if (pauseAudio) AudioListener.pause = false;
    }

    void Update()
    {
        // Échap ouvre/ferme toujours le menu
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
            TogglePause();
    }

    public void TogglePause()
    {
        if (gameIsPaused) Resume();
        else Pause();
    }

    public void Pause()
    {
        ShowUIImmediate();
        Time.timeScale = 0f;
        gameIsPaused = true;

        SetGameplayEnabled(false);       // coupe les contrôles gameplay
        Cursor.visible = true;           // souris pour l'UI
        Cursor.lockState = CursorLockMode.None;

        if (pauseAudio) AudioListener.pause = true;
    }

    public void Resume()
    {
        HideUIImmediate();
        Time.timeScale = 1f;
        gameIsPaused = false;

        SetGameplayEnabled(true);        // réactive les contrôles gameplay
        Cursor.visible = false;          // ajuste selon ton jeu
        Cursor.lockState = CursorLockMode.Locked;

        if (pauseAudio) AudioListener.pause = false;
    }

    private void ShowUIImmediate()
    {
        cg.alpha = 1f;
        cg.blocksRaycasts = true;
        cg.interactable = true;
    }

    private void HideUIImmediate()
    {
        cg.alpha = 0f;
        cg.blocksRaycasts = false;
        cg.interactable = false;
    }

    private void SetGameplayEnabled(bool enabled)
    {
        if (gameplayComponents == null) return;
        foreach (var c in gameplayComponents)
            if (c != null) c.enabled = enabled;
    }

    public void BackToTheLobby()
    {
        Debug.Log("BackToTheLobby called");
        if (backToTheLobby != null)
        {
            SceneTransition.Instance.TransitionToScene("TitleScreen");
            HideUIImmediate();
            Time.timeScale = 1f;
            gameIsPaused = false;
        }
        else
            Debug.LogError("BackToTheLobby is null!");
    }


}
