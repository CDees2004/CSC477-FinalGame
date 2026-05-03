using UnityEngine;

public class Room : MonoBehaviour
{
    // Assign in Inspector
    public int roomID; 
    public AudioClip roomAudio;
    public int enemiesAlive;
    

    private void Start()
    {
        Management_Rooms.Instance.RegisterRoom(roomID, transform, roomAudio);
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

    public void ClearRoom()
    {
        Management_Rooms.Instance.clearedRooms++;
    }
}
