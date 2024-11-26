using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackAbilityInfo", menuName = "ScriptableObjects/Actions/AttackAbilityInfo", order = 1)]
public class AttackAbilityInfo : AbilityActionInfo
{
    [Range(1f, 1000f)]
    public int damage;
}
