using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryUI : MonoBehaviour
{
    [Header("Música da Vitória")]
    public AudioClip victoryMusic;

    private AudioSource audioSource;

    private void Start()
    {
        // 🔥 PARA TODA MÚSICA DO JOGO
        if (AudioManager.Instance != null)
            AudioManager.Instance.StopAll();

        // 🔥 ADICIONA UM AUDIOSOURCE PARA A MÚSICA FINAL
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = victoryMusic;
        audioSource.loop = false;
        audioSource.volume = 1f;
        audioSource.Play();

        // GARANTE QUE O JOGO VOLTE AO TEMPO NORMAL
        Time.timeScale = 1f;
    }

    // 🔥 Botão Restart volta pra fase_1
    public void RestartGame()
    {
        SceneManager.LoadScene("fase_1");
    }
}
