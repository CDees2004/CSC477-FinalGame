using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator animator;
    private bool canMove = true;

    private PlayerInputActions inputActions;

    public Transform playerSprites; // Assign this in Inspector OR find it automatically

    private float lastMoveX = 1f; // Default to facing right
    private float lastMoveY = 0f; // Default to facing horizontally

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Set up Input Actions
        inputActions = new PlayerInputActions();
        inputActions.Player.Enable();

        // Auto-assign playerSprites if not set
        if (playerSprites == null)
        {
            playerSprites = transform.Find("[[ Player Sprites ]] ");
            if (playerSprites == null)
            {
                Debug.LogError("PlayerSprites GameObject not found! Assign it manually in the Inspector.");
            }
        }
    }

    void Update()
    {
        if (!canMove) return;

        // Use new Input System for movement
        movement = inputActions.Player.Move.ReadValue<Vector2>();

        // Normalize diagonal movement
        if (movement.sqrMagnitude > 1)
        {
            movement = movement.normalized;
        }

        // **Prevent small floating values from triggering animations**
        if (Mathf.Abs(movement.x) < 0.1f) movement.x = 0;
        if (Mathf.Abs(movement.y) < 0.1f) movement.y = 0;

        // Set movement animation
        bool isMoving = movement.sqrMagnitude > 0;
        animator.SetBool("IsMoving", isMoving);

        // Update last direction when moving
        if (isMoving)
        {
            lastMoveX = movement.x != 0 ? movement.x : lastMoveX;
            lastMoveY = movement.y != 0 ? movement.y : lastMoveY;
        }

        // Set movement direction in animator
        animator.SetFloat("MoveX", movement.x);
        animator.SetFloat("MoveY", movement.y);

        // Flip player sprite when moving left/right
        if (playerSprites != null && movement.x != 0)
        {
            playerSprites.localScale = new Vector3(Mathf.Sign(movement.x), 1, 1);
        }

        // Handle idle transitions correctly
        if (!isMoving)
        {
            animator.SetFloat("MoveX", lastMoveX);
            animator.SetFloat("MoveY", lastMoveY);
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = movement * moveSpeed;
    }

    public void SetMovementEnabled(bool enabled)
    {
        canMove = enabled;
        if (!enabled)
        {
            rb.linearVelocity = Vector2.zero;
            movement = Vector2.zero;
            animator.SetBool("IsMoving", false);
        }
    }
}
