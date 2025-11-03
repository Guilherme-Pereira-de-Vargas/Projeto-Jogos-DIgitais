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

  
}
