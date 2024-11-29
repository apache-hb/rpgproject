using System;
using System.Collections.Generic;
using MessagePack;
using UnityEngine;

[Serializable]
[MessagePackObject(keyAsPropertyName: true)]
[CreateAssetMenu(fileName = "ItemInfo", menuName = "ScriptableObjects/ItemInfo", order = 1)]
public class ItemInfo : ScriptableObject
{
    public string itemName;
    public string itemDescription;
    public float itemWeight;

    [IgnoreMember]
    public Sprite itemSprite;

    public int itemValue;
}

[Serializable]
[MessagePackObject(keyAsPropertyName: true)]
[CreateAssetMenu(fileName = "Character", menuName = "ScriptableObjects/Character", order = -1)]
public class Character : ScriptableObject
{
    public string Name;
    public int Experience;
    public int Health;
    public int Mana;
}

[Serializable]
[MessagePackObject(keyAsPropertyName: true)]
public class Inventory
{
    public int money;
    public int experience;
    public List<ItemInfo> items = new();
}
