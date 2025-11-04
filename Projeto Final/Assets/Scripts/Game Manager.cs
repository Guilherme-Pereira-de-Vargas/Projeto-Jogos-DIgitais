using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int pontos = 0;
    public int vidas = 3;
    public TextMeshProUGUI textPontos;

    public void AddPoints(int qtd)
    {
        pontos = pontos + qtd;

        if (pontos <= 0)
        {
            pontos = 0;
        }

        textPontos.text = "Pontos: " + pontos;
        Debug.Log("Pontos: " + pontos);
    }

    public void LostLifes(int vida)
    {
        vidas = vidas - vida;
        Debug.Log("Vidas: " + vidas);

        GameObject player = GameObject.FindWithTag("Player");
        player.GetComponent<Player>().RestartPosition();

        if (vidas <= 0)
        {
            Time.timeScale = 0;
            Debug.Log("Game Over!");
        }
    }
}
