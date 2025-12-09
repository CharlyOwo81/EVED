using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CanvasFader : MonoBehaviour
{
    private Image fadeImage;

    void Awake()
    {
        fadeImage = GetComponentInChildren<Image>(true);
        if (fadeImage == null)
        {
            Debug.LogError("CanvasFader necesita un componente Image hijo para funcionar como fundido.");
        }

        DontDestroyOnLoad(gameObject);
    }

    public IEnumerator Fade(float targetAlpha, float duration)
    {
        if (fadeImage == null) yield break;

        float startAlpha = fadeImage.color.a;
        float timer = 0f;

        fadeImage.gameObject.SetActive(true); 

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, timer / duration);
            Color newColor = fadeImage.color;
            newColor.a = newAlpha;
            fadeImage.color = newColor;
            yield return null;
        }

        Color finalColor = fadeImage.color;
        finalColor.a = targetAlpha;
        fadeImage.color = finalColor;

        if (targetAlpha == 0f)
        {
            fadeImage.gameObject.SetActive(false);
        }
    }
}