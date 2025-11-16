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
        // GARANTE QUE O PLAYER EXISTE
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }

        // GARANTE QUE O ANIMATOR EXISTE
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    void Update()
    {
        // ⛔ SE O PLAYER NÃO EXISTE, O SCRIPT PARA E NÃO DÁ ERRO
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        // 🔊 Som de detecção (toca só uma vez)
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

        // Movimentação / ataque
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

    // 🔒 ⛔ Método seguro para tocar sons sem dar NullReference
    void SafePlay(string soundName)
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.Play(soundName);
    }
}
