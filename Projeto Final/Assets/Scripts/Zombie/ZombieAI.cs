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

    private bool hasPlayedDetectSound = false;

    void Start()
    {
        
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }

        
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    void Update()
    {
     
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        
        if (distance <= detectionRange)
        {
            if (!hasPlayedDetectSound)
            {
                SafePlay("SomDeZombie");
                hasPlayedDetectSound = true;
            }
        }
        else
        {
            hasPlayedDetectSound = false;
        }

  
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

        SafePlay("SomDeZombie");

     
        GameManager gm = GameManager.Instance;

        if (gm != null)
        {
            gm.LoseLife(1);          
            Debug.Log("Zumbi causou dano ao player!");
        }
        else
        {
            Debug.LogWarning("GameManager não encontrado — player não levou dano!");
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
        if (gm != null) gm.AddPoints(10);

        SafePlay("Coin");
        Destroy(gameObject);
    }

    
    void SafePlay(string soundName)
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.Play(soundName);
    }
}
