using UnityEngine;

public class SpawnFogo : MonoBehaviour
{
    public GameObject fogoPrefab;
    public float spawnInterval = 4f;
    public float spawnRange = 8f;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            Spawn();
            timer = 0f;
        }
    }

    void Spawn()
    {
        Vector2 randomPos = (Vector2)transform.position +
            Random.insideUnitCircle * spawnRange;

        Instantiate(fogoPrefab, randomPos, Quaternion.identity);
    }
}
