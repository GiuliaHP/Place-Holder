using UnityEngine;

public class CanvasOrient : MonoBehaviour
{
    public Camera targetCamera; // La caméra vers laquelle le canvas doit s'orienter

    void Start()
    {
        // Si aucune caméra n'est assignée, utiliser la caméra principale
        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }
    }

    void LateUpdate()
    {
        if (targetCamera != null)
        {
            // Faire face à la caméra
            transform.LookAt(transform.position + targetCamera.transform.rotation * Vector3.forward, 
                targetCamera.transform.rotation * Vector3.up);
        }
    }
}