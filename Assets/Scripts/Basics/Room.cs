using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public enum RoomType
{
    ENEMY,
    SHOP,
    HEALING,
    STARTING,
}

public class Room : MonoBehaviour
{
    // Assign in Inspector
    public int roomID;
    public AudioClip roomAudio;
    public GameObject enemyPrefab;
    public Transform[] spawnPoints; // Will grab obj's position.
    public Transform playerSpawnPoint;
    public RoomType roomType;
    // For enabling/disabling the room particles 


    public GameObject particles;

    private int enemiesAlive;
    private bool roomCleared = false;
    private List<GameObject> spawnedEnemies = new();

    private const bool DEBUG = true;


    private void Start()
    {
        Management_Rooms.Instance.RegisterRoom(roomID, transform, roomAudio, roomType);
        StartCoroutine(SpawnEnemiesRoutine());
        if (roomType == RoomType.STARTING)
        {
            particles.SetActive(true);
            roomCleared = true;
        }
        else
        {
            particles.SetActive(false);
        }

    }

    public void AddEnemy()
    {
        enemiesAlive++;
        if (DEBUG) print($"Enemies: {enemiesAlive}.");
    }

    // Called my enemy instances
    public void OnEnemyDeath()
    {
        enemiesAlive--;

        if (enemiesAlive <= 0 && !roomCleared)
        {
            roomCleared = true;
            OnRoomCleared();
        }
    }

    private void OnRoomCleared()
    {
        print($"Room {roomID} cleared.");

        // Detroying all enemies if they were not already
        // This extra bit is in case ForceClearRoom was called
        foreach (var enemy in spawnedEnemies)
        {
            if (enemy != null) Destroy(enemy);
        }

        spawnedEnemies.Clear();

        // Indicating you can now go through the doors
        particles.SetActive(true);
    }

    // For cheat codes 
    public void ForceClearRoom()
    {
        if (roomCleared) return;

        roomCleared = true;
        enemiesAlive = 0;

        OnRoomCleared();
    }

    public void ResetRoom()
    {
        enemiesAlive = 0;
        roomCleared = false;

        if (roomType == RoomType.STARTING)
        {
            particles.SetActive(true);
            roomCleared = true;
            return;
        }

        particles.SetActive(false);

        // Resetting enemies
        foreach (var enemy in spawnedEnemies)
        {
            if (enemy != null) Destroy(enemy);
        }

        spawnedEnemies.Clear();

        StopAllCoroutines();
        StartCoroutine(SpawnEnemiesRoutine());
    }

    // ------ Handling Enemy spawning -----
    IEnumerator SpawnEnemiesRoutine()
    {
        foreach (var point in spawnPoints)
        {
            if (DEBUG) print("Enemy spawned.");
            SpawnEnemy(point.position);
            yield return new WaitForSeconds(0.2f);
        }
    }

    private void SpawnEnemy(Vector2 spawnPosition)
    {
        // Enemy objs call AddEnemy upon Start() on their own.
        var enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        spawnedEnemies.Add(enemy);
    }
}
