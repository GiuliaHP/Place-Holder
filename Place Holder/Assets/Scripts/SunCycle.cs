using UnityEngine;

public class SunCycle : MonoBehaviour
{
    [Header("Sun Rotation Settings")]
    public Vector3 rotationAxis = Vector3.right;
    public float cycleDuration = 24f;

    [Header("Night Lighting Rotation Settings")]
    public GameObject nightLightingGroup;
    public Vector3 nightRotationAxis = Vector3.right;

    [Header("Fog Animation Settings")]
    public Gradient fogColorGradient;
    public AnimationCurve fogDensityCurve;
    public float maxFogDensity = 0.0005f;

    [Header("Skybox Animation Settings")]
    public Gradient skyTintColorGradient;

    [Header("Water Material Settings")]
    public Gradient waterTintColorGradient, waterBorderTintColorGradient;
    public GameObject sea;
    public string deepWaterColorProperty = "_DeepWater", shallowWaterColorProperty = "_ShallowWater";

    [Header("Environment Lighting Settings")]
    public AnimationCurve ambientIntensityCurve;
    public float ambientIntensityMultiplier = 1f;

    [Header("Light Intensity Settings")]
    public AnimationCurve lightIntensityCurve;
    public float maxLightIntensity = 1f; // Valeur maximale pour l'intensité de la lumière

    [Header("Stars Settings")]
    public MeshRenderer stars;
    public AnimationCurve starAlphaCurve; // Courbe d'animation pour l'alpha des étoiles
    
    private float cycleProgress = 0f;
    private Material skyboxMaterial;
    private Material waterMaterial;
    private Light attachedLight;

    void Start()
    {
        // Récupération de la lumière attachée
        attachedLight = GetComponent<Light>();

        // Récupération des matériaux
        waterMaterial = sea.GetComponent<MeshRenderer>().material;
        
        if (RenderSettings.skybox != null)
        {
            skyboxMaterial = RenderSettings.skybox;
        }
        else
        {
            Debug.LogWarning("Aucune Skybox assignée dans RenderSettings.");
        }
        
        if (waterMaterial == null)
        {
            Debug.LogWarning("Aucun matériau Water assigné.");
        }

        if (stars == null || stars.material == null)
        {
            Debug.LogWarning("Aucune étoile ou matériau d'étoiles assigné.");
        }
    }

    void Update()
    {
        cycleProgress += Time.deltaTime / cycleDuration;
    
        if (cycleProgress >= 1f)
        {
            cycleProgress = 0f;
        }
    
        float rotationAmount = (360f / cycleDuration) * Time.deltaTime;
        transform.Rotate(rotationAxis, rotationAmount, Space.World);
        nightLightingGroup.transform.Rotate(nightRotationAxis, rotationAmount, Space.World);

        AnimateFog();
        AnimateSkybox();
        AnimateWater();
        AnimateEnvironmentLighting();
        AnimateLightIntensity();
        AnimateStars();
    }


    private void AnimateFog()
    {
        RenderSettings.fogColor = fogColorGradient.Evaluate(cycleProgress);

        float fogDensity = fogDensityCurve.Evaluate(cycleProgress) * maxFogDensity;
        RenderSettings.fogDensity = fogDensity;
    }

    private void AnimateSkybox()
    {
        if (skyboxMaterial != null)
        {
            Color skyTint = skyTintColorGradient.Evaluate(cycleProgress);
            skyboxMaterial.SetColor("_SkyTint", skyTint);
        }
    }

    private void AnimateWater()
    {
        if (waterMaterial != null)
        {
            Color deepWaterColor = waterTintColorGradient.Evaluate(cycleProgress);
            Color shallowWaterColor = waterBorderTintColorGradient.Evaluate(cycleProgress);

            waterMaterial.SetColor(deepWaterColorProperty, deepWaterColor);
            waterMaterial.SetColor(shallowWaterColorProperty, shallowWaterColor);
            skyboxMaterial.SetColor("_GroundColor", deepWaterColor);
        }
    }

    private void AnimateEnvironmentLighting()
    {
        float ambientIntensity = ambientIntensityCurve.Evaluate(cycleProgress) * ambientIntensityMultiplier;
        RenderSettings.ambientIntensity = ambientIntensity;
    }

    private void AnimateLightIntensity()
    {
        if (attachedLight != null)
        {
            // Calculer l'intensité de la lumière en fonction de la courbe
            float lightIntensity = lightIntensityCurve.Evaluate(cycleProgress) * maxLightIntensity;

            // Appliquer l'intensité calculée
            attachedLight.intensity = lightIntensity;
        }
    }

    private void AnimateStars()
    {
        if (stars != null && stars.material != null)
        {
            // Récupérer la valeur de la courbe starAlphaCurve en fonction du cycleProgress
            float brightnessFactor = starAlphaCurve.Evaluate(cycleProgress);

            // Assurez-vous que la valeur reste entre 0 et 1
            brightnessFactor = Mathf.Clamp01(brightnessFactor);

            // Appliquer la nouvelle valeur à la propriété 'Alpha' dans le shader
            stars.material.SetFloat("_Alpha", brightnessFactor);
        }
    }








}
