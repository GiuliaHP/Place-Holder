using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ButtonInteraction : MonoBehaviour
{
    [Space(5)]
    [Header("Animation Durations")]
    public float hoverDuration = 0.2f;
    public float clickDuration = 0.1f;
    public float idleDuration = 0.3f;
    [Space(5)]
    [Header("Animation Settings")]
    public Vector3 hoverScale = new Vector3(1.1f, 1.1f, 1f);
    public Vector3 clickScale = new Vector3(0.95f, 0.95f, 1f);
    public Vector3 idleScale = Vector3.one;
    [Space(5)]
    public Color idleColor = Color.white;
    public Color hoverColor = Color.yellow;
    public Color clickColor = Color.green;
    [Space(5)]
    public Canvas canvasToHide, canvasToShow;
    public PlayerController playerScript;
    public Canvas transitionCanvas;
    [Space(5)]
    private Button[] buttons; // Tous les boutons enfants

    void Start()
    {
        buttons = GetComponentsInChildren<Button>();

        foreach (Button btn in buttons)
        {
            btn.onClick.AddListener(() => OnClick(btn));

            EventTrigger trigger = btn.gameObject.AddComponent<EventTrigger>();

            // PointerEnter
            EventTrigger.Entry entryEnter = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerEnter
            };
            entryEnter.callback.AddListener((eventData) => OnMouseEnter(btn));
            trigger.triggers.Add(entryEnter);

            // PointerExit
            EventTrigger.Entry entryExit = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerExit
            };
            entryExit.callback.AddListener((eventData) => OnMouseExit(btn));
            trigger.triggers.Add(entryExit);
        }


        ResetToIdle();
    }

    public void OnMouseEnter(Button button)
    {
        AnimateHover(button);
    }

    public void OnMouseExit(Button button)
    {
        ResetToIdle(button);
    }

    public void OnClick(Button clickedButton)
    {
        AnimateClick(clickedButton);

        Debug.Log($"Bouton cliqué : {clickedButton.name}");

        // Exemple d'action pour le bouton Play
        if (clickedButton.name == "PlayButton")
        {
            Play();
        }
    }

    private void ResetToIdle()
    {
        foreach (Button btn in buttons)
        {
            ResetToIdle(btn);
        }
    }

    private void ResetToIdle(Button button)
    {
        RectTransform rect = button.GetComponent<RectTransform>();
        Image img = button.GetComponent<Image>();

        rect.DOScale(idleScale, idleDuration);
        if (img != null)
            img.DOColor(idleColor, idleDuration);
    }

    private void AnimateHover(Button button)
    {
        RectTransform rect = button.GetComponent<RectTransform>();
        Image img = button.GetComponent<Image>();

        rect.DOScale(hoverScale, hoverDuration);
        if (img != null)
            img.DOColor(hoverColor, hoverDuration);
    }

    private void AnimateClick(Button button)
    {
        RectTransform rect = button.GetComponent<RectTransform>();
        Image img = button.GetComponent<Image>();

        rect.DOScale(clickScale, clickDuration).OnComplete(() =>
        {
            rect.DOScale(hoverScale, hoverDuration);
        });

        if (img != null)
            img.DOColor(clickColor, clickDuration).OnComplete(() =>
            {
                img.DOColor(hoverColor, hoverDuration);
            });
    }

    public void Play()
    {
        if (transitionCanvas != null)
        {
            transitionCanvas.gameObject.SetActive(true);

            CircleTransition transitionScript = transitionCanvas.GetComponentInChildren<CircleTransition>();

            if (transitionScript != null)
            {
                transitionScript.grow = false; // Réduction du cercle
                transitionScript.StartCoroutine("Start");
            }
        }
        else
        {
            SceneManager.LoadScene("Lair");
        }
    }

    public void ShowAndHideCanvas()
    {

        if (playerScript != null)
        {
            playerScript.pauseBool = false;
        }
        
        if (canvasToShow != null)
        {
            canvasToShow.gameObject.SetActive(true);
        }

        if (canvasToHide != null)
        {
            canvasToHide.gameObject.SetActive(false);
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
}
