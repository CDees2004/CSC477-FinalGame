using UnityEngine;

public class SortingOrderController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private float fadeAlpha = 0.5f;  // Transparency level when behind (0.5 = 50% transparent)
    private float fullAlpha = 1f;    // Full visibility

    [Header("Sorting Order Settings")]
    public int baseSortingOrder = 100;  
    public float sortingOffset = 0f;    

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    void Update()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.sortingOrder = baseSortingOrder - Mathf.RoundToInt(transform.position.y * 100) + (int)sortingOffset;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SetTransparency(fadeAlpha); // Fade when player is behind
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SetTransparency(fullAlpha); // Restore when player moves in front
        }
    }

    private void SetTransparency(float alphaValue)
    {
        if (spriteRenderer != null)
        {
            Color newColor = spriteRenderer.color;
            newColor.a = alphaValue;
            spriteRenderer.color = newColor;
        }
    }
}
