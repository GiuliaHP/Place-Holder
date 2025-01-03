using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CircleTransition : MonoBehaviour
{
    [Header("Transition Settings")]
    public bool grow = true; // Détermine si le cercle grandit ou rétrécit
    public float transitionDuration = 1.0f; // Durée de l'animation
    public string targetScene = ""; // Nom de la scène à charger (si shrink)

    private RectTransform maskTransform; // Transform du cercle
    private Canvas canvas;

    private void Awake()
    {
        maskTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    private void Start()
    {
        if (grow)
        {
            StartCoroutine(GrowAnimation());
        }
        else
        {
            StartCoroutine(ShrinkAnimation());
        }
    }

    private IEnumerator GrowAnimation()
    {
        Vector2 initialSize = new Vector2(0, 0);
        Vector2 finalSize = new Vector2(Screen.width * 2, Screen.height * 2); // Taille qui couvre l'écran

        float elapsedTime = 0f;

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / transitionDuration;

            // Interpolation linéaire
            maskTransform.sizeDelta = Vector2.Lerp(initialSize, finalSize, t);

            yield return null;
        }

        // S'assurer que le cercle est complètement agrandi
        maskTransform.sizeDelta = finalSize;

        // Désactiver le canvas
        if (canvas != null)
        {
            canvas.gameObject.SetActive(false);
        }
    }

    private IEnumerator ShrinkAnimation()
    {
        Vector2 initialSize = maskTransform.sizeDelta;
        Vector2 finalSize = new Vector2(0, 0);

        float elapsedTime = 0f;

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / transitionDuration;

            // Interpolation linéaire
            maskTransform.sizeDelta = Vector2.Lerp(initialSize, finalSize, t);

            yield return null;
        }

        // S'assurer que le cercle est complètement réduit
        maskTransform.sizeDelta = finalSize;

        // Changer de scène
        if (!string.IsNullOrEmpty(targetScene))
        {
            grow = false;
            SceneManager.LoadScene(targetScene);
        }
    }
}
