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

    void Awake()
    {
        if (animator == null) animator = GetComponent<Animator>();
        // Don't rely only on Start — tente já no Awake
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }
    }

    void Update()
    {
        // se player ainda null, tenta encontrar (útil se player é instanciado depois)
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null)
            {
                player = p.transform;
                Debug.Log($"[BatAI] Player found at runtime for bat '{name}'.");
            }
            else
            {
                // só pra evitar spam de logs: checa raramente
                return;
            }
        }

        float distance = Vector2.Distance(transform.position, player.position);

        // debug rápido pra ver se o bat está detectando
        // Debug.Log($"[BatAI] dist to player: {distance} (detectionRange {detectionRange})");

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
                // movimento mais robusto usando Translate (pode ajustar se usar physics)
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
