using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    protected string enemyIdentifier;
    protected float enemyHealth; 
    protected float enemyDamage;
    protected bool idle;

    // Each enemy must belong to a room
    private Room parentRoom;

    private const bool DEBUG = true;

    protected virtual void Awake()
    {
        parentRoom = GetComponentInParent<Room>();
    }

    protected virtual void Start()
    {
        parentRoom = GetComponentInParent<Room>();
        // Preventing crashing on null with Null-Conditional.
        parentRoom?.AddEnemy();

        idle = true;
    }

    public virtual void TakeDamage(float incomingDamage)
    {
        enemyHealth -= incomingDamage;
        if (enemyHealth <= 0) Die();
        if (DEBUG) print($"Enemy took {incomingDamage} damage.");
    }

    protected virtual void Die()
    {
        if (parentRoom == null) Debug.LogError($"{gameObject.name} has no parent room assigned.");
        else 
            parentRoom.OnEnemyDeath();

        Destroy(gameObject);
    }
    
    public virtual void DetectPlayer(){
        idle = false;
    }
}
