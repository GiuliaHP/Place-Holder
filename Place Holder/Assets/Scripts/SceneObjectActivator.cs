using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneObjectActivator : MonoBehaviour
{
    public GameObject letterToActivate, triggerToActivate;
    public CircleTransition transitionScript;
    public string newScene = "SnowScene";

    private void Start()
    {
        // Vérifie si le booléen global est activé
        if (PersistentManager.Instance != null && PersistentManager.Instance.triggerObjectInOtherScene)
        {
            transitionScript.targetScene = newScene;
            letterToActivate.SetActive(true);
            triggerToActivate.SetActive(true);
        }
    }
}