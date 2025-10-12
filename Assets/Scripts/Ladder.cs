using UnityEngine;

public class Ladder : MonoBehaviour
{
    // Quand le joueur entre dans la zone
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // On prévient le PlayerController qu'il est à portée
            collision.GetComponent<PlayerController>().SetIsOnLadder(true);
        }
    }

    // Quand le joueur quitte la zone
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // On prévient le PlayerController qu'il n'est plus à portée
            collision.GetComponent<PlayerController>().SetIsOnLadder(false);
        }
    }
}