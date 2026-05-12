using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // Player stats to be upgraded
    public float moveSpeed = 5f;
    public float playerMaxHealth = 100.0f; // Max that the bar can be 
    public float playerActualHealth = 100.0f;
    public float playerDamage = 50.0f;

    public Slider healthSlider;

    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator animator;
    private bool canMove = true;

    private PlayerInputActions inputActions;

    private EnemyShader playerHitShader;

    public Transform playerSprites; // Assign this in Inspector OR find it automatically

    private float lastMoveX = 1f; // Default to facing right
    private float lastMoveY = 0f; // Default to facing horizontally

    [Header("Combat")] // Putting a label above these params in the Inspector
    // REMEMBER: inspector values overwrite these values.
    public float attackCooldown = 0.8f;
    public float attackRadius = 5.0f;
    public LayerMask enemyLayers;
    public Transform attackPoint;
    public GameObject attackAnimation;

    private float lastAttackTime;
    private const bool DEBUG = false;

    private void Awake()
    {
        inputActions = new PlayerInputActions();

    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Set up Input Actions
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

        // Properly setting up the slider   
        if (healthSlider != null)
        {
            healthSlider.maxValue = playerMaxHealth;
            healthSlider.value = playerActualHealth;
        }

        // Since attack anim is a child of Player, turn off until ready
        attackAnimation.SetActive(false);

        playerHitShader = GetComponent<EnemyShader>();
    }

    void Update()
    {
        // don't allow player movement unless they are in the 
        // actual playing state. 
        if (Management_Game.Instance.UIState != FsmUIState.IN_GAME || !canMove) return;

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

        // Attacking on input ONLY if the cooldown has passed
        if (inputActions.Player.Attack.WasPressedThisFrame())
        {
            PlayerAttack();
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

    private void OnDisable()
    {
        inputActions.Disable();
    }

    // ----- Methods for Player stats ------
    public void HealPlayer(float healAmount)
    {
        playerActualHealth += healAmount;
        playerActualHealth = Mathf.Clamp(playerActualHealth, 0, playerMaxHealth);

        if (DEBUG) print($"Health after healing {playerActualHealth}");

        UpdateHealthUI();
    }

    public void TakeDamage(float incomingDamage)
    {
        playerActualHealth -= incomingDamage;
        playerActualHealth = Mathf.Clamp(playerActualHealth, 0, playerMaxHealth);

        if (DEBUG) print($"Health after damage {playerActualHealth}");

        if (playerHitShader != null) playerHitShader.PlayHitFlash();
        UpdateHealthUI();
        
        // Trigger lose condition
        if (playerActualHealth <= 0)
        {
            Management_Game.Instance.ChangeUIState(FsmUIState.GAME_OVER);
        }
    }

    public void PlayerAttack()
    {
        if (Time.time - lastAttackTime < attackCooldown)
            return;

        // For cooldowns
        lastAttackTime = Time.time;

        attackAnimation.SetActive(true);


        // Trying this weirdo stuff to get this animation to work
        Animator attackAnimator = attackAnimation.GetComponent<Animator>();
        attackAnimator.Play("Attack_Separate");

        Invoke(nameof(DisableAttackVisual), 0.5f);
    }

    // This is painful
    private void DisableAttackVisual()
    {
        attackAnimation.SetActive(false);
    }

    // Need to call this WITHIN the animation
    public void DealDamage()
    {
        Vector2 center = transform.position;

        // using the transform position rather than the attackPoint position
        Collider2D[] hitEnemies =
            Physics2D.OverlapCircleAll(center, attackRadius, enemyLayers);

        foreach (Collider2D enemyCollider in hitEnemies)
        {
            // I don't know why removing this breaks stuff. It really shouldn't
            if (enemyCollider.isTrigger)
                continue;

            Enemy enemy = enemyCollider.GetComponentInParent<Enemy>();

            if (enemy != null)
            {
                enemy.TakeDamage(playerDamage);
                // Quick visual pause
                StartCoroutine(QuickGamePause(0.1f));
                Debug.Log($"DAMAGED: {enemy.name}");
            }
        }
    }

    private IEnumerator QuickGamePause(float pauseDuration)
    {
        Time.timeScale = 0.0f;
        yield return new WaitForSecondsRealtime(pauseDuration);
        if (DEBUG) print($"paused the game for {pauseDuration}");
        Time.timeScale = 1.0f;
    }

    public void SetMaxHealth(float newMaxHealth)
    {
        playerMaxHealth = newMaxHealth;
        playerActualHealth = Mathf.Clamp(playerActualHealth, 0, playerMaxHealth);

        if (healthSlider != null) healthSlider.maxValue = playerMaxHealth;
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        if (healthSlider != null) healthSlider.value = playerActualHealth;
        if (DEBUG) print($"Slider value {healthSlider.value}");
    }

    // Debugging attack 
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
