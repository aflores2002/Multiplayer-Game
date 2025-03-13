using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BallController : NetworkBehaviour
{
    private Rigidbody rb;
    private ScoreManager scoreManager;

    [SerializeField] private float kickForce = 10f; // Configurable kick force
    [SerializeField] private float upwardAngle = 0.5f; // Configurable upward angle

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
        Debug.Log("Collision detected with: " + collision.gameObject.name);

        // Ensure this logic runs only on the server
        if (IsServer && collision.gameObject.CompareTag("Player"))
        {
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                // Calculate kick direction with an upward curve
                Vector3 kickDirection = (transform.position - collision.gameObject.transform.position).normalized;
                kickDirection += Vector3.up * upwardAngle; // Add upward force for curve

                // Apply a kick force
                rb.AddForce(kickDirection * kickForce, ForceMode.Impulse);

                // Debugging: Log the force and angle
                Debug.Log($"Ball kicked with force: {kickForce} and upward angle: {upwardAngle}");
            }
        }
    }
}