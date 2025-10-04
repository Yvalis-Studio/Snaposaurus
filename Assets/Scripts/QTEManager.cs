using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QTEManager : MonoBehaviour
{
    public TextMeshProUGUI promptText;
    public float timeLimit = 2f;
    public KeyCode targetKey = KeyCode.Space;

    private float timer;
    private bool qteActive = false;
    private bool success = false;

    void Start()
    {
        StartQTE(KeyCode.E, 5.0f);
        
        // promptText.text = $"Time left : {timer}";
    }

    void Update()
    {
        if (qteActive)
        {
            timer -= Time.deltaTime;
            promptText.text = $"Time left : {timer}";
            if (Input.GetKeyDown(targetKey))
            {
                QTESuccess();
            }
            else if (timer <= 0f)
            {
                QTEFail();
            }
        }
    }

    public void StartQTE(KeyCode key, float duration)
    {
        targetKey = key;
        timeLimit = duration;
        timer = duration;
        qteActive = true;
        success = false;

        promptText.text = $"Press {key.ToString()}!";
        promptText.gameObject.SetActive(true);
    }

    void QTESuccess()
    {
        success = true;
        qteActive = false;
        promptText.text = "Success!";
        Invoke(nameof(HidePrompt), 0.5f);
    }

    void QTEFail()
    {
        qteActive = false;
        promptText.text = "Failed!";
        Invoke(nameof(HidePrompt), 0.5f);
    }

    void HidePrompt()
    {
        promptText.gameObject.SetActive(false);
    }
}
