using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum AbilityTargetMode
{
    Self,
    SingleEnemy,
    SingleAlly,
    AllEnemies,
    AllAllies,
    All
}

[CreateAssetMenu(fileName = "AbilityActionInfo", menuName = "ScriptableObjects/Actions/AbilityActionInfo", order = -1)]
public abstract class AbilityActionInfo : ScriptableObject
{
    public AbilityTargetMode targetMode = AbilityTargetMode.SingleEnemy;
    public bool divideAcrossTargets = false;
}
