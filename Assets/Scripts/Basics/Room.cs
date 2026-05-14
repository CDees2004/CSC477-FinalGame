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

// Setting up system to allow each spawn point to
// have an associated prefab

// Serializable so all fields are in Inspector
[System.Serializable]
public class SpawnPointData
{
    public Transform spawnPoint;
    public GameObject enemyPrefab;
}

public class Room : MonoBehaviour
{
    // Assign in Inspector
    public int roomID;
    public AudioClip roomAudio;
    public Transform playerSpawnPoint;
    public RoomType roomType;
    public bool roomCleared = false;
    public List<GameObject> spawnedEnemies = new();
    public int maxEnemies = 20;

    // Essentially making a tuple via the serialized class
    public SpawnPointData[] spawnPoints;

    // For enabling/disabling the room particles 
    public GameObject particles;
    public int enemiesAlive;
    private const bool DEBUG = false;

    private void Awake()
    {
        // Resetting 
        enemiesAlive = 0;
    }

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
        //if (DEBUG) print($"Added enemy. Enemies alive: {enemiesAlive}.");
    }

    // Called my enemy instances
    public void OnEnemyDeath()
    {
        enemiesAlive--;

        //if (DEBUG) print($"Enemy died. Enemies alive: {enemiesAlive}.");

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
        SoundManager.Play(SoundType.ROOM_CLEAR);
        Management_Rooms.clearedRooms++;
        if (this.roomType == RoomType.ENEMY) Management_Rooms.clearedEnemyRooms++;

        //if (DEBUG) print($"Enemy rooms cleared: {Management_Rooms.clearedEnemyRooms}");

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
        if (enemiesAlive >= maxEnemies) yield break;
        foreach (var point in spawnPoints)
        {
            if (point.spawnPoint == null || point.enemyPrefab == null) continue;

            // Want to scale spawning off round
            for (int i = 0; i <= Management_Rooms.clearedEnemyRooms; i++)
            {
                SpawnEnemy(point.enemyPrefab, point.spawnPoint.position);
            }
            
            yield return new WaitForSeconds(0.2f);
        }
    }

    private void SpawnEnemy(GameObject enemyPrefab, Vector2 spawnPosition)
    {
        // Enemy objs call AddEnemy upon Start() on their own.
        var enemyObj = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        // Setting the proper parent room for the clearance logic to work
        Enemy enemy = enemyObj.GetComponent<Enemy>();
        enemy.SetParentRoom(this);
        
        spawnedEnemies.Add(enemyObj);
    }
}
