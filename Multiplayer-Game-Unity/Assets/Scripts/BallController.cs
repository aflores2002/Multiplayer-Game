using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
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
        // Goal detection logic
        if (other.CompareTag("Goal"))
        {
            GoalScored(other.gameObject);
        }
    }

    void GoalScored(GameObject goal)
    {
        // Determine which team scored
        bool playerOneScored = goal.name.Contains("PlayerTwoGoal");
        bool playerTwoScored = goal.name.Contains("PlayerOneGoal");

        // Update score using ScoreManager
        if (scoreManager != null)
        {
            scoreManager.AddScore(playerOneScored);
        }

        // Reset ball position
        transform.position = Vector3.zero;
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
        }

        Debug.Log(playerOneScored ? "Player One Scores!" : "Player Two Scores!");
    }
}