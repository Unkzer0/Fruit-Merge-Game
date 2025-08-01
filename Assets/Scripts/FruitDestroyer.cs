using UnityEngine;

public class FruitDestroyer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collided object is tagged as "Fruit"
        if (collision.CompareTag("Fruit"))
        {
            Destroy(collision.gameObject);
        }
    }
}