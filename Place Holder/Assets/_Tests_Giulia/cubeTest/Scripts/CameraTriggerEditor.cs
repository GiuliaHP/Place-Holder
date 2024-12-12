using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CameraTrigger))]
public class CameraTriggerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Récupérer le script attaché
        CameraTrigger cameraTrigger = (CameraTrigger)target;

        // Dessiner les champs par défaut
        DrawDefaultInspector();

        // Afficher le champ "dialogueCanvas" uniquement si "isADialogueTrigger" est vrai
        if (cameraTrigger.isADialogueTrigger)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Dialogue Settings", EditorStyles.boldLabel);
            cameraTrigger.dialogueCanvas = (CanvasGroup)EditorGUILayout.ObjectField("Dialogue Canvas", cameraTrigger.dialogueCanvas, typeof(CanvasGroup), true);
        }
    }
}