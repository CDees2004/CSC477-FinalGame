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
    public bool roomCleared = false;
    public List<GameObject> spawnedEnemies = new();

    // For enabling/disabling the room particles 
    public GameObject particles;
    private int enemiesAlive;
    private const bool DEBUG = true;


    private void Start()
    {
        Management_Rooms.Instance.RegisterRoom(roomID, transform, roomAudio, roomType);

        // Spawning enemies only in the current room
        // Called upon room entry ONCE
        if (this == Management_Rooms.Instance.CurrentRoom)
        {
            StartCoroutine(SpawnEnemiesRoutine());
        }

        if (roomType == RoomType.STARTING)
        {
            roomCleared = true;

            particles.SetActive(true);
        }
        else
        {
            particles.SetActive(false);
        }

    }

    // Called upon Enemy spawn by the enemy itself
    public void AddEnemy()
    {
        enemiesAlive++;
        if (DEBUG) print($"Added enemy. Enemies alive: {enemiesAlive}.");
    }

    // Called my enemy instances
    public void OnEnemyDeath()
    {
        enemiesAlive--;

        if (DEBUG) print($"Enemy died. Enemies alive: {enemiesAlive}.");

        if (enemiesAlive <= 0 && !roomCleared)
        {
            roomCleared = true;
            OnRoomCleared();
        }
    }

    private void OnRoomCleared()
    {
        // Detroying all enemies if they were not already
        // This extra bit is in case ForceClearRoom was called
        foreach (var enemy in spawnedEnemies)
        {
            if (enemy != null) Destroy(enemy);
        }

        spawnedEnemies.Clear();

        DoorTrigger[] allDoors = FindObjectsByType<DoorTrigger>(FindObjectsSortMode.None);

        foreach (var door in allDoors)
        {
            if (door.parentRoom == this)
            {
                door.UnlockDoor();
            }
        }

        // Indicating you can now go through the doors
        particles.SetActive(true);
        Management_Rooms.clearedRooms++;
        if (this.roomType == RoomType.ENEMY) Management_Rooms.clearedEnemyRooms++;

        if (DEBUG) print($"Enemy rooms cleared: {Management_Rooms.clearedEnemyRooms}");

        // Checking the win condition after every room clearance
        Management_Game.Instance.CheckWinCondition();
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

        // Reset door unlock status
        foreach (var door in FindObjectsByType<DoorTrigger>(FindObjectsSortMode.None))
        {
            if (door.parentRoom == this)
            {
                door.LockDoor();
            }
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
            // Want to scale spawning off round
            for (int i = 0; i <= Management_Rooms.clearedEnemyRooms; i++)
            {
                SpawnEnemy(point.position);
            }
            
            yield return new WaitForSeconds(0.2f);
        }
    }

    private void SpawnEnemy(Vector2 spawnPosition)
    {
        // Enemy objs call AddEnemy upon Start() on their own.
        var enemyObj = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        // Setting the proper parent room for the clearance logic to work
        Enemy enemy = enemyObj.GetComponent<Enemy>();
        enemy.SetParentRoom(this);
        
        spawnedEnemies.Add(enemyObj);
    }
}
