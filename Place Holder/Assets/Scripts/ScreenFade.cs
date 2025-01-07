using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScreenFade : MonoBehaviour
{
    public float fadeDuration = 1f;

    public void FadeToBlack()
    {
        StartCoroutine(FadeCanvas(1f)); // Fait un fondu vers noir
    }

    public void FadeFromBlack()
    {
        StartCoroutine(FadeCanvas(0f)); // Fait un fondu vers transparent
    }

    private IEnumerator FadeCanvas(float targetAlpha)
    {
        float startAlpha = gameObject.GetComponent<CanvasGroup>().alpha;
        float timeElapsed = 0f;

        while (timeElapsed < fadeDuration)
        {
            gameObject.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(startAlpha, targetAlpha, timeElapsed / fadeDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        gameObject.GetComponent<CanvasGroup>().alpha = targetAlpha; // S'assure d'atteindre la valeur cible

        if (gameObject.GetComponent<CanvasGroup>().alpha >= 1)
        {
            yield return new WaitForSeconds(3f);
            SceneManager.LoadScene("Menu");
        }
    }

    void Start()
    {
        gameObject.GetComponent<CanvasGroup>().alpha = 0f; // Assure que l'alpha commence à 0
    }
}