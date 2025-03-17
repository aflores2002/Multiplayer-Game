using UnityEngine;
using Unity.Netcode;

public class PlayerController : NetworkBehaviour
{
    public float moveSpeed = 5f; // Adjusts the speed of the player
    public float jumpForce = 1f; // Adjust jump force for player
    public KeyCode[] moveKeys = new KeyCode[] { KeyCode.A, KeyCode.D }; // Default keys

    private Rigidbody2D rb2D;
    private Animator animator;
    private bool isGrounded;
    private NetworkVariable<Vector2> networkedPosition = new NetworkVariable<Vector2>();

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Log the initial jump force
        Debug.Log("Initial jump force: " + jumpForce);

        // Ensure appropriate physics setup
        if (rb2D != null)
        {
            rb2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            rb2D.gravityScale = 1;
            Debug.Log("Rigidbody2D initialized with gravity enabled.");
        }
        else
        {
            Debug.LogError("No Rigidbody2D component found on " + gameObject.name);
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

        // Debug log for input
        Debug.Log($"Player {gameObject.name} horizontal input: {horizontalInput}");

        // Set animation parameters
        if (animator != null)
        {
            animator.SetBool("isRunning", horizontalInput != 0);

            // Jumping
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                Debug.Log("Jumping with force: " + jumpForce);
                rb2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                isGrounded = false; // Set isGrounded to false after jumping
            }
        }

        // Play walk sound if moving and grounded
        if (horizontalInput != 0 && isGrounded)
        {
            if (!AudioManager.Instance.footstepsSource.isPlaying)
            {
                AudioManager.Instance.PlayWalkSound(); // Play walk sound
            }
        }
        else
        {
            AudioManager.Instance.StopFootsteps(); // Stop walk sound
        }

        // Move the player
        if (rb2D != null)
        {
            Vector2 movement = new Vector2(horizontalInput * moveSpeed, rb2D.linearVelocity.y);
            rb2D.linearVelocity = movement;

            // Update networked position
            if (IsServer)
            {
                networkedPosition.Value = transform.position;
            }

            // Rotate player based on movement direction
            if (horizontalInput > 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (horizontalInput < 0)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
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

    void OnCollisionExit2D(Collision2D collision)
    {
        // Check if player left the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Left Ground");
            isGrounded = false;
        }
    }
}