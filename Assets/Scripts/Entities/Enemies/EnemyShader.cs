using System.Collections;
using UnityEngine;

public class EnemyShader : MonoBehaviour
{
    private Material enemyDamageMaterial;
    private Coroutine flashRoutine;

    private void Start()
    {
        // Need to search for the sprite renderer since it is on a child object
        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (spriteRenderer != null) enemyDamageMaterial = spriteRenderer.material;
        else
            Debug.LogError($"No sprite renderer found on {gameObject.name} or children objects");
    }

    public void PlayHitFlash(float duration = 0.1f)
    {
        if (flashRoutine != null) StopCoroutine(flashRoutine);
        flashRoutine = StartCoroutine(FlashCoroutine(duration));
    }

    private IEnumerator FlashCoroutine(float duration)
    {
        // Setting flash to full brightness 
        enemyDamageMaterial.SetFloat("_FlashAmount", 0.35f);

        // Waiting for half a second 
        yield return new WaitForSeconds(duration);

        // Go back to normal
        enemyDamageMaterial.SetFloat("_FlashAmount", 0.0f);
    }
}
