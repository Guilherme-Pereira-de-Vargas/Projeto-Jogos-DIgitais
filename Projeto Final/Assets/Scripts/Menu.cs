using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [Header("Som do Jogo")]
    public AudioSource musicSource;     // arraste aqui o AudioSource com a música
    public AudioClip musicMainMenu;     // música do menu
    public AudioClip musicGameplay;     // música da fase

    private void Start()
    {
        // Quando o menu abrir, toca a música inicial
        if (musicSource != null && musicMainMenu != null)
        {
            musicSource.clip = musicMainMenu;
            musicSource.loop = true;
            musicSource.volume = 1f;
            musicSource.Play();
        }
    }

    // -----------------------------
    //       SISTEMA DE CENAS
    // -----------------------------
    public void StartGame()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1;

        // trocar música para música da fase
        if (musicSource != null && musicGameplay != null)
        {
            musicSource.clip = musicGameplay;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    public void goMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 0;

        // volta para a música do menu
        if (musicSource != null && musicMainMenu != null)
        {
            musicSource.clip = musicMainMenu;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    // -----------------------------
    //         CONFIG MENU
    // -----------------------------
    public void ActiveConfig(GameObject go)   => go.SetActive(true);
    public void DisableConfig(GameObject go)  => go.SetActive(false);

    // -----------------------------
    //       SISTEMA DE PAUSE
    // -----------------------------
    public void ActivePause(GameObject go)
    {
        go.SetActive(true);
        Time.timeScale = 0;
    }

    public void DisablePause(GameObject go)
    {
        go.SetActive(false);
        Time.timeScale = 1;
    }

    // -----------------------------
    //     LIGAR / DESLIGAR SOM
    // -----------------------------
    public void EnableSound()
    {
        if (musicSource != null)
            musicSource.volume = 1f;       // volume ligado

        AudioListener.volume = 1f;         // ativa tudo
        Debug.Log("🔊 SOM LIGADO");
    }

    public void DisableSound()
    {
        if (musicSource != null)
            musicSource.volume = 0f;       // silencia a música

        AudioListener.volume = 0f;         // silencia tudo
        Debug.Log("🔇 SOM DESLIGADO");
    }
}
