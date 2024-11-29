using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PartyInfo", menuName = "ScriptableObjects/PartyInfo", order = -1)]
public class PartyInfo : ScriptableObject
{
    [SerializeReference]
    public List<CharacterInfo> characters;

    [SerializeField]
    private CharacterInfo primaryCharacter;

    public CharacterInfo PrimaryCharacter
    {
        set => primaryCharacter = value;
        get => primaryCharacter == null ? characters[0] : primaryCharacter;
    }
}
