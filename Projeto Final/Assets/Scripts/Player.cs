using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator anim;
    private Rigidbody2D rigi;
    public float speed = 5f;
    public bool isGround;
    public Vector2 PosicaoInicial;
    public GameManager GameManager;

    void Start()
    {
        anim = GetComponent<Animator>();
        rigi = GetComponent<Rigidbody2D>();
        rigi.gravityScale = 0; // evita "cair" se o jogo for top-down
        PosicaoInicial = transform.position;
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        // Captura as teclas de movimento
        float moveX = Input.GetAxis("Horizontal"); // A e D
        float moveY = Input.GetAxis("Vertical");   // W e S

        // Define o movimento em 2 eixos
        Vector2 movimento = new Vector2(moveX, moveY);

        // Atualiza a velocidade do Rigidbody2D
        rigi.linearVelocity = movimento * speed;

        // Se estiver se movendo
        if (movimento.magnitude > 0)
        {
            anim.SetInteger("transition", 1);

            // Inverte o sprite horizontalmente
            if (moveX > 0)
                transform.eulerAngles = new Vector2(0, 0);
            else if (moveX < 0)
                transform.eulerAngles = new Vector2(0, 180);
        }
        else
        {
            anim.SetInteger("transition", 0);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            isGround = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            isGround = false;
        }
    }

    public void RestartPosition()
    {
        transform.position = PosicaoInicial;
    }
}
