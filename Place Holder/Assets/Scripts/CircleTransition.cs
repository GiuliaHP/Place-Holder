using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CircleTransition : MonoBehaviour
{
    [Header("Transition Settings")]
    public bool grow = true; // Détermine si le cercle grandit ou rétrécit
    public float transitionDuration = 1.0f; // Durée de l'animation
    public string targetScene = ""; // Nom de la scène à charger (si shrink)

    [Header("Audio Settings")]
    public List<AudioSource> audioSources; // Liste des AudioSources
    public bool fadeAudio = true; // Activer ou non le fondu audio

    private RectTransform maskTransform; // Transform du cercle
    private Canvas canvas;
    private Dictionary<AudioSource, float> initialVolumes = new Dictionary<AudioSource, float>();

    private void Awake()
    {
        maskTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();

        // Enregistrer les volumes initiaux des AudioSources
        if (fadeAudio && audioSources != null)
        {
            foreach (var audioSource in audioSources)
            {
                if (audioSource != null)
                {
                    initialVolumes[audioSource] = audioSource.volume;
                }
            }
        }
    }

    public void Start()
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

            // Interpolation linéaire pour la taille
            maskTransform.sizeDelta = Vector2.Lerp(initialSize, finalSize, t);

            // Fondu audio pour chaque source
            if (fadeAudio)
            {
                foreach (var audioSource in audioSources)
                {
                    if (audioSource != null && initialVolumes.ContainsKey(audioSource))
                    {
                        audioSource.volume = Mathf.Lerp(0f, initialVolumes[audioSource], t);
                    }
                }
            }

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

            // Interpolation linéaire pour la taille
            maskTransform.sizeDelta = Vector2.Lerp(initialSize, finalSize, t);

            // Fondu audio pour chaque source
            if (fadeAudio)
            {
                foreach (var audioSource in audioSources)
                {
                    if (audioSource != null && initialVolumes.ContainsKey(audioSource))
                    {
                        audioSource.volume = Mathf.Lerp(initialVolumes[audioSource], 0f, t);
                    }
                }
            }

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
