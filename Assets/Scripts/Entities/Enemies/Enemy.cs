using System.Drawing;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    protected string enemyIdentifier;
    protected float enemyHealth; 
    protected float enemyDamage;
    protected int enemyDeathScore;
    protected bool idle;
    protected Room parentRoom; // Each enemy must belong to a room
    protected EnemyShader enemyShader;
    protected Rigidbody2D rb;
    protected Vector2 knockbackVelocity;

    [SerializeField] protected float knockbackRecoverySpeed = 8.0f;

    private const bool DEBUG = true;

    protected virtual void Awake()
    {
        parentRoom = GetComponentInParent<Room>();
    }

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (parentRoom == null) Debug.LogError($"{gameObject.name} has no parent room.");
        else
            parentRoom.AddEnemy();

        enemyShader = GetComponent<EnemyShader>();

        idle = true;
    }

    protected virtual void FixedUpdate()
    {
            knockbackVelocity = Vector2.Lerp(
                knockbackVelocity,
                Vector2.zero,
                knockbackRecoverySpeed * Time.fixedDeltaTime
            );
    }

    public virtual void TakeDamage(float incomingDamage, Vector2 hitDirection, float knockbackForce)
    {
        enemyHealth -= incomingDamage;

        knockbackVelocity = hitDirection.normalized * knockbackForce;

        if (enemyShader != null) enemyShader.PlayHitFlash();

        SoundManager.Play(SoundType.ENEMY_HURT);
        
        if (enemyHealth <= 0) Die();

        if (DEBUG) print($"Enemy took {incomingDamage} damage.");
    }

    protected virtual void Die()
    {
        if (parentRoom == null) Debug.LogError($"{gameObject.name} has no parent room assigned.");
        else
        {
            if (enemyShader != null) enemyShader.PlayHitFlash();
            parentRoom.OnEnemyDeath();
        }

        Destroy(gameObject);
    }
    
    public virtual void DetectPlayer(){
        idle = false;
    }

    public void SetParentRoom(Room room)
    {
        parentRoom = room;
    }
}
