using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFade : MonoBehaviour
{
    public static ScreenFade Instance;
    private Image fadeImage;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        fadeImage = GetComponentInChildren<Image>();
        canvasGroup = GetComponent<CanvasGroup>();
        DontDestroyOnLoad(gameObject);
    }

    public void FadeToBlack(float duration)
    {
        StartCoroutine(FadeRoutine(1, duration));
    }

    public void FadeFromBlack(float duration)
    {
        StartCoroutine(FadeRoutine(0, duration));
    }

    private IEnumerator FadeRoutine(float targetAlpha, float duration)
    {
        float startAlpha = canvasGroup.alpha;
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
            yield return null;
        }

        canvasGroup.alpha = targetAlpha;
    }
}
