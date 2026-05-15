using System.Runtime.CompilerServices;
using UnityEngine;
using Random = UnityEngine.Random;
using DashState = FsmDashState;

public enum FsmDashState
{
    WAITING,
    DASHING,
}

public class Enemy_Dash : Enemy
{
    public float pauseTime = 1.0f;
    public float dashSpeed = 10.0f;


    private Vector2 dashDirection;

    private DashState currentState;

    private float stateTimer;

    protected Transform player;

    protected override void Awake()
    {
        // Getting components from parent class to properly assign roomID
        base.Awake();

        // Unique components
        enemyIdentifier = "Dasher";
        enemyHealth = 150.0f;
        enemyDamage = 7.0f;
        enemyDeathScore = 250;
    }

    protected override void Start()
    {
        base.Start();

        rb = GetComponent<Rigidbody2D>();

        player = GameObject.FindWithTag("Player").transform;

        // Waiting as initial state
        StartWaiting();
    }

    private void Update()
    {
        stateTimer -= Time.deltaTime;

        // Swapping from waiting to dashing based on timer
        switch (currentState)
        {
            case DashState.WAITING:

                if (stateTimer <= 0f)
                {
                    StartDash();
                }

                break;
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        switch (currentState)
        {
            case DashState.WAITING:
                rb.linearVelocity = knockbackVelocity;
                break;

            // Ensuring that the knockback takes the dash into account
            case DashState.DASHING:
                rb.linearVelocity = dashDirection * dashSpeed + knockbackVelocity;
                break;
        }
    }

    private void StartWaiting()
    {
        // During this time the enemy does not move
        // Still deals damage if you run into them
        currentState = DashState.WAITING;
        stateTimer = pauseTime;

        rb.linearVelocity = Vector2.zero;
    }

    private void StartDash()
    {
        currentState = DashState.DASHING;

        dashDirection = GetPlayerDirection();
    }

    // Getting a randomized direction that the enemy will dash into
    private Vector2 GetRandomCardinalDirection()
    {
        int dir = Random.Range(0, 4);

        switch (dir)
        {
            case 0:
                return Vector2.up;

            case 1:
                return Vector2.down;

            case 2:
                return Vector2.left;

            default:
                return Vector2.right;
        }
    }

    private Vector2 GetPlayerDirection()
    {
        Vector2 toPlayer = (player.position - transform.position).normalized;

        return toPlayer; // Seeing if direct player pursuit is more interesting
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Stop dash upon hitting a wall
        if (!collision.gameObject.CompareTag("Player"))
        {
            if (currentState == DashState.DASHING)
            {
                StartWaiting();
            }
        }
        else
        {
            Player player = collision.gameObject.GetComponent<Player>();

            if (player != null)
            {
                Vector2 hitDirection =
                    (player.transform.position - transform.position).normalized;

                player.TakeDamage(enemyDamage, hitDirection, 12f);
            }
        }
    }

    protected override void Die()
    {
        parentRoom.OnEnemyDeath();

        Score.Instance.UpdateScore(enemyDeathScore);

        Destroy(gameObject);
    }
}