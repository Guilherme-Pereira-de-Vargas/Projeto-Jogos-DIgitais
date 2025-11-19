using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;

public class FinalDoor : MonoBehaviour
{
    [Header("Cena de Vitória")]
    public string victoryScene = "Victory";

    [Header("Pontuação necessária para liberar a porta")]
    public int requiredScore = 250;

    [Header("Luz da porta final (Light2D)")]
    public Light2D doorLight;

    private bool playerNear = false;
    private bool unlocked = false;

    private void Start()
    {
        // Luz começa apagada
        if (doorLight != null)
        {
            doorLight.gameObject.SetActive(false);
            doorLight.enabled = false;
            doorLight.intensity = 0f;
        }
    }

    private void Update()
    {
        // 1 — libera a porta quando atingir a pontuação
        if (!unlocked && GameManager.Instance.GetScore() >= requiredScore)
        {
            unlocked = true;

            if (doorLight != null)
            {
                doorLight.gameObject.SetActive(true);
                doorLight.enabled = true;
                doorLight.intensity = 1f;
            }

            Debug.Log("✨ Porta final desbloqueada! Entre para vencer o jogo.");
        }

        // 2 — precisa estar perto
        if (!playerNear) return;

        // 3 — pressionar E e porta estar liberada
        if (Input.GetKeyDown(KeyCode.E) && unlocked)
        {
            Debug.Log("🏆 Indo para a tela de vitória...");
            SceneManager.LoadScene(victoryScene);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            playerNear = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            playerNear = false;
    }
}
