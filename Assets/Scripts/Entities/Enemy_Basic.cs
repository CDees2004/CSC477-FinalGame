using UnityEngine;

public class Enemy_Basic : Enemy
{
    protected override void Awake()
    {
        // Get the enemies room ID so that death updates the room
        base.Awake();

        // Overwriting the parent class fields for unique Enemy stats
        enemyIdentifier = "Basic";
        enemyHealth = 100.0f;
        enemyDamage = 10.0f;
    }

    protected override void Start()
    {
        // Gets the parent room and adds the Enemy to it
        base.Start();
    }

    private void Update()
    {
        // Our Basic Enemy's custom behavior
    }
}
