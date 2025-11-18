using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [Header("Referências")]
    public GameObject bulletPrefab;   
    public Transform firePointRight;       
    public Transform firePointLeft;        

    [Header("Configurações do Tiro")]
    public float bulletSpeed = 10f;
    public float fireRate = 0.3f;     

    private float nextFireTime = 0f;
    private PlayerMoviment player;

    void Start()
    {
        player = GetComponent<PlayerMoviment>(); 
    }

    void Update()
    {
        
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
