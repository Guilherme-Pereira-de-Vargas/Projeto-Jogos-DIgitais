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
    public int nextBossScore = 100;
    public GameObject bossPrefab;
    public Transform bossSpawnPoint;

    private int score = 0;
    private bool uiInitialized = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        Invoke(nameof(InitializeUI), 0.05f); // garante que a UI exista
    }

    private void InitializeUI()
    {
        if (player == null)
            player = FindObjectOfType<PlayerMoviment>();

        uiInitialized = true;
        UpdateHeartsUI();
        UpdateScoreUI();
    }

    public void AddPoints(int amount)
    {
        AddScore(amount);
    }

    public void AddScore(int amount)
    {
        score += amount;
        if (score < 0) score = 0;

        UpdateScoreUI();

        if (score >= nextBossScore)
        {
            SpawnBoss();
            nextBossScore += 100;
        }
    }

    private void UpdateScoreUI()
    {
        if (!uiInitialized) return;
        if (textPontos == null) return;

        textPontos.text = "Pontos: " + score;
    }

    public void LoseLife(int qtd)
    {
        if (player == null) return;

        player.TakeDamage(qtd);
        UpdateHeartsUI();
    }

    public void PlayerTookDamage()
    {
        UpdateHeartsUI();
    }

    public void UpdateHeartsUI()
    {
        if (!uiInitialized) return; // ← NUNCA MAIS QUEBRA POR ISSO

        if (player == null) return;
        if (hearts == null || hearts.Length == 0) return;
        if (fullHeart == null || emptyHeart == null) return;

        int lives = player.GetLives();

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < lives) hearts[i].sprite = fullHeart;
            else hearts[i].sprite = emptyHeart;
        }
    }

    private void SpawnBoss()
    {
        if (bossPrefab != null && bossSpawnPoint != null)
        {
            Instantiate(bossPrefab, bossSpawnPoint.position, Quaternion.identity);
            Debug.Log($"👹 Boss Spawnado aos {score} pontos!");
        }
        else
        {
            Debug.LogWarning("GameManager: Não consegui spawnar o Boss.");
        }
    }

    public int GetScore() => score;
}
