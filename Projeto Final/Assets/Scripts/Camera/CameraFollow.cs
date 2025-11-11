using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Configurações")]
    public Transform target;       // o player
    public float smoothSpeed = 0.125f;
    public Vector3 offset;         // distância da câmera em relação ao player

    void LateUpdate()
    {
        if (target == null) return;

        // posição desejada
        Vector3 desiredPosition = target.position + offset;

        // movimento suave
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // aplica na câmera
        transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, transform.position.z);
    }
}
