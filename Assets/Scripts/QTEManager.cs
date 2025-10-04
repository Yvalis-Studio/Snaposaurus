using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class QTEManager : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI nextKeyText;
    public float timeLimit = 2f;
    public KeyCode targetKey = KeyCode.Space;


    private float timer;
    private bool qteActive = false;
    private bool success = false;

    private string[] possibleKeys = { "up", "down", "left", "right" };
    private List<string> qteKeyList = new List<string>();
    private string qteNextKey = "up";

    public DinosaurQTE dinosaur;

    void Start()
    {
        StartQTE(5.0f);

        // foreach (var key in targetKeyList)
        // {
        //     Debug.Log(key);
        // }

        // timerText.text = $"Time left : {timer}";
    }

    void Update()
    {
        if (qteActive)
        {
            timer -= Time.deltaTime;
            timerText.text = $"Time left : {timer}";

            if (timer <= 0f)
            {
                QTEFail();
            }
        }
    }

    public void StartQTE(float duration)
    {
        GenerateQTEKeyList(dinosaur.qteLength);

        timeLimit = duration;
        timer = duration;
        qteActive = true;
        success = false;

        GetNextKey();
        nextKeyText.gameObject.SetActive(true);
        timerText.gameObject.SetActive(true);
    }

    void GenerateQTEKeyList(int length)
    {
        while (length >= 0)
        {
            length--;
            string chosenKey = possibleKeys[Random.Range(0, possibleKeys.Count() - 1)];
            qteKeyList.Add(chosenKey);
            Debug.Log(qteKeyList);
        }
    }

    bool GetNextKey()
    {
        if (qteKeyList.Count > 0)
        {
            qteNextKey = qteKeyList[0];
            qteKeyList.RemoveAt(0);
            nextKeyText.text = qteNextKey;
            return true;
        }
        return false;
    }

    void QTESuccess()
    {
        success = true;
        qteActive = false;
        nextKeyText.text = "Success!";
        // Invoke(nameof(HidePrompt), 0.5f);
    }

    void QTEFail()
    {
        qteActive = false;
        nextKeyText.text = "Failed!";
        // Invoke(nameof(HidePrompt), 0.5f);
    }

    // void HidePrompt()
    // {
    //     timerText.gameObject.SetActive(false);
    // }

    public void DoQTE(string key)
    {
        if (qteActive && key == qteNextKey.ToLower())
        {
            if (!GetNextKey())
            {
                QTESuccess();
            }
        }
    }
}
