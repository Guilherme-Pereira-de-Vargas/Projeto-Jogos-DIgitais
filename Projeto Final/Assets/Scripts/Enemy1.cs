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
        // Movimento simples
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

    // =============================
    //      SISTEMA DE DANO
    // =============================
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        // 1) Primeiro tenta pegar direto no objeto atingido
        PlayerMoviment player = other.GetComponent<PlayerMoviment>();

        // 2) Se não estiver no mesmo objeto, tenta nos pais
        if (player == null)
            player = other.GetComponentInParent<PlayerMoviment>();

        // 3) Se ainda não encontrou, tenta nos filhos (por segurança)
        if (player == null)
            player = other.GetComponentInChildren<PlayerMoviment>();

        // 4) Agora aplica dano de forma segura
        if (player != null)
        {
            Debug.Log("Jogador tomou dano pelo Enemy1!");
            player.TakeDamage(1);
        }
        else
        {
            Debug.LogWarning("⚠ Enemy1 detectou Player, mas NÃO encontrou PlayerMoviment em nenhum lugar!");
        }
    }
}
