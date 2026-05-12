using System.Collections;
using UnityEngine;

/*
 * This enemy type will spin around and spawn projectiles.
 * The projectiles will be Enemy_Basic objects
 */

namespace Assets.Scripts.Entities.Enemies
{
    public class Enemy_Spawner : Enemy
    {
        private GameObject basicEnemyPrefab; // Used for spawning
        private float rotationSpeed = 100.0f;
        private float minSpawnTime = 1.0f;
        private float maxSpawnTime = 3.0f;


        private Transform player;

        protected override void Awake()
        {
            // Getting roomID so death updates the room
            base.Awake();

            // Overwriting parent class fields for unique stats
            enemyIdentifier = "Projectile";
            enemyHealth = 150.0f;
            enemyDamage = 5.0f; // Reduced touch damage, more on ranged
        }

        protected override void Start()
        {
            // Adding Enemy to parent room
            base.Start();

            player = GameObject.FindWithTag("Player").transform;
        }

        private void Update()
        {
            // Projectile Enemy's custom behavior
            transform.Rotate(Vector3.up * 100.0f * Time.deltaTime);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            // Damaging the player on contact
           if (collision.gameObject.CompareTag("Player"))
            {
                Player player = collision.gameObject.GetComponent<Player>();
                if (player != null)
                    player.TakeDamage(enemyDamage);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                Player player = collision.gameObject.GetComponent<Player>();
                if (player != null)
                {
                    player.TakeDamage(enemyDamage * Time.deltaTime);
                }
            }
        }
    }
}