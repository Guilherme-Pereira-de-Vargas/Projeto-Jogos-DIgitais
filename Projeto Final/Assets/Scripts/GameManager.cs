using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int pontos = 0;
    public int vidas = 3;
    public TextMeshProUGUI textPontos;

    private void Start()
    {
        InvokeRepeating("AddPoints", 0.01f, 3f);
    }

    // Método chamado automaticamente pelo InvokeRepeating
    private void AddPoints()
    {
        AddPoints(10); // adiciona, por exemplo, 10 pontos a cada 3 segundos
    }

    public void AddPoints(int qtd)
    {
        pontos += qtd;

        if (pontos < 0)
            pontos = 0;

        Debug.Log("Pontos: " + pontos);
    }

    public void LostLifes(int vida)
    {
        vidas -= vida;
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
