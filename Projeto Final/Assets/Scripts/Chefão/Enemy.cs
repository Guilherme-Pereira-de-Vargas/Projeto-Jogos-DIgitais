using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int damage = 1;
    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (gameManager != null)
                gameManager.LoseLife(damage); // ✅ usa LoseLife, não LostLifes
        }
    }
}
