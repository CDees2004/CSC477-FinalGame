using UnityEngine;

public class Interactable : MonoBehaviour
{
    public string interactableName; // Name of the object (for debugging/UI use)
    public AudioClip interactSound; // Assign sound effect
    private Animator animator;
    private AudioSource audioSource;

    private void Start()
    {
        // Get Animator component attached to this GameObject
        animator = GetComponent<Animator>();

        // Setup audio source
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f; // 3D sound
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Interact();
        }
    }

    public void Interact()
    {
        // Play Interact animation
        if (animator != null)
        {
            animator.SetTrigger("Interact");
        }

        // Play sound effect
        if (interactSound != null)
        {
            audioSource.PlayOneShot(interactSound);
        }
    }
}
