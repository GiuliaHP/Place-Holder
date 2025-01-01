using UnityEditor;
using UnityEngine;
using Unity.Cinemachine;

[CustomEditor(typeof(CameraTrigger))]
public class CameraTriggerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        CameraTrigger cameraTrigger = (CameraTrigger)target;

        // Dessiner l'inspecteur de base
        DrawDefaultInspector();

        // Si c'est un trigger de dialogue
        if (cameraTrigger.isADialogueTrigger)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Dialogue Settings", EditorStyles.boldLabel);

            // Afficher le champ pour le Canvas de dialogue
            cameraTrigger.dialogueCanvas = (CanvasGroup)EditorGUILayout.ObjectField(
                "Dialogue Canvas", 
                cameraTrigger.dialogueCanvas, 
                typeof(CanvasGroup), 
                true
            );
        }

        // Si c'est un trigger d'animation
        if (cameraTrigger.isAnAnimationTrigger)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Animation Settings", EditorStyles.boldLabel);

            // Afficher le champ pour la caméra secondaire
            cameraTrigger.secondCamera = (CinemachineCamera)EditorGUILayout.ObjectField(
                "Second Camera", 
                cameraTrigger.secondCamera, 
                typeof(CinemachineVirtualCamera), // Corriger le type pour la caméra virtuelle
                true
            );
        }
    }
}