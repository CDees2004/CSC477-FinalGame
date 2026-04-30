using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public Image itemCollectedImage; // Assign in the Inspector
    public float animationDuration = 1f; // Time for animation

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayItemCollectedAnimation(Sprite itemIcon)
    {
        StartCoroutine(ShowCollectedItem(itemIcon));
    }

    private IEnumerator ShowCollectedItem(Sprite itemIcon)
    {
        itemCollectedImage.sprite = itemIcon;
        itemCollectedImage.gameObject.SetActive(true);

        // Animate (fade in & move up)
        float elapsedTime = 0;
        Vector3 startPosition = itemCollectedImage.transform.position;
        Vector3 endPosition = startPosition + new Vector3(0, 50, 0); // Move up

        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = elapsedTime / animationDuration;
            itemCollectedImage.transform.position = Vector3.Lerp(startPosition, endPosition, alpha);
            itemCollectedImage.color = new Color(1, 1, 1, alpha); // Fade in
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        // Fade out
        elapsedTime = 0;
        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = 1 - (elapsedTime / animationDuration);
            itemCollectedImage.color = new Color(1, 1, 1, alpha); // Fade out
            yield return null;
        }

        itemCollectedImage.gameObject.SetActive(false);
    }
}
