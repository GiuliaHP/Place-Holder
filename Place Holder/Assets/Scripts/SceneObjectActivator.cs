using UnityEngine;

public class SceneObjectActivator : MonoBehaviour
{
    public GameObject objectToActivate;

    private void Start()
    {
        // Vérifie si le booléen global est activé
        if (PersistentManager.Instance != null && PersistentManager.Instance.triggerObjectInOtherScene)
        {
            // Active l'objet et réinitialise le booléen si nécessaire
            objectToActivate.SetActive(true);
            PersistentManager.Instance.triggerObjectInOtherScene = false;
        }
    }
}