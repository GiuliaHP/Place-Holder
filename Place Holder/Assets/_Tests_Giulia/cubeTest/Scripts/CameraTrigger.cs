using UnityEngine;
using Unity.Cinemachine;
using DG.Tweening;

public class CameraTrigger : MonoBehaviour
{
    public CinemachineCamera currentCamera;
    public CinemachineCamera newCamera;
    public Transform player;
    public CanvasGroup infoCanvas;
    
    private bool isPlayerInZone = false;

    private void Start()
    {
        if (infoCanvas != null)
        {
            infoCanvas.alpha = 0f;
            infoCanvas.transform.localScale = Vector3.zero;
            infoCanvas.gameObject.SetActive(false);
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
                ShowCanvas();
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
                HideCanvas();
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
            HideCanvas();
        }
    }

    private void ShowCanvas()
    {
        if (infoCanvas != null)
        {
            infoCanvas.transform.localScale = Vector3.zero;
            infoCanvas.alpha = 0f;

            infoCanvas.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
            infoCanvas.DOFade(1f, 0.5f);
        }
    }

    private void HideCanvas()
    {
        if (infoCanvas != null)
        {
            infoCanvas.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack);
            infoCanvas.DOFade(0f, 0.5f).OnComplete(() =>
            {
                infoCanvas.gameObject.SetActive(false);
            });
        }
    }
}
