using System.Collections;
using UnityEngine;
using Unity.Cinemachine;
using DG.Tweening;

public class CameraTrigger : MonoBehaviour
{
    [Header("Trigger Settings")]
    public bool isADialogueTrigger;
    public bool isAnAnimationTrigger;
    [Space(20)]
    public CinemachineCamera currentCamera;
    public CinemachineCamera newCamera;
    public Transform player;
    public CanvasGroup infoCanvas;
    public Animator playerAnimator;

    private bool isPlayerInZone = false;
    private bool hasDialogueShown = false;
    private bool isAnimationFinished = false;
    private float playerCharacterControllerCenter;
    private MoveToTarget moveJump;
    
    
    [Space(40)]
    
    [HideInInspector, SerializeField]
    public CanvasGroup dialogueCanvas;
    [HideInInspector, SerializeField]
    public CinemachineCamera secondCamera;


    private void Start()
    {
        // Initialisation des Canvas
        if (infoCanvas != null)
        {
            infoCanvas.alpha = 0f;
            infoCanvas.transform.localScale = Vector3.zero;
            infoCanvas.gameObject.SetActive(false);
        }

        if (dialogueCanvas != null)
        {
            dialogueCanvas.alpha = 0f;
            dialogueCanvas.transform.localScale = Vector3.zero;
            dialogueCanvas.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = true;

            if (infoCanvas != null)
            {
                infoCanvas.gameObject.SetActive(true);
                ShowCanvas(infoCanvas);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = false;

            if (infoCanvas != null)
            {
                HideCanvas(infoCanvas);
            }

            if (dialogueCanvas != null)
            {
                HideCanvas(dialogueCanvas);
                hasDialogueShown = false;
            }

            if (currentCamera != null && newCamera != null)
            {
                newCamera.gameObject.SetActive(false);
                currentCamera.gameObject.SetActive(true);
            }
        }
    }

    private void Update()
    {
        if (isPlayerInZone && Input.GetKeyDown(KeyCode.Space))
        {
            ChangeCamera();

            if (isADialogueTrigger && !hasDialogueShown && dialogueCanvas != null)
            {
                ShowCanvas(dialogueCanvas);
                hasDialogueShown = true;
            }
            else if (isAnAnimationTrigger)
            {
                TriggerAnimation();
            }
        }
    }

    private void ChangeCamera()
    {
        if (currentCamera != null)
        {
            currentCamera.gameObject.SetActive(false);
        }

        if (newCamera != null)
        {
            newCamera.gameObject.SetActive(true);
            newCamera.Follow = player;
            newCamera.LookAt = player;
        }

        if (infoCanvas != null)
        {
            HideCanvas(infoCanvas);
        }

        if (isAnimationFinished)
        {
            secondCamera.gameObject.SetActive(true);
            newCamera.gameObject.SetActive(false);
        }
    }

    private void ShowCanvas(CanvasGroup canvas)
    {
        if (canvas != null)
        {
            canvas.transform.localScale = Vector3.zero;
            canvas.alpha = 0f;

            canvas.gameObject.SetActive(true);

            canvas.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
            canvas.DOFade(1f, 0.5f);
        }
    }

    private void HideCanvas(CanvasGroup canvas)
    {
        if (canvas != null)
        {
            canvas.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack);
            canvas.DOFade(0f, 0.5f).OnComplete(() =>
            {
                canvas.gameObject.SetActive(false);
            });
        }
    }

    private void TriggerAnimation()
    {
        playerCharacterControllerCenter = player.GetComponent<CharacterController>().center.y;
        playerCharacterControllerCenter = 2.5f;
        StartCoroutine(TriggerAnimationWithDelay());
    }

    private IEnumerator TriggerAnimationWithDelay()
    {
        
        
        yield return new WaitForSeconds(1f);

        if (playerAnimator != null)
        {
            playerAnimator.SetBool("isWalking", false);
            playerAnimator.SetBool("isRunning", false);
            playerAnimator.SetBool("isGrounded", false);
            playerAnimator.SetBool("Jump", true);
        }
        moveJump = player.GetComponent<MoveToTarget>();
        player.GetComponent<PlayerController>().pauseBool = true;
        moveJump.enabled = true;

        yield return new WaitForSeconds(0.7f);

        isAnimationFinished = true;
        ChangeCamera();
    }
}
