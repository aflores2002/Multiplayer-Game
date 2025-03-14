using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BallController : NetworkBehaviour
{
    private Rigidbody2D rb2D;
    private ScoreManager scoreManager;

    [SerializeField] private float kickForce = 10f; // Configurable kick force
    [SerializeField] private float upwardAngle = 0.5f; // Configurable upward angle

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        scoreManager = FindObjectOfType<ScoreManager>();
        if (rb2D != null)
        {
            rb2D.gravityScale = 1;
            Debug.Log("Ball Rigidbody2D initialized with gravity enabled.");
        }
        else
        {
            Debug.LogError("No Rigidbody2D component found on " + gameObject.name);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (IsServer) // Ensure scoring logic is handled on the server
        {
            if (other.CompareTag("RightGoal"))
            {
                scoreManager.AddScore(false); // Client scores
            }
            else if (other.CompareTag("LeftGoal"))
            {
                scoreManager.AddScore(true); // Host scores
            }

            // Reset ball position
            transform.position = Vector3.zero;
            rb2D.velocity = Vector2.zero;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision detected with: " + collision.gameObject.name);

        // Ensure this logic runs only on the server
        if (IsServer && collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                // Calculate kick direction with an upward curve
                Vector2 kickDirection = (transform.position - collision.gameObject.transform.position).normalized;
                kickDirection += Vector2.up * upwardAngle; // Add upward force for curve

                // Apply a kick force
                rb2D.AddForce(kickDirection * kickForce, ForceMode2D.Impulse);

                // Debugging: Log the force and angle
                Debug.Log($"Ball kicked with force: {kickForce} and upward angle: {upwardAngle}");
            }
        }
    }
}