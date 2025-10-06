using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseMenuRouter : MonoBehaviour
{
    [Header("Panneaux")]
    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject optionsPanel;
    [SerializeField] GameObject controlsPanel;
    [SerializeField] GameObject audioPanel;

    [Header("Focus (optionnel)")]
    [SerializeField] Selectable pauseFirst;
    [SerializeField] Selectable optionsFirst;
    [SerializeField] Selectable controlsFirst;
    [SerializeField] Selectable audioFirst;

    readonly List<GameObject> all = new();
    readonly Stack<GameObject> stack = new();

    void Awake()
    {
        Debug.Log("[PauseMenuRouter] Awake - Initialisation");
        all.AddRange(new[]{pausePanel, optionsPanel, controlsPanel, audioPanel});

        // Debug des panels assignés
        Debug.Log($"[PauseMenuRouter] pausePanel: {(pausePanel != null ? pausePanel.name : "NULL")}");
        Debug.Log($"[PauseMenuRouter] optionsPanel: {(optionsPanel != null ? optionsPanel.name : "NULL")}");
        Debug.Log($"[PauseMenuRouter] controlsPanel: {(controlsPanel != null ? controlsPanel.name : "NULL")}");
        Debug.Log($"[PauseMenuRouter] audioPanel: {(audioPanel != null ? audioPanel.name : "NULL")}");

        // Ne rien faire au Awake - le PauseMenu gère l'affichage initial
        // On initialise juste la stack vide
        stack.Clear();
    }

    void OnEnable()
    {
        // Quand le menu devient visible, on affiche le PausePanel
        ShowPause();
    }

    public void ShowPause()
    {
        Show(pausePanel, clear: true);
    }

    public void ShowOptions()
    {
        Show(optionsPanel);
    }

    public void ShowControls()
    {
        Debug.Log("[PauseMenuRouter] ShowControls appelé!");
        Debug.Log($"[PauseMenuRouter] controlsPanel null? {controlsPanel == null}");
        Show(controlsPanel);
    }

    public void ShowAudio()
    {
        Show(audioPanel);
    }

    public void Back()
    {
        if (stack.Count <= 1)
        {
            ShowPause();
            return;
        }

        var current = stack.Pop();
        current.SetActive(false);
        var prev = stack.Peek();
        prev.SetActive(true);
        Focus(prev);
    }

    void Show(GameObject target, bool clear=false)
    {
        Debug.Log($"[PauseMenuRouter] Show appelé pour: {(target != null ? target.name : "NULL")}, clear={clear}");

        foreach (var p in all) if (p) p.SetActive(false);
        if (clear) stack.Clear();
        if (!target)
        {
            Debug.LogWarning("[PauseMenuRouter] Target est NULL, impossible d'afficher!");
            return;
        }
        stack.Push(target);
        target.SetActive(true);
        Debug.Log($"[PauseMenuRouter] Panel {target.name} activé, stack count: {stack.Count}");
        Focus(target);
    }

    void Focus(GameObject panel)
    {
        Selectable first =
            panel == pausePanel     ? pauseFirst:
            panel == optionsPanel  ? optionsFirst :
            panel == controlsPanel ? controlsFirst :
            panel == audioPanel  ? audioFirst : null;

        if (first)
        {
            EventSystem.current?.SetSelectedGameObject(first.gameObject);
            first.Select();
        }
    }
}