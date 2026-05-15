using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace Assets.Scripts.Basics
{
    public class Screenshake : MonoBehaviour
    {
        public static Screenshake Instance;

        private Vector3 originalPosition;
        private Coroutine currentShake; // Preventing multiple shakes at one time

        private bool isShaking = false;


        private void Awake()
        {
            Instance = this;
        }
        
        public void StartShake(float duration, float strength)
        {
            // Preventing multiple shakes at once
            if (isShaking) return;
            // Stopping the existing shake before a new one can start 
            if (currentShake != null)
            {
                originalPosition = transform.localPosition;
                StopCoroutine(currentShake);
                transform.localPosition = originalPosition; // Need this reset to avoid breaking everything
            }

            currentShake = StartCoroutine(Shake(duration, strength));
        }

        public IEnumerator Shake(float duration, float strength)
        {
            Vector3 startPos = transform.localPosition;

            isShaking = true;

            float elapsed = 0f;

            while (elapsed < duration)
            {
                // Moving the camera a random amount quickly
                transform.localPosition = startPos + (Vector3)Random.insideUnitCircle * strength;

                elapsed += Time.deltaTime;

                yield return null;
            }

            isShaking = false;

            // Returning the camera after random movement
            transform.localPosition = startPos;
        }
    }
}