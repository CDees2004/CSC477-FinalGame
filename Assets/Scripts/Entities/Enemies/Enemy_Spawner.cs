using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Entities.Enemies
{
    public class Enemy_Spawner : Enemy
    {
        [Header("Spawning")]
        // Needs the basicEnemy prefab to be able to instantiate it
        [SerializeField] private GameObject basicEnemyPrefab;
        [SerializeField] private float spawnDistance = 1.5f;
        // For shooting the basic enemy away from the spawn location
        [SerializeField] private float spawningLaunchForce = 5.0f;
        [SerializeField] private float minSpawnTime = 1.0f;
        [SerializeField] private float maxSpawnTime = 3.0f;

        [Header("Rotation")]
        // The actual sprite rotation speed as it will spin constantly
        [SerializeField] private float rotationSpeed = 100.0f;

        private Coroutine spawningCoroutine;

        protected override void Awake()
        {
            base.Awake();

            // Setting individual enemy fields
            enemyIdentifier = "Projectile";
            enemyHealth = 200.0f;
            enemyDamage = 5.0f;
            enemyDeathScore = 500;
        }

        protected override void Start()
        {
            base.Start();

            // Start spawning loop
            spawningCoroutine = StartCoroutine(SpawnRoutine());
        }

        private void Update()
        {
            // Spinning around the z-axis
            transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
        }

        private IEnumerator SpawnRoutine()
        {
            while (true)
            {
                float waitTime = Random.Range(minSpawnTime, maxSpawnTime);
                yield return new WaitForSeconds(waitTime);

                SpawnEnemy();
            }
        }

        private void SpawnEnemy()
        {
            if (basicEnemyPrefab == null) return;

            // Random direction around spawner
            Vector2 dir = Random.insideUnitCircle.normalized;
            Vector3 spawnPos = transform.position + (Vector3)(dir * spawnDistance);

            GameObject enemy = Instantiate(basicEnemyPrefab, spawnPos, Quaternion.identity);

            // Launching them outwards
            Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.AddForce(dir * spawningLaunchForce, ForceMode2D.Impulse);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
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

        private void OnDestroy()
        {
            if (spawningCoroutine != null)
                StopCoroutine(spawningCoroutine);
        }
    }
}