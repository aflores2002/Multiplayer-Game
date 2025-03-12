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
            // Set appropriate physics for a ball
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
            rb.constraints = RigidbodyConstraints.FreezePositionZ;
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
}