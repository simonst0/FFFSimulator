using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fraction : MonoBehaviour
{
    public int id;
    public string fractionName;
    public Color fractionColor;
    public float baseSpreadChance;
    public float baseGrowthSpeed;
    public float influencePercentage = 0;

    public float spreadChance { get { return baseSpreadChance * influencePercentage; } }
    public float growthSpeed { get { return baseGrowthSpeed * influencePercentage; } }
    public Color colorAttribution { get { return fractionColor * influencePercentage; } }

    public void FillDataFromScriptableObject(FractionScriptableObject fraction)
    {
        id = fraction.id;
        fractionName = fraction.fractionName;
        fractionColor = fraction.fractionColor;
        baseSpreadChance = fraction.baseSpreadChance;
        baseGrowthSpeed = fraction.baseGrowthSpeed;
        influencePercentage = fraction.startInfluencePercentage;
    }
}