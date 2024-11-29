using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DefendAbilityInfo", menuName = "ScriptableObjects/Actions/DefendAbilityInfo", order = 1)]
public class DefendAbilityInfo : AbilityActionInfo
{
    [Range(0f, 1f)]
    public float resistance;
}
