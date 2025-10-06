using UnityEngine;
using UnityEngine.UI;

public class QTEExitButton : MonoBehaviour
{
    [Header("References")]
    public QTEManager qteManager;
    public DinosaurQTE dinosaurQTE;

    [Header("Optional")]
    public GameObject qteCanvas; // Tout le canvas à désactiver

    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OnExitClicked);
        }
    }

    void OnExitClicked()
    {
        Debug.Log("Exit QTE clicked");

        // Stop QTE
        if (qteManager != null)
        {
            qteManager.StopQTE();
        }

        // Disable canvas or hide QTE
        if (qteCanvas != null)
        {
            qteCanvas.SetActive(false);
        }

        // Optionally disable the dinosaur
        if (dinosaurQTE != null)
        {
            dinosaurQTE.gameObject.SetActive(false);
        }
    }
}
