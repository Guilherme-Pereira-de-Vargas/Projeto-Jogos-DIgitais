using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [Header("Referências")]
    public GameObject bulletPrefab;   // Arrasta o prefab da Bullet aqui
    public Transform firePoint;       // Posição da arma ou da frente do Player

    [Header("Configurações do Tiro")]
    public float bulletSpeed = 10f;
    public float fireRate = 0.3f;     // tempo entre tiros

    private float nextFireTime = 0f;

    void Update()
    {
        // Atira quando apertar Espaço ou clique esquerdo
        if (Input.GetKey(KeyCode.Space) && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        if (bulletPrefab == null || firePoint == null) return;

        // Cria o projétil
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // Faz ele andar pra frente
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = firePoint.right * bulletSpeed; // direção do tiro
        }
    }
}
