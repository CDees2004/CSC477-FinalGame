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
        [SerializeField] private float minSpawnTime = 2.0f;
        [SerializeField] private float maxSpawnTime = 6.0f;

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
            // Safety check so coroutine doesn't die instantly
            if (Management_Rooms.Instance == null ||
                Management_Rooms.Instance.CurrentRoom == null)
                yield break;

            while (!Management_Rooms.Instance.CurrentRoom.roomCleared)
            {
                float waitTime = Random.Range(minSpawnTime, maxSpawnTime);
                yield return new WaitForSeconds(waitTime);

                SpawnEnemy(basicEnemyPrefab);
            }
        }

        private void SpawnEnemy(GameObject prefab)
        {
            if (prefab == null) return;

            if (parentRoom != null && parentRoom.enemiesAlive >= 20)
                return;

            Vector2 dir = Random.insideUnitCircle.normalized;
            Vector3 spawnPos = transform.position + (Vector3)(dir * spawnDistance);

            if (Physics2D.OverlapCircle(spawnPos, 0.3f) != null)
                return;

            GameObject enemy = Instantiate(prefab, spawnPos, Quaternion.identity);

            // Ensuring the spawned enemies have their parent room assigned
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.SetParentRoom(parentRoom);
            }

            Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.AddForce(dir * spawningLaunchForce, ForceMode2D.Impulse);
            }

            // Counting the spawned enemies towards room completion
            //parentRoom.AddEnemy();
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

                parentRoom.OnEnemyDeath();

                Score.Instance.UpdateScore(enemyDeathScore);
            }
            Destroy(gameObject);
        }
    }
}