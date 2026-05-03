using UnityEngine;
using System.Collections.Generic;
using System.Collections;

// Every Room is responsible for its own enemies

public class Room : MonoBehaviour
{
    // Assign in Inspector
    public int roomID;
    public AudioClip roomAudio;
    public GameObject enemyPrefab;
    public Transform[] spawnPoints; // Will grab obj's position.

    private int enemiesAlive;
    private bool roomCleared = false;

    private const bool DEBUG = true;


    private void Start()
    {
        Management_Rooms.Instance.RegisterRoom(roomID, transform, roomAudio);
        StartCoroutine(SpawnEnemiesRoutine());
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

        }
    }

    private void OnRoomCleared()
    {
        print($"Room {roomID} cleared.");
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
    }
}
