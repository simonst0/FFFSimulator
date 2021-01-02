using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MaterialSwitch : MonoBehaviour
{
    public Renderer targetRenderer;
    private Material substituteMaterialInstance;
    public Material substituteMaterial;
    private Material originalMaterial;
    private string MaterialPath = "Materials/";
    public string MaterialName = "OutlineMat";

    private void Start()
    {
        if (!substituteMaterial)
            substituteMaterial = Resources.Load<Material>(MaterialPath + MaterialName);
        substituteMaterialInstance = Material.Instantiate(substituteMaterial);
        if (!targetRenderer && !TryGetComponent<Renderer>(out targetRenderer))
            Debug.LogError("No Renderer found for " + gameObject.name);
        originalMaterial = targetRenderer.material;

        //This works for the standard shader + synti models shader
        if (originalMaterial.HasProperty("_Color"))
            substituteMaterialInstance.color = originalMaterial.color;
        if (originalMaterial.HasProperty("_MainTex"))
            substituteMaterialInstance.mainTexture = originalMaterial.mainTexture;
        else if (originalMaterial.HasProperty("_Texture"))
            substituteMaterialInstance.mainTexture = originalMaterial.GetTexture("_Texture");
    }

    public void ToggleMaterial()
    {
        if (targetRenderer.material == substituteMaterialInstance)
            SetOriginalMaterial();
        else
            SetSubstituteMaterial();
    }

    public void SetSubstituteMaterial()
    {
        targetRenderer.material = substituteMaterialInstance;
    }

    public void SetOriginalMaterial()
    {
        targetRenderer.material = originalMaterial;
    }
}
