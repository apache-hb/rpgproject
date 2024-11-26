using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealAbilityInfo", menuName = "ScriptableObjects/Actions/HealAbilityInfo", order = 1)]
public class HealAbilityInfo : AbilityActionInfo
{
    [Range(1f, 1000f)]
    public int healAmount;
}
