using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Entities
{
    public class PlayerShader : MonoBehaviour
    {
        private Material playerDamageMaterial;
        private Coroutine flashRoutine;

        private void Start()
        {
            SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();

            if (spriteRenderer != null)
            {
                playerDamageMaterial = spriteRenderer.material;
            }
            else
            {
                Debug.LogError($"No SpriteRenderer found on {gameObject.name} or children objects");
            }
        }

        public void PlayHitFlash(float duration = 0.1f)
        {
            if (flashRoutine != null) StopCoroutine(flashRoutine);
            flashRoutine = StartCoroutine(FlashCoroutine(duration));
        }

        private IEnumerator FlashCoroutine(float duration)
        {
            if (playerDamageMaterial == null) yield break;

            playerDamageMaterial.SetFloat("_FlashAmount", 1.0f);

            yield return new WaitForSeconds(duration);

            playerDamageMaterial.SetFloat("_FlashAmount", 0.0f);

            flashRoutine = null;
        }
    }
}