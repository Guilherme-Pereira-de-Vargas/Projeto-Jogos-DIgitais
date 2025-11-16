using UnityEngine;

public class PhysicsDebugger : MonoBehaviour
{
    void Update()
    {
        Debug.Log("PlayerRB: " + (FindObjectOfType<PlayerMoviment>()?.GetComponent<Rigidbody2D>() != null));
        Debug.Log("PlayerCollider: " + (FindObjectOfType<PlayerMoviment>()?.GetComponent<Collider2D>() != null));

        GameObject player = FindObjectOfType<PlayerMoviment>()?.gameObject;

        if (player != null)
        {
            Collider2D[] cols = player.GetComponentsInChildren<Collider2D>();
            foreach (var c in cols)
                Debug.Log("Player Collider found: " + c.name + " | Trigger: " + c.isTrigger);
        }
    }
}
