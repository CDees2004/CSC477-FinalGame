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

    protected virtual void TakeDamage(float incomingDamage)
    {
        enemyHealth -= incomingDamage;
        if (enemyHealth <= 0) Die();
    }

    protected virtual void Die()
    {
        parentRoom?.OnEnemyDeath();
        Destroy(gameObject);
    }
    
    public virtual void DetectPlayer(){
        idle = false;
    }
}
