using System.Collections;
using UnityEngine;

public class DinosaurQTE : MonoBehaviour
{
    [Header("QTE Settings")]
    public float qteTimer = 5;
    public int qteLength = 5;

    [Header("Dinosaur Info")]
    public string dinoName = "T-Rex";

    [Header("Photo Sprites")]
    public Sprite perfectPhoto; // Photo perfect si succès
    public Sprite clearPhoto; // Photo nette si succès
    public Sprite blurryPhoto; // Photo floue si échec

    [Header("References")]
    public QTEManager qteManager;
    public float photoDisplayDelay = 1.5f; // Délai après le message de succès/échec

    [Header("Retry Settings")]
    public bool autoRetry = true;
    public float retryDelay = 4f; // Délai avant de relancer le QTE

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
            Debug.Log("QTE finished! Showing photo in " + photoDisplayDelay + "s");
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
        Debug.Log($"ShowPhoto called - Success: {qteManager.isSuccess}");

        if (PhotoManager.Instance == null)
        {
            Debug.LogError("PhotoManager not found in scene!");
            return;
        }

        Sprite photoToShow;
        // Choose photo based on QTE result
        if (qteManager.isSuccess)
        {
            if (qteManager.perfect)
            {
                photoToShow = perfectPhoto;
            }
            else
            {
                photoToShow = clearPhoto;
            }
        }
        else
        {
            photoToShow = blurryPhoto;
        }

        // Sprite photoToShow = qteManager.isSuccess ? clearPhoto : blurryPhoto;
            Debug.Log($"Photo to show: {(photoToShow != null ? photoToShow.name : "NULL")}");

        if (photoToShow != null)
        {
            // Show and save the photo
            Debug.Log("Calling PhotoManager.ShowAndSavePhoto...");
            PhotoManager.Instance.ShowAndSavePhoto(dinoName, photoToShow, qteManager.isSuccess);
        }
        else
        {
            Debug.LogWarning($"No photo sprite assigned for {(qteManager.isSuccess ? "success" : "failure")}");
        }

        // Schedule retry only if failed and autoRetry enabled
        if (autoRetry && !qteManager.isSuccess)
        {
            Debug.Log($"QTE Failed - Retrying in {retryDelay} seconds");
            Invoke(nameof(RetryQTE), retryDelay);
        }
        else if (qteManager.isSuccess)
        {
            Debug.Log("QTE Success! Stopping QTE system - No retry.");
            // Don't invoke retry, QTE stops here
        }
    }

    void RetryQTE()
    {
        Debug.Log("Retrying QTE...");

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
