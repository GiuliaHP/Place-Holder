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
            EditorGUILayout.Space(50);
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
            EditorGUILayout.Space(50);
            EditorGUILayout.LabelField("Animation Settings", EditorStyles.boldLabel);

            // Afficher le champ pour la caméra secondaire
            cameraTrigger.secondCamera = (CinemachineCamera)EditorGUILayout.ObjectField(
                "Second Camera", 
                cameraTrigger.secondCamera, 
                typeof(CinemachineCamera), // Corriger le type pour la caméra virtuelle
                true
            );
            cameraTrigger.playerAnimator = (Animator)EditorGUILayout.ObjectField(
                "Player Animator", 
                cameraTrigger.playerAnimator, 
                typeof(Animator), // Corriger le type pour la caméra virtuelle
                true
            );
            cameraTrigger.animToPlay = EditorGUILayout.TextField(
                "Anim To Play", 
                cameraTrigger.animToPlay
            );
            cameraTrigger.nextAnimToPlay = EditorGUILayout.TextField(
                "Next Anim To Play", 
                cameraTrigger.nextAnimToPlay
            );
        }
    }
}