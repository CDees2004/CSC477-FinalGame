using HighScore; 
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro; // Add this to use Dictionary

public class Management_Rooms : MonoBehaviour
{
    public static Management_Rooms Instance;

    private Dictionary<int, Transform> roomDictionary = new Dictionary<int, Transform>();
    private Dictionary<int, AudioClip> roomAudioDictionary = new Dictionary<int, AudioClip>();
    private Camera mainCamera;
    private AudioSource audioSource;
    public int startingRoomID = 1; // Default starting room ID

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

    public void RegisterRoom(int roomID, Transform roomTransform, AudioClip roomAudio)
    {
        if (!roomDictionary.ContainsKey(roomID))
        {
            roomDictionary.Add(roomID, roomTransform);
            roomAudioDictionary.Add(roomID, roomAudio);
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

    // Selecting a random room from the appropriate rooms for 
    // each door transition 
    public void SelectRoom()
    {

    }
}
