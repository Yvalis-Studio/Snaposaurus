using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class QTEManager : MonoBehaviour
{
    // GameObject References
    public DinosaurQTE dinosaur;

    // UI
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI nextKeyText;


    // DBM PULL
    public float dbmPull = 3.0f;
    float dbmTimer;

    // QTE TIMER
    float timeLimit;
    float timer;

    // QTE Status
    public bool isActive { get { return qteActive; } }
    bool qteActive;
    public bool isSuccess { get { return success; } }
    public bool success;

    bool perfect = true;

    string[] possibleKeys = { "up", "down", "left", "right" };
    List<string> qteKeyList = new List<string>();
    string qteNextKey = "up";

    void Start()
    {
        dbmTimer = dbmPull;
        nextKeyText.text = $"{dbmTimer} s";
        timerText.text = "Get Ready...";
        success = false;
        qteActive = false;
    }

    void Update()
    {
        // Prepull
        if (dbmTimer > 0)
        {
            dbmTimer -= Time.deltaTime;
            nextKeyText.text = $"{dbmTimer.ToString("F2")} s";

            if (dbmTimer <= 0)
            {
                StartQTE();
            }
        }

        // In fight
        if (qteActive)
        {
            timer -= Time.deltaTime;
            timerText.text = $"Time left : {timer.ToString("F2")} s";

            if (timer <= 0f)
            {
                QTEFail();
            }
        }
    }

    public void StartQTE()
    {
        GenerateQTEKeyList(dinosaur.qteLength);

        timeLimit = dinosaur.qteTimer;
        timer = dinosaur.qteTimer;
        qteActive = true;
        success = false;

        GetNextKey();
        nextKeyText.gameObject.SetActive(true);
        timerText.gameObject.SetActive(true);
    }

    void GenerateQTEKeyList(int length)
    {
        while (length > 0)
        {
            length--;
            string chosenKey = possibleKeys[Random.Range(0, possibleKeys.Count() - 1)];
            qteKeyList.Add(chosenKey);
        }
    }

    bool GetNextKey()
    {
        if (qteKeyList.Count > 0)
        {
            qteNextKey = qteKeyList[0];
            qteKeyList.RemoveAt(0);
            UpdateKeyPreview();
            return true;
        }
        return false;
    }

    void UpdateKeyPreview()
    {
        string previewText = qteNextKey;
        if (qteKeyList.Count > 0)
        {
            previewText += $" - {qteKeyList[0]}";
        }
        if (qteKeyList.Count > 1)
        {
            previewText += $" - {qteKeyList[1]}";
        }
        nextKeyText.text = previewText;
    }

    void QTESuccess()
    {
        success = true;
        qteActive = false;
        nextKeyText.text = "Success!";
        // Invoke(nameof(HidePrompt), 0.5f);
        if (perfect)
        {
            GameManager.Instance.Dino1.score = 3;
        }
        else
        {
            GameManager.Instance.Dino1.score = 2;
        }
        // SceneTransition.Instance.TransitionToScene("Level 1");
        Invoke(nameof(ExitQTE), 1.0f);
    }

    void QTEFail()
    {
        qteActive = false;
        nextKeyText.text = "Failed!";
        GameManager.Instance.Dino1.score = 1;
        // SceneTransition.Instance.TransitionToScene("Level 1");
        Invoke(nameof(ExitQTE), 1.0f);
    }

    void ExitQTE()
    {
        SceneTransition.Instance.TransitionToScene("Level 1");
    }

    public void DoQTE(string key)
    {
        if (qteActive)
        {
            if (key == qteNextKey.ToLower())
            {
                if (!GetNextKey())
                {
                    QTESuccess();
                }
            }
            else
            {
                perfect = false;
            }
        }
    }
}
