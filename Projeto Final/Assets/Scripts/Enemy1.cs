using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    [Header("Configurações do inimigo")]
    public int health = 3;
    public float moveSpeed = 2f;
    public bool moveRight = true;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        
        rb.linearVelocity = new Vector2((moveRight ? 1 : -1) * moveSpeed, rb.linearVelocity.y);
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        Debug.Log($"{gameObject.name} levou {amount} de dano! Vida restante: {health}");

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} morreu!");
        Destroy(gameObject);
    }

    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        
        PlayerMoviment player = other.GetComponent<PlayerMoviment>();

        
        if (player == null)
            player = other.GetComponentInParent<PlayerMoviment>();

        
        if (player == null)
            player = other.GetComponentInChildren<PlayerMoviment>();

        
        if (player != null)
        {
            Debug.Log("Jogador tomou dano pelo Enemy1!");
            player.TakeDamage(1);
        }
        else
        {
            Debug.LogWarning("Enemy1 detectou Player, mas NÃO encontrou PlayerMoviment em nenhum lugar!");
        }
    }
}
