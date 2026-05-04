using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Room types for selection pools 
public enum DoorDirection
{
    LEFT,
    RIGHT,
    UP,
    DOWN,
};

public class Management_Rooms : MonoBehaviour
{
    public static Management_Rooms Instance;

    private Dictionary<int, DoorDirection> DoorDirectionDictionary = new();

    private Dictionary<int, Transform> roomDictionary = new();
    private Dictionary<int, AudioClip> roomAudioDictionary = new();
    private Camera mainCamera;
    private AudioSource audioSource;
    public int startingRoomID = 1; // Default starting room ID

    public int clearedRooms;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            mainCamera = Camera.main; // Get main camera reference
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.loop = true;
            audioSource.playOnAwake = false;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        // Ensure starting room audio plays when the game begins
        ChangeRoomAudio(startingRoomID);
    }

    public void RegisterRoom(int roomID, Transform roomTransform, AudioClip roomAudio, DoorDirection type)
    {
        if (!roomDictionary.ContainsKey(roomID))
        {
            roomDictionary.Add(roomID, roomTransform);
            roomAudioDictionary.Add(roomID, roomAudio);
            DoorDirectionDictionary.Add(roomID, type);
        }
    }

    public Transform GetRoomCenter(int roomID)
    {
        return roomDictionary.TryGetValue(roomID, out Transform roomTransform) ? roomTransform : null;
    }

    public void MoveCameraToRoom(int roomID)
    {
        if (roomDictionary.TryGetValue(roomID, out Transform roomTransform))
        {
            mainCamera.transform.position = new Vector3(roomTransform.position.x, roomTransform.position.y, mainCamera.transform.position.z);
            ChangeRoomAudio(roomID);
        }
    }

    private void ChangeRoomAudio(int roomID)
    {
        if (roomAudioDictionary.TryGetValue(roomID, out AudioClip newAudio))
        {
            if (audioSource.clip != newAudio)
            {
                audioSource.clip = newAudio;
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Stop(); // Stop audio if no specific clip for this room
        }
    }

    // Given a type of room, chooses random room from pool of valid selections
    // Returns the room's ID
    public int SelectRoom(DoorDirection type)
    {
        List<int> validRooms = new();

        foreach (var pair in DoorDirectionDictionary)
        {
            if (pair.Value == type) validRooms.Add(pair.Key);
        }

        return validRooms[Random.Range(0, validRooms.Count)];
    }
}
