using UnityEngine;
using System.Collections;

public class DoorTrigger : MonoBehaviour
{
    public DoorDirection transitionDoorDirection;

    // twinDoor is the key part that needs to be eradicated
    public DoorTrigger twinDoor; // Drag the twin door here manually in the Inspector

    public int targetRoomID; // The room this door leads to
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

        // Check if twinDoor is manually assigned
        if (twinDoor == null)
        {
            Debug.LogError($"Twin door not assigned for {name}. Assign it manually in the Inspector.");
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
        int nextRoomID = Management_Rooms.Instance.SelectRoom(transitionDoorDirection);
        Management_Rooms.Instance.MoveCameraToRoom(nextRoomID);

        // Move the player to the twin door's landing position
        if (twinDoor != null && twinDoor.landingPosition != null)
        {
            player.position = twinDoor.landingPosition.position;
        }
        else
        {
            Debug.LogWarning($"Twin door not set up correctly for {name}");
        }

        // Start fade in
        ScreenFade.Instance.FadeFromBlack(fadeDuration);
        yield return new WaitForSeconds(fadeDuration);

        if (playerController != null) playerController.SetMovementEnabled(true);
        isTransitioning = false;
    }
}
