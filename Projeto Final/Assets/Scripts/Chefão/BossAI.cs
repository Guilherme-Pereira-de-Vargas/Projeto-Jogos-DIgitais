using UnityEngine;

public class BossAI : MonoBehaviour
{
    public float speed = 2.5f;
    public float detectionRange = 12f;
    public float attackRange = 2f;
    public int health = 10;

    public Transform player;
    public Animator animator;
    public GameManager gameManager; // <- público agora

    private float lastAttackTime;
    public float attackCooldown = 1f;

    void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (animator == null)
            animator = GetComponent<Animator>();

        if (gameManager == null)
            gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= attackRange)
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isAttacking", true);

            if (Time.time - lastAttackTime >= attackCooldown)
            {
                AttackPlayer();
                lastAttackTime = Time.time;
            }
        }
        else if (distance <= detectionRange)
        {
            Vector3 dir = (player.position - transform.position).normalized;
            transform.position += dir * speed * Time.deltaTime;

            animator.SetBool("isRunning", true);
            animator.SetBool("isAttacking", false);
        }
        else
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isAttacking", false);
        }
    }

    void AttackPlayer()
    {
        Debug.Log("👹 Boss atacou o player!");
        if (gameManager != null)
            gameManager.LoseLife(1); // dá 1 de dano
    }

    public void TakeDamage(int dmg)
    {
        health -= dmg;
        Debug.Log($"Boss tomou {dmg} de dano. Vida restante: {health}");
        if (health <= 0) Die();
    }

    void Die()
    {
        Debug.Log("💀 Boss morreu!");
        Destroy(gameObject);
    }
}
