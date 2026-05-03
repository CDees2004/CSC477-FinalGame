using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    protected string enemyIdentifier;
    protected float enemyHealth; 
    protected float enemyDamage;

    // Each enemy must belong to a room
    private Room parentRoom;

    private void Start()
    {
        parentRoom = GetComponentInParent<Room>();
        parentRoom?.AddEnemy();
    }

    protected virtual void TakeDamage(float enemyHealth)
    {

    }

    protected virtual void Die()
    {
        parentRoom?.OnEnemyDeath();
        Destroy(gameObject);
    }
}
