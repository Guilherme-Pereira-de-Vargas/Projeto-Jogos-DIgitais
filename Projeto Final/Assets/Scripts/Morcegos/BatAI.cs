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
    private bool hasPlayedDetectSound = false;

    // Dano causado ao player
    public int damageToPlayer = 1;

    // Referência fixa ao script do player
    private PlayerMoviment playerScript;

    void Awake()
    {
        if (animator == null) animator = GetComponent<Animator>();

        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }
    }

    void Start()
    {
        // pega o script aqui pra garantir que não fica null
        if (player != null)
            playerScript = player.GetComponent<PlayerMoviment>();
    }

    void Update()
    {
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null)
            {
                player = p.transform;
                playerScript = player.GetComponent<PlayerMoviment>();
                Debug.Log($"[BatAI] Player found at runtime for bat '{name}'.");
            }
            else return;
        }

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= detectionRange)
        {
            if (!hasPlayedDetectSound)
            {
                SafePlay("SomDeMorcego");
                hasPlayedDetectSound = true;
            }

            if (distance > attackRange)
            {
                Vector2 dir = (player.position - transform.position).normalized;
                transform.position += (Vector3)(dir * speed * Time.deltaTime);

                if (animator != null)
                {
                    animator.SetBool("isMoving", true);
                    animator.SetBool("isAttacking", false);
                }
            }
            else
            {
                if (animator != null)
                {
                    animator.SetBool("isMoving", false);
                    animator.SetBool("isAttacking", true);
                }

                if (Time.time - lastAttackTime >= attackCooldown)
                {
                    Attack();
                    lastAttackTime = Time.time;
                }
            }
        }
        else
        {
            if (animator != null)
            {
                animator.SetBool("isMoving", false);
                animator.SetBool("isAttacking", false);
            }

            hasPlayedDetectSound = false;
        }
    }

    void Attack()
    {
        Debug.Log($"[BatAI] {name} atacou o player!");
        SafePlay("SomDeMorcego");

        // aplica dano ao player
        if (playerScript != null)
        {
            playerScript.TakeDamage(damageToPlayer);
        }
        else
        {
            Debug.LogWarning("[BatAI] playerScript está NULL — dano ignorado.");
        }
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

        SafePlay("Coin");
        Destroy(gameObject);
    }

    void SafePlay(string soundName)
    {
        if (AudioManager.Instance != null && AudioManager.Instance.Exists(soundName))
            AudioManager.Instance.Play(soundName);
    }
}
