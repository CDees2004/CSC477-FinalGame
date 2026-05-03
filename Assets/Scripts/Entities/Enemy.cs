using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    protected string enemyIdentifier;
    protected float enemyHealth; 
    protected float enemyDamage; 

    private void Start()
    {

    }

    private void Update()
    {

    }

    protected virtual void TakeDamage(float enemyHealth)
    {

    }

    protected virtual void Die()
    {

    }
}
