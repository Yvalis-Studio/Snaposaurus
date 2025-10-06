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
        all.AddRange(new[]{pausePanel, optionsPanel, controlsPanel, audioPanel});
        Show(pausePanel, clear:true);
    }

    public void ShowPause()     => Show(pausePanel, clear:true);
    public void ShowOptions()  => Show(optionsPanel);
    public void ShowControls() => Show(controlsPanel);
    public void ShowAudio()    => Show(audioPanel);
    public void Back()
    {
        if (stack.Count <= 1) { ShowPause(); return; }
        var current = stack.Pop();
        current.SetActive(false);
        var prev = stack.Peek();
        prev.SetActive(true);
        Focus(prev);
    }

    void Show(GameObject target, bool clear=false)
    {
        foreach (var p in all) if (p) p.SetActive(false);
        if (clear) stack.Clear();
        if (!target) return;
        stack.Push(target);
        target.SetActive(true);
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