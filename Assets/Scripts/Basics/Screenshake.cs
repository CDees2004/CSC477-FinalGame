using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace Assets.Scripts.Basics
{
    public class Screenshake : MonoBehaviour
    {
        public static Screenshake Instance;

        private void Awake()
        {
            Instance = this;
        }

        public IEnumerator Shake(float duration, float strength)
        {
            Vector3 startPos = transform.localPosition;

            float elapsed = 0f;

            while (elapsed < duration)
            {
                // Moving the camera a random amount quickly
                transform.localPosition = startPos + (Vector3)Random.insideUnitCircle * strength;

                elapsed += Time.deltaTime;

                yield return null;
            }

            // Returning the camera after random movement
            transform.localPosition = startPos;
        }
    }
}