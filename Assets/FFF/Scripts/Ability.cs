using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
{
    public bool unlocked = false;

    public string abilityName;
    public string abilityDescription;
    public Effect effect;
    public float abilityCost;

    public List<string> requiredAbilityNames;

    private void Awake()
    {
        foreach (var ability in requiredAbilityNames)
            Broadcaster.Instance.RegisterToBroadcastImmediate(ability, "OnAbilityUnlockBroadcast", this);
    }

    private void OnAbilityUnlockBroadcast(params object[] list)
    {
        Ability unlocked = Util.GetBroadcastParamAtIndex<Ability>(list, 0);
        if(unlocked.unlocked)
        {
            Broadcaster.Instance.DeregisterFromBroadcast(unlocked.abilityName, "OnAbilityUnlockBroadcast", this);
            requiredAbilityNames.Remove(unlocked.abilityName);
        }
        if (requiredAbilityNames.Count == 0)
            UnlockAbility();
    }

    private void UnlockAbility()
    {
        unlocked = true;
    }

    public void ActivateAbility()
    {
        if (!unlocked) // later add cost for ability here 
            return;
        Broadcaster.Instance.Broadcast(abilityName, false, this);
    }
}

[System.Serializable]
public class Effect
{
    public AbilityEffect effect;
    public float value;
    public EffectConstraint[] constraints;
    
}

public enum AbilityEffect
{
    None,
    Spread,
    Growth
    //Radicalization
}

[System.Serializable]
public class EffectConstraint
{
    public List<int> fractions = new List<int>(){ -1 };
    public Continent continent = Continent.All;
    public ClimateZone climateZone = ClimateZone.All;
    public PoliticalSystem politicalSystem = PoliticalSystem.All;
    public float minEducationLevel = 0;
    public float minHumanDevelopmentIndex = 0;
    public float minClimateRiskIndex = 0;
}

public enum Continent
{
    All,
    Africa,
    Asia,
    Australia,
    Europe,
    NorthAmeria,
    SouthAmerica
}

public enum ClimateZone
{
    All
}

public enum PoliticalSystem
{
    All
}