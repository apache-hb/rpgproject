using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyInfo : ScriptableObject
{
    [SerializeReference]
    public List<CharacterInfo> characters;
}
