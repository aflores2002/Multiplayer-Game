using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    void OnTriggerEnter2D(Collider2D other)
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

        // Reset ball position
        transform.position = Vector3.zero;
        rb.velocity = Vector2.zero;

        // Update score (you'll need to implement a score tracking system)
        Debug.Log(playerOneScored ? "Player One Scores!" : "Player Two Scores!");
    }
}