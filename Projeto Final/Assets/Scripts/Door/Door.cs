using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;

public class Door : MonoBehaviour
{
    [Header("Nome da cena para carregar")]
    public string nextSceneName = "fase_2";

    [Header("Pontuação mínima para usar a porta")]
    public int requiredScore = 50;

    [Header("Luz da porta (deixe aqui a Light2D filha)")]
    public Light2D doorLight;

    private bool playerNear = false;
    private bool unlocked = false;

    private void Start()
    {
        Debug.Log("Start da porta rodou!");

        
        if (doorLight != null)
        {
            doorLight.enabled = false;
            doorLight.intensity = 0f;
            doorLight.gameObject.SetActive(false); 
        }
    }

    private void Update()
    {
        int score = GameManager.Instance.GetScore();

        // Ativa a luz quando atingir a pontuação
        if (!unlocked && score >= requiredScore)
        {
            unlocked = true;

            if (doorLight != null)
            {
                doorLight.gameObject.SetActive(true);
                doorLight.enabled = true;
                doorLight.intensity = 1f;
            }

            Debug.Log("✨ Porta desbloqueada! Vá até ela.");
        }

        
        if (!playerNear) return;

        
        if (Input.GetKeyDown(KeyCode.E) && unlocked)
        {
            Debug.Log("🌟 Entrando na próxima fase...");
            SceneManager.LoadScene(nextSceneName);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerNear = true;
            Debug.Log("Pressione E para entrar.");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerNear = false;
        }
    }
}
