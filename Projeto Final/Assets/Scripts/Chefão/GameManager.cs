using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Status do Jogo")]
    public int pontos = 0;
    public int vidas = 3;
    public TextMeshProUGUI textPontos;

    [Header("Configurações do Boss")]
    public int bossSpawnScore = 100;
    public GameObject bossPrefab;
    public Transform bossSpawnPoint;
    private bool bossSpawned = false;

    private void Start()
    {
        UpdatePontosUI();
    }

    private void Update()
    {
        UpdatePontosUI();

        if (!bossSpawned && pontos >= bossSpawnScore)
            SpawnBoss();
    }

    public void AddPoints(int qtd)
    {
        pontos += qtd;
        if (pontos < 0) pontos = 0;
    }

    public void LoseLife(int qtd)
    {
        vidas -= qtd;

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
            player.GetComponent<Player>().RestartPosition();

        if (vidas <= 0)
        {
            Time.timeScale = 0f;
            Debug.Log("Game Over!");
        }
    }

    private void SpawnBoss()
    {
        if (bossPrefab != null && bossSpawnPoint != null)
        {
            GameObject boss = Instantiate(bossPrefab, bossSpawnPoint.position, Quaternion.identity);

            // garante que o boss tenha referência do GameManager
            BossAI bossAI = boss.GetComponent<BossAI>();
            if (bossAI != null)
                bossAI.gameManager = this;

            bossSpawned = true;
            Debug.Log("👹 Boss spawnado!");
        }
    }

    private void UpdatePontosUI()
    {
        if (textPontos != null)
            textPontos.text = "Pontos: " + pontos;
    }
}
