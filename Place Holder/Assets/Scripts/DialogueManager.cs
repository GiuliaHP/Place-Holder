using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI characterNameText;
    public TextMeshProUGUI dialogueText;
    public Image characterImage;

    [Space(20)]
    [Header("Dialogue Settings")]
    public float typingSpeed = 0.05f;
    public KeyCode nextDialogueKey = KeyCode.Space;

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
        if (Input.GetKeyDown(nextDialogueKey) && !isTyping)
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

        // Si on est à la deuxième ligne, activer le booléen
        if (currentDialogueIndex == 2 && objectAnimator != null)
        {
            objectAnimator.SetBool(animatorBoolParameter, true);

            Debug.Log("ACTIVATION OBEJT AUTRE SCENE");
            // Active le booléen global dans le gestionnaire persistant
            persMan.triggerObjectInOtherScene = true;
            
            StartCoroutine(WaitForAnimationAndContinue());
        }
        else if (currentDialogueIndex < dialogueData.dialogueLines.Length)
        {
            DisplayDialogue(dialogueData.dialogueLines[currentDialogueIndex]);
        }
        else
        {
            EndDialogue();
        }
    }

    private IEnumerator WaitForAnimationAndContinue()
    {
        // Attendre que l'animation commence
        while (!objectAnimator.GetCurrentAnimatorStateInfo(0).IsName(animationStateName))
        {
            yield return null;
        }

        // Attendre que l'animation soit terminée
        while (objectAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }

        if (objectAnimator != null)
        {
            
            // Attend la fin de l'animation
            yield return new WaitForSeconds(objectAnimator.GetCurrentAnimatorStateInfo(0).length);

            // Continue le dialogue
            NextDialogue();

            // Désactive l'objet local
            animatedObject.SetActive(false);
        }

        // Continuer le dialogue
        if (currentDialogueIndex < dialogueData.dialogueLines.Length)
        {
            DisplayDialogue(dialogueData.dialogueLines[currentDialogueIndex]);
        }
        else
        {
            EndDialogue();
        }
        
    }

    private void EndDialogue()
    {
        gameObject.SetActive(false);
    }
}
