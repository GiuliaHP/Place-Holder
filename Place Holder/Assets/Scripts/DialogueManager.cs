using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    [Space(20)]
    public Boolean isHeronDialogue;
    
    [Space(20)]
    [Header("UI Elements")]
    public TextMeshProUGUI characterNameText;
    public TextMeshProUGUI dialogueText;
    public Image characterImage;

    [Space(20)]
    [Header("Dialogue Settings")]
    public float typingSpeed = 0.05f;
    public KeyCode nextDialogueKey = KeyCode.Space;

    [Space(20)]
    [Space(20)]
    [Header("Dialogue Data")]
    public DialogueData dialogueData;

    [Space(20)]
    [Header("Animation Settings")]
    public GameObject animatedObject;
    public string animatorBoolParameter = "IsActive"; // Nom du booléen dans l'Animator
    public string animationStateName = "AnimationState"; // Nom de l'état de l'animation à surveiller
    public PersistentManager persMan;

    private Animator objectAnimator;
    private int currentDialogueIndex = 0;
    private bool isTyping = false;
    private bool isAnimationInProgress = false;

    private void Start()
    {
        if (dialogueData != null && dialogueData.dialogueLines.Length > 0)
        {
            DisplayDialogue(dialogueData.dialogueLines[currentDialogueIndex]);
        }

        if (animatedObject != null)
        {
            objectAnimator = animatedObject.GetComponent<Animator>();
        }
    }

    private void Update()
    {
        // Désactive l'entrée pendant l'animation en cours
        if (Input.GetKeyDown(nextDialogueKey) && !isTyping && !isAnimationInProgress)
        {
            NextDialogue();
        }
    }

    private void DisplayDialogue(DialogueData.DialogueLine dialogueLine)
    {
        characterNameText.text = dialogueLine.characterName;
        characterImage.sprite = dialogueLine.characterSprite;
        StartCoroutine(TypeDialogue(dialogueLine.dialogueText));
    }

    private IEnumerator TypeDialogue(string text)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in text)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    private void NextDialogue()
    {
        currentDialogueIndex++;

        if ((currentDialogueIndex == 2 && objectAnimator != null) && isHeronDialogue)
        {
            objectAnimator.SetBool(animatorBoolParameter, true);

            Debug.Log("Activation de l'objet dans une autre scène.");
            persMan.triggerObjectInOtherScene = true;

            StartCoroutine(WaitForAnimationAndContinue());
        }
        else if (currentDialogueIndex < dialogueData.dialogueLines.Length)
        {
            DisplayDialogue(dialogueData.dialogueLines[currentDialogueIndex]);
        }
        else
        {
            StartCoroutine(EndDialogue());
        }
    }

    private IEnumerator WaitForAnimationAndContinue()
    {
        isAnimationInProgress = true;

        // Attendre le début de l'animation
        while (!objectAnimator.GetCurrentAnimatorStateInfo(0).IsName(animationStateName) || 
               objectAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }

        // Désactiver l'objet après l'animation
        if (animatedObject != null)
        {
            animatedObject.SetActive(false);
        }

        isAnimationInProgress = false;

        // Continuer le dialogue
        if (currentDialogueIndex < dialogueData.dialogueLines.Length)
        {
            DisplayDialogue(dialogueData.dialogueLines[currentDialogueIndex]);
        }
        else
        {
            StartCoroutine(EndDialogue());
        }
    }

    private IEnumerator EndDialogue()
    {
        if (!isHeronDialogue)
        {
            objectAnimator.SetBool(animatorBoolParameter, true);
            yield return new WaitForSeconds(3f);
            persMan.isLapinGone = true;
            animatedObject.gameObject.SetActive(false);
        }
        dialogueText.text = ""; // Réinitialise le texte
        characterNameText.text = ""; // Réinitialise le nom du personnage
        gameObject.SetActive(false);
    }
}
