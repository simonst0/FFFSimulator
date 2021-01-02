using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Fraction", order = 1)]
public class FractionScriptableObject : ScriptableObject
{
    public int id;
    public string fractionName;
    public Color fractionColor;
    public float baseSpreadChance;
    public float baseGrowthSpeed;
    public float startInfluencePercentage;
}