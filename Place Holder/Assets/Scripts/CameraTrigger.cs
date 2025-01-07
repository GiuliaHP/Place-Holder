using System;
using System.Collections;
using UnityEngine;
using Unity.Cinemachine;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public static class GameObjectUtils
{
    public static GameObject FindInactiveObjectByName(string objectName)
    {
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (obj.name == objectName) // Vérifie si le nom correspond
            {
                return obj; // Retourne le GameObject correspondant
            }
        }

        Debug.LogWarning($"GameObject with name '{objectName}' not found.");
        return null;
    }
}

public class CameraTrigger : MonoBehaviour
{
    [Space(50)]
    [Header("Trigger Settings")]
    public bool isADialogueTrigger;
    public bool isAnAnimationTrigger;
    
    [Space(50)]
    [Header("Cameras and other parameters")]
    public CinemachineCamera currentCamera;
    public CinemachineCamera newCamera;
    public Transform player;
    public CanvasGroup infoCanvas;

    //[Space(50)]
    //[Header("Booleans Because My Script Don't Work")]
    private bool isPlayerInZone = false;
    private bool hasDialogueShown = false;
    private bool isAnimationFinished = false;
    
    
    private float playerCharacterControllerCenter;
    private MoveToTarget moveJump;
    
    
    [Space(50)]
    
    [HideInInspector, SerializeField]
    public CanvasGroup dialogueCanvas;
    [HideInInspector, SerializeField]
    public CinemachineCamera secondCamera;
    [HideInInspector, SerializeField]
    public Animator playerAnimator;
    [HideInInspector, SerializeField]
    public string animToPlay = "Jump";
    [HideInInspector, SerializeField]
    public string nextAnimToPlay;

    public GameObject heronZoomCamera;
    private Boolean asHeronBeenZoomed = false;


    private void Start()
    {
        if (heronZoomCamera != null)
        {
            heronZoomCamera.SetActive(false);
        }
        
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
            else if ((isAnAnimationTrigger) && playerAnimator != null)
            {
                TriggerAnimation();
            }
        }
    }

    private void ChangeCamera()
    {
        StartCoroutine(ChangeCameraWithDelay());
    }
    
    private void ActivateObject(string objectName)
    {
        GameObject targetObject = GameObjectUtils.FindInactiveObjectByName(objectName);

        if (targetObject != null)
        {
            targetObject.SetActive(true);
            Debug.Log($"{objectName} a été activé !");
        }
        else
        {
            Debug.LogWarning($"Impossible de trouver l'objet nommé '{objectName}'.");
        }
    }
    
    private IEnumerator ChangeCameraWithDelay()
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

            if (secondCamera != null && secondCamera.name == "LetterZoomCamera") // ZOOM LETTRE
            {

                yield return new WaitForSeconds(1f); // Ajoute un délai avant d'activer la deuxième caméra
                secondCamera.gameObject.SetActive(true);
                newCamera.gameObject.SetActive(false);
                ActivateObject("bedTrigger");

                if (Input.GetKeyDown(KeyCode.Space))
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
        }
        
        if (infoCanvas != null)
        {
            HideCanvas(infoCanvas);
        }
    
        if (isAnimationFinished)
        {
            if (secondCamera != null)
            {
                secondCamera.gameObject.SetActive(true);
                newCamera.gameObject.SetActive(false);
            }
        }

        if ((newCamera.name == "héronFocusCamera") && !asHeronBeenZoomed)
        {
            asHeronBeenZoomed = true;
            //yield return new WaitForSeconds(1f);
            heronZoomCamera.SetActive(true);
            newCamera.gameObject.SetActive(false);
            yield return new WaitForSeconds(3f);
            Destroy(heronZoomCamera);
            newCamera.gameObject.SetActive(true);
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
        playerCharacterControllerCenter = 2.5f;
        StartCoroutine(TriggerAnimationWithDelay());
    }

    private IEnumerator TriggerAnimationWithDelay()
    {
        yield return new WaitForSeconds(1f);

        player.GetComponent<PlayerController>().pauseBool = true;
        moveJump = player.GetComponent<MoveToTarget>();
        if (moveJump != null)
        {
            moveJump.enabled = true;
        }

        if (playerAnimator != null)
        {
            ResetAnimatorBools();
            playerAnimator.SetBool(animToPlay, true);

            while (true)
            {
                AnimatorStateInfo stateInfo = playerAnimator.GetCurrentAnimatorStateInfo(0);

                if (stateInfo.IsName(animToPlay) && stateInfo.normalizedTime >= 1f)
                {
                    if (!string.IsNullOrEmpty(nextAnimToPlay))
                    {
                        playerAnimator.SetBool(nextAnimToPlay, true);
                        if (nextAnimToPlay == "SleepLoop")
                        {
                            ScreenFade scriptInstance = FindObjectOfType<ScreenFade>();
                            yield return new WaitForSeconds(1f);
                            scriptInstance.FadeToBlack();
                        }
                        yield break;
                    }
                    else
                    {
                        isAnimationFinished = true;
                        yield return new WaitForSeconds(0.5f);

                        ChangeCamera();
                        yield return new WaitForSeconds(1f);

                        // Transition vers une nouvelle scène
                        SceneManager.LoadScene("Lair");
                        yield break;
                    }
                }

                yield return null;
            }
        }
    }

    private void ResetAnimatorBools()
    {
        playerAnimator.SetBool("isWalking", false);
        playerAnimator.SetBool("isRunning", false);
        playerAnimator.SetBool("isGrounded", false);
        playerAnimator.SetBool("Jump", false);
        playerAnimator.SetBool("Sleep", false);
        playerAnimator.SetBool("SleepLoop", false);
    }

}
