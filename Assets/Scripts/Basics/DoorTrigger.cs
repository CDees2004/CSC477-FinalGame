using UnityEngine;
using System.Collections;

public class DoorTrigger : MonoBehaviour
{
    public RoomType targetRoomType;

    public float fadeDuration = 0.5f;
    private bool isTransitioning = false;

    private Player playerController;
    public Transform landingPosition;
    
    [Header("Audio Settings")]
    public AudioClip doorSound; // Sound effect for door transition
    private AudioSource audioSource;

    private void Start()
    {
        playerController = FindFirstObjectByType<Player>();

        // Find landing position
        Transform foundLanding = transform.Find("LandingPosition");
        if (foundLanding != null)
        {
            landingPosition = foundLanding;
        }
        else
        {
            Debug.LogWarning($"No LandingPosition found for door {name}. Defaulting to door position.");
            landingPosition = transform;
        }

        // Setup audio source
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isTransitioning)
        {
            StartCoroutine(TransitionPlayer(other.transform));
        }
    }

    private IEnumerator TransitionPlayer(Transform player)
    {
        isTransitioning = true;
        if (playerController != null) playerController.SetMovementEnabled(false);

        // Play door sound if assigned
        if (doorSound != null)
        {
            audioSource.PlayOneShot(doorSound);
        }

        // Start fade out
        ScreenFade.Instance.FadeToBlack(fadeDuration);
        yield return new WaitForSeconds(fadeDuration + 0.2f);

        // Move the camera to the new room
        int nextRoomID = Management_Rooms.Instance.SelectRoom(targetRoomType);
        Management_Rooms.Instance.MoveCameraToRoom(nextRoomID);

        Transform roomTransform = Management_Rooms.Instance.GetRoomCenter(nextRoomID);
        Room room = roomTransform.GetComponent<Room>();

        // Spawn the player in the room's center ONLY if the spawn was not 
        // given in the Room
        if (room.playerSpawnPoint != null)
        {
            player.position = room.playerSpawnPoint.position;
        }
        else
        {
            player.position = room.transform.position;
        }

        // Start fade in
        ScreenFade.Instance.FadeFromBlack(fadeDuration);
        yield return new WaitForSeconds(fadeDuration);

        if (playerController != null) playerController.SetMovementEnabled(true);
        isTransitioning = false;
    }
}
