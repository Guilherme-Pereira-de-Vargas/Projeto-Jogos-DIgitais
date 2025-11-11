using UnityEngine;

public class BatSpawner : MonoBehaviour
{
    [Header("Configurações de Spawn")]
    public GameObject batPrefab;
    public float spawnInterval = 3f;
    public int maxBats = 8;
    public float spawnRadius = 8f;
    public float minSpawnDistance = 2f; // distância mínima do player

    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        InvokeRepeating(nameof(SpawnBat), 1f, spawnInterval);
    }

    void SpawnBat()
    {
        if (player == null || GameObject.FindGameObjectsWithTag("Bat").Length >= maxBats)
            return;

        Vector2 randPos;
        // Garante que o ponto esteja fora da área próxima ao player
        do
        {
            randPos = Random.insideUnitCircle * spawnRadius;
        } while (randPos.magnitude < minSpawnDistance);

        Vector3 spawnPos = player.position + new Vector3(randPos.x, randPos.y, 0f);

        GameObject newBat = Instantiate(batPrefab, spawnPos, Quaternion.identity);
        newBat.tag = "Bat";

        BatAI batAI = newBat.GetComponent<BatAI>();
        if (batAI == null)
            batAI = newBat.AddComponent<BatAI>();

        batAI.player = player;
    }
}
