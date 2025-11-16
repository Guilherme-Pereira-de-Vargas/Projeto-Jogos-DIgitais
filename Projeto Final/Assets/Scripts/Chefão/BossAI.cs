using UnityEngine;

public class BossAI : MonoBehaviour
{
    [Header("Configurações")]
    public float speed = 2.5f;
    public float detectionRange = 12f;
    public float attackRange = 2f;
    public int health = 10;

    [Header("Referências")]
    public Transform player;
    public Animator animator;

    private float lastAttackTime;
    public float attackCooldown = 1f;

    void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (animator == null)
            animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        // Ataque
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
        // Persegue o player
        else if (distance <= detectionRange)
        {
            Vector3 dir = (player.position - transform.position).normalized;
            transform.position += dir * speed * Time.deltaTime;

            animator.SetBool("isRunning", true);
            animator.SetBool("isAttacking", false);
        }
        // Parado
        else
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isAttacking", false);
        }
    }

    void AttackPlayer()
    {
        PlayerMoviment playerScript = player.GetComponent<PlayerMoviment>();
        if (playerScript != null)
        {
            playerScript.TakeDamage(1);
        }
    }

    public void TakeDamage(int dmg)
    {
        health -= dmg;

        if (health <= 0)
            Die();
    }

    void Die()
    {
        // Toca o som antes de destruir
        if (AudioManager.Instance != null)
            AudioManager.Instance.Play("Coin");

        // Caso você queira animação de morte:
        if (animator != null)
            animator.SetTrigger("Death");

        // Destrói depois de um pequeno delay
        Destroy(gameObject, 0.1f);
    }
}
