using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MessagePack;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "CharacterInfo", menuName = "ScriptableObjects/CharacterInfo", order = -1)]
[MessagePackObject(keyAsPropertyName: true)]
public class CharacterInfo : ScriptableObject
{
    public string characterName;
    public int defaultHealth;
    public int defaultEnergy;
    public int defaultMoney;

    public int currentHealth;
    public int currentEnergy;
    public int currentMoney;

    public List<ItemInfo> items;

    [IgnoreMember]
    [HideInInspector]
    public List<ItemInfo> Items => items.Where(item => item != null).ToList();

    public bool CanAfford(ItemInfo item)
    {
        return currentMoney >= item.itemValue;
    }

    public void Buy(ItemInfo item, CharacterInfo from)
    {
        currentMoney -= item.itemValue;
        items.Add(item);
        from.SellItem(item);
    }

    public void SellItem(ItemInfo item)
    {
        currentMoney += item.itemValue;
        items.Remove(item);
    }
}
