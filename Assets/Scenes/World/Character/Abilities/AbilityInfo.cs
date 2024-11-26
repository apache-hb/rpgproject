using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AbilityInfo", menuName = "ScriptableObjects/AbilityInfo", order = 1)]
public class AbilityInfo : ScriptableObject
{
    public string abilityName;
    public int abilityCost;

    [SerializeReference]
    public List<AbilityActionInfo> actions;
}
