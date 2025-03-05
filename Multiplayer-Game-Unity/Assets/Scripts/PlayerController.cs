using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public KeyCode[] moveKeys;  // 0: Left, 1: Right, 2: Jump
    public bool isPlayerOne;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Horizontal Movement
        float horizontalInput = 0f;
        if (Input.GetKey(moveKeys[0])) // Left key
            horizontalInput = -1f;
        else if (Input.GetKey(moveKeys[1])) // Right key
            horizontalInput = 1f;

        // Move the player
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);

        // Jumping
        if (Input.GetKeyDown(moveKeys[2])) // Jump key
        {
            // Add jump logic here if needed
            rb.AddForce(Vector2.up * 300f, ForceMode2D.Impulse);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Ball kicking logic
        if (collision.gameObject.CompareTag("Ball"))
        {
            Rigidbody2D ballRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (ballRb != null)
            {
                // Calculate kick direction based on player's position and ball's position
                Vector2 kickDirection = (collision.gameObject.transform.position - transform.position).normalized;

                // Apply a kick force
                ballRb.AddForce(kickDirection * 10f, ForceMode2D.Impulse);
            }
        }
    }
}