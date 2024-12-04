using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ButtonInteraction : MonoBehaviour
{
    [Space (5)]
    [Header("Animation Durations")]
    public float hoverDuration = 0.2f;
    public float clickDuration = 0.1f;
    public float idleDuration = 0.3f;
    [Space (5)]
    [Header("Animation Settings")]
    public Vector3 hoverScale = new Vector3(1.1f, 1.1f, 1f);
    public Vector3 clickScale = new Vector3(0.95f, 0.95f, 1f);
    public Vector3 idleScale = Vector3.one;
    [Space (5)]
    public Color idleColor = Color.white;
    public Color hoverColor = Color.yellow;
    public Color clickColor = Color.green; 
    [Space(5)]
    public String sceneToPlayName = "TEST_Giulia";
    public Canvas canvasToHide, canvasToShow;
    public PlayerController playerScript;
    [Space(5)]
    private Image buttonImage; // Image du bouton
    private bool isSelected = false; // Garde la trace de l'état sélectionné
    private Button button; // Référence au bouton
    private RectTransform rectTransform; // Transform du bouton (pour les animations de taille, par exemple)

    void Start()
    {
        button = gameObject.GetComponent<Button>();
        rectTransform = GetComponent<RectTransform>();
        buttonImage = button.GetComponent<Image>();

        // Associe les événements
        button.onClick.AddListener(OnClick);
        ResetToIdle();
    }

    public void OnMouseEnter()
    {
        if (!isSelected) AnimateHover();
    }

    public void OnMouseExit()
    {
        if (!isSelected) ResetToIdle();
    }

    public void OnClick()
    {
        AnimateClick();
        isSelected = true;

        // Rétablir l'état après un clic
        Invoke(nameof(ResetSelection), clickDuration * 2);
    }

    private void ResetSelection()
    {
        isSelected = false;
        ResetToIdle();
    }

    private void ResetToIdle()
    {
        rectTransform.DOScale(idleScale, idleDuration);
        if (buttonImage != null)
            buttonImage.DOColor(idleColor, idleDuration);
    }

    private void AnimateHover()
    {
        rectTransform.DOScale(hoverScale, hoverDuration);
        if (buttonImage != null)
            buttonImage.DOColor(hoverColor, hoverDuration);
    }

    private void AnimateClick()
    {
        rectTransform.DOScale(clickScale, clickDuration).OnComplete(() =>
        {
            rectTransform.DOScale(hoverScale, hoverDuration); // Revenir à l'état survolé
        });

        if (buttonImage != null)
            buttonImage.DOColor(clickColor, clickDuration).OnComplete(() =>
            {
                buttonImage.DOColor(hoverColor, hoverDuration);
            });
    }

    public void Play()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneToPlayName);
    }

    public void Credits()
    {
        if (canvasToShow != null)
        {
            canvasToShow.gameObject.SetActive(true);
        }

        if (canvasToHide != null)
        {
            canvasToHide.gameObject.SetActive(false);
        }

        if (playerScript != null)
        {
            playerScript.pauseBool = false;
        }
        
    }

    public void Quit()
    {
        Application.Quit();
    }
}
