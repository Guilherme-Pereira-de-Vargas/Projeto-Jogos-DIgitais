using UnityEngine;
using UnityEngine.SceneManagement;

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

    private int lives = 5;

    public bool isFacingRight => facingRight;

    private string currentFireSound = "Fire";

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        string scene = SceneManager.GetActiveScene().name;
        if (scene == "fase_2")
            currentFireSound = "Fire2";
        else if (scene == "fase_3")
            currentFireSound = "Fire3";
        else
            currentFireSound = "Fire";
    }

    void Start()
    {
        lives = 5;
        GameManager.Instance.UpdateHeartsUI();
    }

    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // ========= FIX REAL =========
        rb.linearVelocity = new Vector2(horizontalInput * velocidade, verticalInput * velocidade);
        // ============================

        if (horizontalInput > 0)
            facingRight = true;
        else if (horizontalInput < 0)
            facingRight = false;

        bool isMovingHoriz = horizontalInput != 0f;
        bool isMovingVert = verticalInput != 0f;
        anim.SetBool("IsMoving", (isMovingHoriz || isMovingVert));

        anim.SetFloat("Horizontal", horizontalInput);

        if (Input.GetKeyDown(KeyCode.F) || Input.GetMouseButtonDown(0))
        {
            Shoot();
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

    public void FireProjectile()
    {
        SafePlay(currentFireSound);

        if (facingRight)
            FireBullet(firePointRight, 1f);
        else
            FireBullet(firePointLeft, -1f);
    }

    private void FireBullet(Transform firePoint, float dir)
    {
        if (bulletPrefab != null && firePoint != null)
        {
            Vector3 spawnPos = firePoint.position + new Vector3(0.2f * dir, 0f, 0f);

            GameObject bullet = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);
            Rigidbody2D rbBullet = bullet.GetComponent<Rigidbody2D>();

            if (rbBullet != null)
            {
                // ==== FIX REAL ====
                rbBullet.linearVelocity = new Vector2(dir * bulletSpeed, 0f);
                // ===================
            }

            Vector3 scale = bullet.transform.localScale;
            scale.x = Mathf.Abs(scale.x) * dir;
            bullet.transform.localScale = scale;

            Destroy(bullet, 2f);
        }
    }

    public void TakeDamage(int dmg)
    {
        lives -= dmg;

        // 🔊 SOM DE DANO DO PLAYER ADICIONADO AQUI
        SafePlay("player_damage");

        GameManager.Instance.PlayerTookDamage();

        if (lives <= 0)
        {
            SceneManager.LoadScene("fase_1");
        }
    }

    public int GetLives()
    {
        return lives;
    }

    void SafePlay(string sound)
    {
        if (AudioManager.Instance == null)
            return;

        if (!AudioManager.Instance.Exists(sound))
            return;

        AudioManager.Instance.Play(sound);
    }
}
