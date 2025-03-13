using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BallController : NetworkBehaviour
{
    private Rigidbody rb;
    private ScoreManager scoreManager;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        scoreManager = FindObjectOfType<ScoreManager>();
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.FreezePositionZ;
            rb.useGravity = true;
            Debug.Log("Ball Rigidbody initialized with gravity enabled.");
        }
        else
        {
            Debug.LogError("No Rigidbody component found on " + gameObject.name);
        }
    }

    void OnTriggerEnter(Collider other)
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
            rb.velocity = Vector3.zero;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Ball interaction logic
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                // Calculate kick direction based on player's position and ball's position
                Vector3 kickDirection = (transform.position - collision.gameObject.transform.position).normalized;

                // Apply a kick force
                rb.AddForce(kickDirection * 10f, ForceMode.Impulse);
            }
        }
    }
}