using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class PlayerMoviment : MonoBehaviour
{
    [Header("Configurações de movimento")]
    [SerializeField] private float velocidade = 5f;

    [Header("Configurações de tiro")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePointRight;
    [SerializeField] private Transform firePointLeft;
    [SerializeField] private float bulletSpeed = 10f;

    private Rigidbody2D rb;
    private Animator anim;

    private float horizontalInput;
    private float verticalInput;
    private bool facingRight = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // --- ENTRADA DE MOVIMENTO ---
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // Movimento físico
        rb.linearVelocity = new Vector2(horizontalInput * velocidade, verticalInput * velocidade);

        // Atualiza direção do sprite
        if (horizontalInput > 0)
            facingRight = true;
        else if (horizontalInput < 0)
            facingRight = false;

        // --- ANIMAÇÃO DE MOVIMENTO ---
        bool isMovingHorizontally = horizontalInput != 0f;
        bool isMovingVertically = verticalInput != 0f;

        if (isMovingVertically && !isMovingHorizontally)
        {
            anim.SetBool("IsMoving", false);
            anim.SetFloat("Horizontal", 0);
        }
        else
        {
            anim.SetBool("IsMoving", (horizontalInput != 0f || verticalInput != 0f));
            anim.SetFloat("Horizontal", horizontalInput);
        }

        // --- DISPARO ---
        if (Input.GetKeyDown(KeyCode.F) || Input.GetMouseButtonDown(0))
        {
            Shoot(); // apenas inicia a animação agora
        }
    }

    private void Shoot()
    {
        if (facingRight)
        {
            anim.ResetTrigger("ShootLeft");
            anim.SetTrigger("ShootRight");
        }
        else
        {
            anim.ResetTrigger("ShootRight");
            anim.SetTrigger("ShootLeft");
        }
    }

    // 🔥 Esse método será chamado por um evento na animação
    public void FireProjectile()
    {
        if (facingRight)
            FireBullet(firePointRight, 1f);
        else
            FireBullet(firePointLeft, -1f);
    }

    private void FireBullet(Transform firePoint, float direction)
    {
        if (bulletPrefab != null && firePoint != null)
        {
            Vector3 spawnPos = firePoint.position + new Vector3(0.2f * direction, 0f, 0f);

            GameObject bullet = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);
            Rigidbody2D rbBullet = bullet.GetComponent<Rigidbody2D>();

            if (rbBullet != null)
                rbBullet.linearVelocity = new Vector2(direction * bulletSpeed, 0f);

            Vector3 scale = bullet.transform.localScale;
            scale.x = Mathf.Abs(scale.x) * direction;
            bullet.transform.localScale = scale;

            Destroy(bullet, 2f);
        }
    }
}
