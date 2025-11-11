using UnityEngine;

public class ZombieAI : MonoBehaviour
{
    public float speed = 2f;
    public float detectionRange = 10f;
    public float attackRange = 1.5f;
    public int health = 2;
    public Animator animator;
    public Transform player;

    private float lastAttackTime;
    public float attackCooldown = 1.5f;

    void Start()
    {
        if (player == null) player = GameObject.FindGameObjectWithTag("Player")?.transform;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance > attackRange)
        {
            Vector3 dir = (player.position - transform.position).normalized;
            transform.position += dir * speed * Time.deltaTime;

            animator.SetBool("isWalking", true);
            animator.SetBool("isAttacking", false);
        }
        else
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isAttacking", true);

            if (Time.time - lastAttackTime >= attackCooldown)
            {
                Attack();
                lastAttackTime = Time.time;
            }
        }
    }

    void Attack()
    {
        Debug.Log("Zombie atacou!");
    }

    public void TakeDamage(int dmg)
    {
        health -= dmg;
        if (health <= 0) Die();
    }

    void Die()
    {
        GameManager gm = FindObjectOfType<GameManager>();
        if (gm != null) gm.AddPoints(10);
        Destroy(gameObject);
    }
}
