using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy_Basic : Enemy
{
    // set in script
    private Vector3 idleDir;
    private Transform player; 

    // set in inspector
    public float movementSpeed;
    public float minRedirDist;
    

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
        idleDir = Random.onUnitSphere;
        movementSpeed = 0.5f;
        minRedirDist = 1f;
        player = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        // Our Basic Enemy's custom behavior
        if (idle) {
            this.transform.Translate(idleDir * movementSpeed * Time.deltaTime);
        } else {
            this.transform.position = Vector3.MoveTowards(transform.position, player.position, movementSpeed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D c){
        if (!c.gameObject.CompareTag("Player")){
            idleDir = Random.onUnitSphere;
        }
    }

    public override void DetectPlayer(){
        idle = false;
        movementSpeed = 1f;
    }
}
