using UnityEngine;

public class Enemy1 : MonoBehaviour, IDamageable
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
        // Movimento simples só pra exemplo
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

    // (Opcional) inverter direção se colidir com parede
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground") || collision.collider.CompareTag("Obstacle"))
        {
            moveRight = !moveRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }
}
