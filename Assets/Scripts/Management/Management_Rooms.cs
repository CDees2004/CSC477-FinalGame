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
    STARTING,
};

public class Management_Rooms : MonoBehaviour
{
    public static Management_Rooms Instance;

    private Dictionary<string, int> doorToRoomMap = new();

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
        GenerateRun();
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

    // Door -> Room selection is done ONCE at the start of each run
    // and then kept consistent for the whole run.
    public void GenerateRun()
    {
        doorToRoomMap.Clear();

        DoorTrigger[] allDoors = FindObjectsByType<DoorTrigger>(FindObjectsSortMode.None);

        foreach (var door in allDoors)
        {
            int selectedRoom = SelectRoom(door.transitionDoorDirection);

            doorToRoomMap[door.doorID] = selectedRoom;
        }
        print($"Found {allDoors.Length} doors.");

        foreach (var pair in doorToRoomMap)
        {
            print($"{pair.Key} -> Room {pair.Value}");
        }
    }

    // Getter of door info for the DoorTrigger logic
    public int GetRoomForDoor(string doorID)
    {
        if (doorToRoomMap.TryGetValue(doorID, out int roomID)) return roomID;

        Debug.LogError($"No room mapped for door {doorID}");
        return startingRoomID;
    }
}
