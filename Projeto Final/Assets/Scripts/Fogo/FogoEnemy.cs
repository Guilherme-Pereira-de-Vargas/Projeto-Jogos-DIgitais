using UnityEngine;

public class FogoEnemy : MonoBehaviour
{
    [Header("Configurações")]
    public float speed = 2f;
    public int life = 1;

    [Header("Dano no Player")]
    public int damage = 1;
    public float damageInterval = 1f;

    private Transform player;
    private bool isPlayerInside = false;
    private float timer = 0f;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (player != null)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                player.position,
                speed * Time.deltaTime
            );
        }

        if (isPlayerInside)
        {
            timer += Time.deltaTime;
            if (timer >= damageInterval)
            {
                GameManager.Instance.LoseLife(damage);
                timer = 0f;
            }
        }
    }

    // ============================
    // COLISÕES
    // ============================
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("🔥 COLIDIU COM: " + collision.gameObject.name + 
                  " | TAG: " + collision.tag);

        // Player entrou
        if (collision.CompareTag("Player"))
        {
            isPlayerInside = true;
            timer = damageInterval;
        }

        // Bullet acertou
        if (collision.CompareTag("Bullet"))
        {
            Debug.Log("💥 BULLET DETECTADA NO FOGO!");
            TakeDamage(1);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInside = false;
            timer = 0f;
        }
    }

    // ============================
    // DANO / MORTE
    // ============================
    public void TakeDamage(int dmg)
    {
        life -= dmg;

        if (life <= 0)
        {
            GameManager.Instance.AddPoints(1);
            Destroy(gameObject);
        }
    }
}
