using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterInfo", menuName = "ScriptableObjects/CharacterInfo", order = -1)]
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
