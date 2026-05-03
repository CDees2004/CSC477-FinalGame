using UnityEngine;
using System.Collections.Generic; 

public class Room : MonoBehaviour
{
    // Assign in Inspector
    public int roomID; 
    public AudioClip roomAudio;
    public int enemiesAlive;

    private Dictionary<Enemy, int> enemiesPerRoom = new();
    

    private void Start()
    {
        Management_Rooms.Instance.RegisterRoom(roomID, transform, roomAudio);

        // Populating the room with enemies 
    }

    public void AddEnemy()
    {
        enemiesAlive++;
    }

    // Called my enemy instances
    public void RemoveEnemy()
    {
        enemiesAlive--;
    }

    public void CheckClearedRoom()
    {
        if (enemiesAlive == 0) Management_Rooms.Instance.clearedRooms++;
    }
}
