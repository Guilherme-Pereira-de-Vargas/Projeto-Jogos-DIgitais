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
        // Movimento simples só para exemplo
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

    // ===== CORREÇÃO AQUI =====
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))  // garante que só o Player recebe dano
        {
            // *** AQUI ESTÁ A CORREÇÃO PRINCIPAL ***
            PlayerMoviment player = other.GetComponentInParent<PlayerMoviment>();

            if (player != null)
            {
                Debug.Log("Jogador tomou dano!");
                player.TakeDamage(1);
            }
            else
            {
                Debug.LogWarning("O Player foi detectado, mas o PlayerMoviment NÃO está no mesmo objeto!");
            }
        }
    }
}
