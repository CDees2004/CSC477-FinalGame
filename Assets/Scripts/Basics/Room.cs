using UnityEngine;

public class Room : MonoBehaviour
{
    public int roomID; // Assign in Inspector
    public AudioClip roomAudio; // Assign in Inspector

    private void Start()
    {
        Management_Rooms.Instance.RegisterRoom(roomID, transform, roomAudio);
    }
}
