using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class Player : MonoBehaviour
{
    public Animator anim;
    private Rigidbody2D rigi;
    public float speed = 5f;
    public Vector2 PosicaoInicial;

    [Header("Tiro")]
    public GameObject bulletPrefab;
    public Transform firePointRight;
    public Transform firePointLeft;
    public float bulletSpeed = 10f;

    private bool facingRight = true;

    void Start()
    {
        anim = GetComponent<Animator>();
        rigi = GetComponent<Rigidbody2D>();
        rigi.gravityScale = 0;
        PosicaoInicial = transform.position;
    }

    void Update()
    {
        Move();
        ShootInput();
    }

    void Move()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        Vector2 movimento = new Vector2(moveX, moveY);
        rigi.linearVelocity = movimento * speed;

        if (movimento.magnitude > 0)
        {
            anim.SetInteger("transition", 1);

            if (moveX > 0) transform.eulerAngles = new Vector2(0, 0);
            else if (moveX < 0) transform.eulerAngles = new Vector2(0, 180);

            facingRight = moveX >= 0;
        }
        else
        {
            anim.SetInteger("transition", 0);
        }
    }

    void ShootInput()
    {
        if (Input.GetKeyDown(KeyCode.F) || Input.GetMouseButtonDown(0))
        {
            anim.ResetTrigger(facingRight ? "ShootLeft" : "ShootRight");
            anim.SetTrigger(facingRight ? "ShootRight" : "ShootLeft");
        }
    }

    public void FireProjectile()
    {
        Transform firePoint = facingRight ? firePointRight : firePointLeft;
        float dir = facingRight ? 1f : -1f;

        if (bulletPrefab != null && firePoint != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null) rb.linearVelocity = new Vector2(dir * bulletSpeed, 0);

            Vector3 scale = bullet.transform.localScale;
            scale.x = Mathf.Abs(scale.x) * dir;
            bullet.transform.localScale = scale;

            Destroy(bullet, 2f);
        }
    }

    public void RestartPosition()
    {
        transform.position = PosicaoInicial;
    }
}
