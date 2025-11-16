using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Referências do Player")]
    public PlayerMoviment player;

    [Header("UI de Vidas")]
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    [Header("UI de Pontos")]
    public TextMeshProUGUI textPontos;

    [Header("Configurações de Boss")]
    public int nextBossScore = 100;   // boss nasce no 100, 200, 300...
    public GameObject bossPrefab;
    public Transform bossSpawnPoint;

    private int score = 0;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        if (player == null)
            player = FindObjectOfType<PlayerMoviment>();

        UpdateHeartsUI();  // Atualiza a UI de corações com o valor inicial
        UpdateScoreUI();   // Atualiza a UI de pontos
    }

    // ======= PONTOS =======
    public void AddPoints(int amount)
    {
        AddScore(amount);
    }

    public void AddScore(int amount)
    {
        score += amount;
        if (score < 0) score = 0;

        UpdateScoreUI();

        // Spawnar Boss a cada 100 pontos
        if (score >= nextBossScore)
        {
            SpawnBoss();
            nextBossScore += 100;
        }
    }

    private void UpdateScoreUI()
    {
        if (textPontos != null)
            textPontos.text = "Pontos: " + score;
    }

    // ======= VIDA =======
    public void LoseLife(int qtd)
    {
        if (player == null)
        {
            player = FindObjectOfType<PlayerMoviment>();
            if (player == null) return;
        }

        player.TakeDamage(qtd);  // Chama o método TakeDamage no PlayerMoviment
        UpdateHeartsUI();        // Atualiza a UI de corações
    }

    // Atualiza a UI de corações quando o jogador toma dano
    public void PlayerTookDamage()
    {
        UpdateHeartsUI();  // Atualiza a UI de corações
    }

    // Atualiza os corações na UI
    public void UpdateHeartsUI()
    {
        if (player == null) return;
        if (hearts == null || hearts.Length == 0) return;

        int lives = player.GetLives();  // Pega as vidas do jogador

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < lives)
                hearts[i].sprite = fullHeart;  // Coragem cheia
            else
                hearts[i].sprite = emptyHeart;  // Coragem vazia
        }
    }

    // ======= SPAWN DO BOSS =======
    private void SpawnBoss()
    {
        if (bossPrefab != null && bossSpawnPoint != null)
        {
            Instantiate(bossPrefab, bossSpawnPoint.position, Quaternion.identity);
            Debug.Log($"👹 Boss Spawnado aos {score} pontos!");
        }
    }

    public int GetScore() => score;
}
