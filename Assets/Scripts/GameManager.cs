using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public DinosaurQTE Dino1;
    public Vector3 playerPosition;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Dino1.isActive = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SavePlayerPosition(Vector3 position)
    {
        playerPosition = position;
    }

    public void RestorePlayerPosition(GameObject player)
    {
        player.transform.position = playerPosition;
    }
}
