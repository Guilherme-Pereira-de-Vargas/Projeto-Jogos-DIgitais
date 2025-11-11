using UnityEngine;

public class BatAI : MonoBehaviour
{
    public float speed = 3.5f;
    public float detectionRange = 10f;
    public float attackRange = 1.2f;
    public int health = 1;
    public Animator animator;
    public Transform player;
    public int pointsOnDeath = 5;

    private float lastAttackTime;
    public float attackCooldown = 1.2f;

    void Start()
    {
        if (player == null) player = GameObject.FindGameObjectWithTag("Player")?.transform;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= detectionRange)
        {
            if (distance > attackRange)
            {
                Vector2 dir = (player.position - transform.position).normalized;
                transform.position += (Vector3)(dir * speed * Time.deltaTime);

                animator?.SetBool("isMoving", true);
                animator?.SetBool("isAttacking", false);
            }
            else
            {
                animator?.SetBool("isMoving", false);
                animator?.SetBool("isAttacking", true);

                if (Time.time - lastAttackTime >= attackCooldown)
                {
                    Attack();
                    lastAttackTime = Time.time;
                }
            }
        }
        else
        {
            animator?.SetBool("isMoving", false);
            animator?.SetBool("isAttacking", false);
        }
    }

    void Attack()
    {
        Debug.Log("Morcego atacou!");
    }

    public void TakeDamage(int dmg)
    {
        health -= dmg;
        if (health <= 0) Die();
    }

    void Die()
    {
        GameManager gm = FindObjectOfType<GameManager>();
        if (gm != null) gm.AddPoints(pointsOnDeath);
        Destroy(gameObject);
    }
}
