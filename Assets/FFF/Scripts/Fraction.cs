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

    private float abilitySpreadChance = 1.0f;
    private float abilityGrowthSpeed = 1.0f;

    public float spreadChance { get { return baseSpreadChance * influencePercentage * abilitySpreadChance; } }
    public float growthSpeed { get { return baseGrowthSpeed * influencePercentage * abilityGrowthSpeed; } }
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

    public void ApplyAbilityEffect(Effect effect)
    {
        switch (effect.effect)
        {
            case AbilityEffect.None:
                break;
            case AbilityEffect.Spread:
                abilitySpreadChance += effect.value;
                break;
            case AbilityEffect.Growth:
                abilityGrowthSpeed += effect.value;
                break;
            default:
                break;
        }
    }
}