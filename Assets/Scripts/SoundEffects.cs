using UnityEngine;

public class SoundEffects : MonoBehaviour
{
    public InputAction moveAction = PlayerCharacter.MoveAction;
    public AudioClip[] playlist;
    public AudioSource audioSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource.clip = playlist[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (moveAction.WasPressedThisFrame())
        {
            PlayerCharacter.audioSource.Play();
        }
    }
}
