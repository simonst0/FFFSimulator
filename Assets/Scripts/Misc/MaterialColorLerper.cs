using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialColorLerper : MonoBehaviour
{
    private Material targetMaterial;

    public List<Color> colors;
    public List<float> fractions;

    private float currentFraction;

    private void Awake()
    {
        Renderer renderer = GetComponentInChildren<Renderer>();
        if (!renderer)
            return;
        targetMaterial = renderer.material;
    }

    public void SetColor(float fraction)
    {
        if (colors.Count == 1)
        {
            targetMaterial.color = colors[0];
            return;
        }
        fraction = Mathf.Clamp(fraction, 0f, 1f);
        int i = 1;
        for (; i < fractions.Count - 1 && fractions[i] < fraction; i++) ;
        fraction = (fraction - fractions[i - 1]) / (fractions[i] - fractions[i - 1]);
        targetMaterial.color = Color.Lerp(colors[i - 1], colors[i], fraction);
        currentFraction = fraction;
    }

    public void AddColor(float fraction, Color color)
    {
        fractions.Add(fraction);
        colors.Add(color);
        SetColor(currentFraction);
    }

    public void ChangeFractionAtIndex(int idx, float fraction)
    {
        fractions[idx] = fraction;
        SetColor(currentFraction);
    }
}
