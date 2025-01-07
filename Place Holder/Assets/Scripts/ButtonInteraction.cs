using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ButtonInteraction : MonoBehaviour
{
    [Header("URL to Open")]
    public string url1 = "https://www.example.com";
    public string url2 = "https://www.example.com";
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
        
        // Récupérer le composant Button et ajouter un listener pour le clic
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OpenLink1);
            button.onClick.AddListener(OpenLink2);
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
                transitionScript.Start();
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
    
    public void OpenLink1()
    {
        if (!string.IsNullOrEmpty(url1))
        {
            Application.OpenURL(url1);
        }
        else
        {
            Debug.LogError("URL est vide ou non définie !");
        }
    }
    
    public void OpenLink2()
    {
        if (!string.IsNullOrEmpty(url2))
        {
            Application.OpenURL(url2);
        }
        else
        {
            Debug.LogError("URL est vide ou non définie !");
        }
    }

}
