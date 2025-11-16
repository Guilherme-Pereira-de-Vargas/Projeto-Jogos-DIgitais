using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [Header("Referências")]
    public GameObject bulletPrefab;   // Arrasta o prefab da Bullet aqui
    public Transform firePointRight;       // Posição da arma ou da frente do Player
    public Transform firePointLeft;        // Posição da arma para o lado esquerdo do player

    [Header("Configurações do Tiro")]
    public float bulletSpeed = 10f;
    public float fireRate = 0.3f;     // tempo entre tiros

    private float nextFireTime = 0f;
    private PlayerMoviment player;

    void Start()
    {
        player = GetComponent<PlayerMoviment>(); // Pega o script do PlayerMoviment
    }

    void Update()
    {
        // Atira quando apertar F ou clicar com o mouse
        if (Time.time >= nextFireTime && (Input.GetKey(KeyCode.F) || Input.GetMouseButton(0)))
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        if (bulletPrefab == null || (firePointRight == null && firePointLeft == null)) return;

        // Cria o projétil
        if (player.isFacingRight)
            FireBullet(firePointRight, 1f);
        else
            FireBullet(firePointLeft, -1f);
    }

    void FireBullet(Transform firePoint, float direction)
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = firePoint.right * direction * bulletSpeed; // Direção do tiro
        }
    }
}
