using System.Collections;
using UnityEngine;

public class DinosaurQTE : MonoBehaviour
{
    [Header("References")]
    public QTEManager qteManager;
    public PhotoManager photoManager;

    [Header("Display Settings")]
    public float photoDisplayDelay = 1.5f; // Delay after success/failure message before showing photo

    [Header("Retry Settings")]
    public bool autoRetry = true;
    public float retryDelay = 4f; // Delay before restarting QTE after failure

    // Runtime data - assigned dynamically by QTESceneInitializer
    [HideInInspector] public DinosaurData dinosaurData;
    [HideInInspector] public float baseTimeLimit;
    [HideInInspector] public int baseKeyCount;
    [HideInInspector] public string dinoName;
    [HideInInspector] public Sprite perfectPhoto;
    [HideInInspector] public Sprite clearPhoto;
    [HideInInspector] public Sprite blurryPhoto;

    private bool photoShown = false;
    private bool qteWasActive = false;

    void Start()
    {
        // Reset states at start
        photoShown = false;
        qteWasActive = false;
    }

    void Update()
    {
        // Track if QTE was running
        if (qteManager != null && qteManager.isActive)
        {
            qteWasActive = true;
            // Debug.Log("QTE is active");
        }

        // Check if QTE just finished
        if (qteManager != null && !qteManager.isActive && qteWasActive && !photoShown)
        {
            // Debug.Log("QTE finished! Showing photo in " + photoDisplayDelay + "s");
            photoShown = true;
            StartCoroutine(ShowPhotoAfterDelay());
        }
    }

    IEnumerator ShowPhotoAfterDelay()
    {
        yield return new WaitForSeconds(photoDisplayDelay);
        ShowPhoto();
    }

    void ShowPhoto()
    {
        Debug.Log($"ShowPhoto called - Success: {qteManager.isSuccess}, Perfect: {qteManager.perfect}");

        Sprite photoToShow;
        // Choose photo based on QTE result
        if (qteManager.isSuccess)
        {
            if (qteManager.perfect)
            {
                photoToShow = perfectPhoto;
                Debug.Log($"Using perfectPhoto: {(perfectPhoto != null ? perfectPhoto.name : "NULL")}");
            }
            else
            {
                photoToShow = clearPhoto;
                Debug.Log($"Using clearPhoto: {(clearPhoto != null ? clearPhoto.name : "NULL")}");
            }
        }
        else
        {
            photoToShow = blurryPhoto;
            Debug.Log($"Using blurryPhoto: {(blurryPhoto != null ? blurryPhoto.name : "NULL")}");
        }

        Debug.Log($"Photo to show: {(photoToShow != null ? photoToShow.name : "NULL")}");

        if (photoToShow != null)
        {
            // Show and save the photo
            Debug.Log("Calling PhotoManager.ShowAndSavePhoto...");
            // Get dinosaurID from DinosaurData, fallback to dinoName if not available
            string dinosaurID = (dinosaurData != null) ? dinosaurData.dinosaurID : dinoName.ToLower();
            bool isPerfect = qteManager.perfect;

            photoManager.ShowAndSavePhoto(dinoName, photoToShow, qteManager.isSuccess, isPerfect, dinosaurID);
        }
        else
        {
            Debug.LogWarning($"No photo sprite assigned for {(qteManager.isSuccess ? "success" : "failure")}");
        }

        // Schedule retry only if failed and autoRetry enabled
        if (autoRetry && !qteManager.isSuccess)
        {
            // Debug.Log($"QTE Failed - Retrying in {retryDelay} seconds");
            Invoke(nameof(RetryQTE), retryDelay);
        }
        else if (qteManager.isSuccess)
        {
            // Debug.Log("QTE Success! Stopping QTE system - No retry.");
            // Don't invoke retry, QTE stops here
        }
    }

    void RetryQTE()
    {
        // Debug.Log("Retrying QTE...");

        // Reset states
        photoShown = false;
        qteWasActive = false;

        // Restart QTE by resetting the dbmTimer in QTEManager
        if (qteManager != null)
        {
            // You'll need to add a public method in QTEManager to restart
            qteManager.RestartQTE();
        }
    }

    public void ResetQTE()
    {
        photoShown = false;
        qteWasActive = false;
    }
    public bool isActive = true;
    public int score = 0;
}
