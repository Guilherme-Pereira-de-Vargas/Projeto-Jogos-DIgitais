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
    private bool bossSpawned = false;

    [Header("Sistema de Vitória")]
    public GameObject victoryPanel;
    public TextMeshProUGUI victoryText;
    public AudioSource audioSource;
    public AudioClip victoryMusic;
    public int scoreToWin = 400;

    private int score = 0;
    private bool uiInitialized = false;
    private bool hasWon = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        Invoke(nameof(InitializeUI), 0.05f);

        if (victoryPanel != null)
            victoryPanel.SetActive(false);
    }

    private void InitializeUI()
    {
        if (player == null)
            player = FindObjectOfType<PlayerMoviment>();

        uiInitialized = true;
        UpdateHeartsUI();
        UpdateScoreUI();
    }

    // ---------------------------
    //        SCORE
    // ---------------------------
    public void AddPoints(int amount)
    {
        AddScore(amount);
    }

    public void AddScore(int amount)
    {
        if (hasWon) return;

        score += amount;
        if (score < 0) score = 0;

        UpdateScoreUI();

        // spawn de boss corrigido
        if (score >= nextBossScore && !bossSpawned)
        {
            bossSpawned = true;
            SpawnBoss();
            nextBossScore += 100;
        }

        if (score >= scoreToWin)
        {
            WinGame();
        }
    }

    private void UpdateScoreUI()
    {
        if (!uiInitialized || textPontos == null) return;

        textPontos.text = "Pontos: " + score;
    }

    // ---------------------------
    //        VIDAS
    // ---------------------------
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
        if (!uiInitialized) return;
        if (player == null || hearts == null || hearts.Length == 0) return;
        if (fullHeart == null || emptyHeart == null) return;

        int lives = player.GetLives();

        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].sprite = (i < lives) ? fullHeart : emptyHeart;
        }
    }

    // ---------------------------
    //        SPAWN DE BOSS
    // ---------------------------
    private void SpawnBoss()
    {
        if (bossPrefab != null && bossSpawnPoint != null)
        {
            Instantiate(bossPrefab, bossSpawnPoint.position, Quaternion.identity);
            Debug.Log($"👹 Boss Spawnado aos {score} pontos!");

            // permite novo spawn no futuro
            Invoke(nameof(ResetBossSpawn), 1f);
        }
        else
        {
            Debug.LogWarning("GameManager: Não consegui spawnar o Boss.");
        }
    }

    private void ResetBossSpawn()
    {
        bossSpawned = false;
    }

    // ---------------------------
    //        VITÓRIA
    // ---------------------------
    private void WinGame()
    {
        if (hasWon) return;
        hasWon = true;

        Debug.Log("🎉 O jogador venceu o jogo!");

        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);

            if (victoryText != null)
                victoryText.text = "VOCÊ VENCEU!";
        }

        if (audioSource != null && victoryMusic != null)
        {
            audioSource.clip = victoryMusic;
            audioSource.Play();
        }

        Time.timeScale = 1f;
    }

    public int GetScore() => score;
}
