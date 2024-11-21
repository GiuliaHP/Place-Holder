using UnityEngine;
using Unity.Cinemachine;
using DG.Tweening;

public class CameraTrigger : MonoBehaviour
{
    public CinemachineCamera currentCamera;
    public CinemachineCamera newCamera;
    public Transform player;
    public CanvasGroup infoCanvas;
    public CanvasGroup dialogueCanvas;

    private bool isPlayerInZone = false;
    private bool hasDialogueShown = false;

    private void Start()
    {
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

            if (!hasDialogueShown && dialogueCanvas != null)
            {
                ShowCanvas(dialogueCanvas);
                hasDialogueShown = true;
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
}
