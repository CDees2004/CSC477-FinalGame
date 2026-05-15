using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy_Basic : Enemy
{

    // set in inspector
    public float movementSpeed;
    public float minRedirDist;


    // set in script
    private Vector2 idleDir;
    private Transform player;

    public ParticleSystem deathParticles;


    protected override void Awake()
    {
        // Get the enemies room ID so that death updates the room
        base.Awake();

        // Overwriting the parent class fields for unique Enemy stats
        enemyIdentifier = "Basic";
        enemyHealth = 100.0f;
        enemyDamage = 10.0f;
        enemyDeathScore = 100;
    }

    protected override void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Gets the parent room and adds the Enemy to it
        base.Start();

        idleDir = Random.insideUnitCircle.normalized;

        movementSpeed = 0.5f;
        minRedirDist = 1f;

        player = GameObject.FindWithTag("Player").transform;

        enemyShader = GetComponent<EnemyShader>();
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

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (idle)
        {
            rb.linearVelocity = idleDir * movementSpeed + knockbackVelocity;
        }
        else
        {
            Vector2 dir = (player.position - transform.position).normalized;

            rb.linearVelocity = dir * movementSpeed + knockbackVelocity;
        }
    }

    private void OnCollisionEnter2D(Collision2D c){
        if (!c.gameObject.CompareTag("Player")){
            idleDir = Random.insideUnitCircle.normalized; // Dangerous line
        }
        else
        {
            Player player = c.gameObject.GetComponent<Player>();
            if (player != null)
            {
                Vector2 hitDirection = (player.transform.position - transform.position).normalized;

                player.TakeDamage(enemyDamage, hitDirection, 12.0f);
            }
        }

    }


    public override void DetectPlayer(){
        idle = false;
        movementSpeed = 1f;
    }

    protected override void Die()
    {
        if (parentRoom == null)
        {
            Debug.LogError($"{gameObject.name} has no parent room assigned.");
        }
        else
        {
            if (enemyShader != null)
                enemyShader.PlayHitFlash();
            
            // explosion death particles
            Instantiate(deathParticles, transform.position, Quaternion.identity);
            parentRoom.OnEnemyDeath();

            Score.Instance.UpdateScore(enemyDeathScore);
        }

        Destroy(gameObject);
    }
}
