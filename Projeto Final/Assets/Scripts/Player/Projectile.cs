using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Configurações do Projétil")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifeTime = 2f;
    [SerializeField] private int damage = 1;

    private Rigidbody2D rb;
    private bool facingRight = true;
    private bool directionSet = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (directionSet)
        {
            float direction = facingRight ? 1f : -1f;
            rb.linearVelocity = new Vector2(direction * speed, 0f);
        }
    }

    public void SetDirection(bool faceRight)
    {
        facingRight = faceRight;
        directionSet = true;

        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * (facingRight ? 1 : -1);
        transform.localScale = scale;

        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
       
        ZombieAI zombie = other.GetComponent<ZombieAI>();
        if (zombie != null)
        {
            zombie.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        
        BatAI bat = other.GetComponent<BatAI>();
        if (bat != null)
        {
            bat.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        
        BossAI boss = other.GetComponent<BossAI>();
        if (boss != null)
        {
            boss.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }
    }
}
