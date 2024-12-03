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
    
    private int currentDialogueIndex = 0;
    private bool isTyping = false;

    private void Start()
    {
        if (dialogueData != null && dialogueData.dialogueLines.Length > 0)
        {
            DisplayDialogue(dialogueData.dialogueLines[currentDialogueIndex]);
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
