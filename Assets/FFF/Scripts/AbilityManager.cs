using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class AbilityManager : IndexingDictionaryManager<string, Ability>
{
    private List<Ability> _activeAbilities = new List<Ability>();
    public Ability[] activeAbilities { get { return _activeAbilities.ToArray(); } }

    protected override void Awake()
    {
        Debug.Assert(keyFieldName == "abilityName");
        base.Awake();
        foreach (var ability in managedReferences)
            Broadcaster.Instance.RegisterToBroadcastImmediate(ability.Value.abilityName, "OnAbilityUnlockBroadcast", this);
    }

    private void OnAbilityUnlockBroadcast(params object[] list)
    {
        _activeAbilities.Add(Util.GetBroadcastParamAtIndex<Ability>(list, 0));
    }
}