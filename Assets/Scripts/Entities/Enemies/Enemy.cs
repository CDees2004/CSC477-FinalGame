using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    protected string enemyIdentifier;
    protected float enemyHealth; 
    protected float enemyDamage;
    protected int enemyDeathScore;
    protected bool idle;
    protected Room parentRoom; // Each enemy must belong to a room

    private EnemyShader enemyShader;

    private const bool DEBUG = true;

    // Making an instance of our score for updates 
    Score score;

    protected virtual void Awake()
    {
        parentRoom = GetComponentInParent<Room>();
    }

    protected virtual void Start()
    {
        if (parentRoom == null) Debug.LogError($"{gameObject.name} has no parent room.");
        else
            parentRoom.AddEnemy();

        enemyShader = GetComponent<EnemyShader>();

        idle = true;
    }

    public virtual void TakeDamage(float incomingDamage)
    {
        enemyHealth -= incomingDamage;

        if (enemyShader != null) enemyShader.PlayHitFlash();
        
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
            // Updating the player's score upon enemy defeat
            score.UpdateScore(enemyDeathScore);
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
