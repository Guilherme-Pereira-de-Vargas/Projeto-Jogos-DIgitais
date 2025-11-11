using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    [Header("Configurações de Spawn")]
    public GameObject zombiePrefab;
    public float spawnInterval = 3f;
    public int maxZombies = 10;
    public float spawnRadius = 8f;
    public float minSpawnDistance = 2f; // distância mínima do player

    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        InvokeRepeating(nameof(SpawnZombie), 1f, spawnInterval);
    }

    void SpawnZombie()
    {
        if (player == null || GameObject.FindGameObjectsWithTag("Enemy").Length >= maxZombies)
            return;

        Vector2 randPos;
        // Garante que o ponto esteja entre min e max distância
        do
        {
            randPos = Random.insideUnitCircle * spawnRadius;
        } while (randPos.magnitude < minSpawnDistance);

        Vector3 spawnPos = player.position + new Vector3(randPos.x, randPos.y, 0f);

        GameObject newZombie = Instantiate(zombiePrefab, spawnPos, Quaternion.identity);
        newZombie.tag = "Enemy";

        ZombieAI zombieAI = newZombie.GetComponent<ZombieAI>();
        if (zombieAI == null)
            zombieAI = newZombie.AddComponent<ZombieAI>();

        zombieAI.player = player;
    }
}
