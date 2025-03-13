using UnityEngine;
using Unity.Netcode;

public class PlayerController : NetworkBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 1f; // Adjusted jump force
    public KeyCode[] moveKeys;  // 0: Left, 1: Right
    public bool isPlayerOne;

    private Rigidbody rb;
    private bool isGrounded;
    private NetworkVariable<Vector3> networkedPosition = new NetworkVariable<Vector3>();

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Log the initial jump force
        Debug.Log("Initial jump force: " + jumpForce);

        // Assign controls based on player
        if (isPlayerOne)
        {
            moveKeys = new KeyCode[] { KeyCode.A, KeyCode.D }; // WASD for Player 1
        }
        else
        {
            moveKeys = new KeyCode[] { KeyCode.LeftArrow, KeyCode.RightArrow }; // Arrow keys for Player 2
        }

        // Ensure appropriate physics setup
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionZ;
            rb.useGravity = true;
            Debug.Log("Rigidbody initialized with gravity enabled.");
        }
        else
        {
            Debug.LogError("No Rigidbody component found on " + gameObject.name);
        }
    }

    void Update()
    {
        if (!IsOwner) return; // Only allow the owner to control the player

        // Horizontal Movement
        float horizontalInput = 0f;
        if (Input.GetKey(moveKeys[0])) // Left key
            horizontalInput = -1f;
        else if (Input.GetKey(moveKeys[1])) // Right key
            horizontalInput = 1f;

        // Move the player
        if (rb != null)
        {
            Vector3 movement = new Vector3(horizontalInput * moveSpeed, rb.velocity.y, 0);
            rb.velocity = movement;

            // Update networked position
            if (IsServer)
            {
                networkedPosition.Value = transform.position;
            }

            // Rotate player based on movement direction
            if (horizontalInput > 0)
            {
                transform.rotation = Quaternion.Euler(0, 100, 0);
            }
            else if (horizontalInput < 0)
            {
                transform.rotation = Quaternion.Euler(0, -100, 0);
            }

            // Jumping
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded) // Space bar for jump
            {
                Debug.Log("Jumping with force: " + jumpForce);
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                isGrounded = false; // Set isGrounded to false after jumping
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if player is grounded
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Grounded");
            isGrounded = true;
        }

        // Ball kicking logic
        if (collision.gameObject.CompareTag("Ball"))
        {
            Rigidbody ballRb = collision.gameObject.GetComponent<Rigidbody>();
            if (ballRb != null)
            {
                // Calculate kick direction based on player's position and ball's position
                Vector3 kickDirection = (collision.gameObject.transform.position - transform.position).normalized;

                // Apply a kick force
                ballRb.AddForce(kickDirection * 10f, ForceMode.Impulse);
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        // Check if player left the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Left Ground");
            isGrounded = false;
        }
    }
}