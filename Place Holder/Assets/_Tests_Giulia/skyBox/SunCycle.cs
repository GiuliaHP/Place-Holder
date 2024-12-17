using UnityEngine;

public class SunCycle : MonoBehaviour
{
    [Header("Sun Rotation Settings")]
    public Vector3 rotationAxis = Vector3.right;
    public float cycleDuration = 24f;

    [Header("Fog Animation Settings")]
    public Gradient fogColorGradient;
    
    [Header("Skybox Animation Settings")]
    public Gradient skyTintColorGradient;

    [Header("Water Material Settings")]
    public Gradient waterTintColorGradient;
    public GameObject sea;
    public string deepWaterColorProperty = "_DeepWater";
    
    private float cycleProgress = 0f;
    private Material skyboxMaterial;
    private Material waterMaterial;

    void Start()
    {
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

        AnimateFog();
        AnimateSkybox();
        AnimateWater();
    }

    private void AnimateFog()
    {
        RenderSettings.fogColor = fogColorGradient.Evaluate(cycleProgress);
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
            waterMaterial.SetColor(deepWaterColorProperty, deepWaterColor);
        }
    }
}
