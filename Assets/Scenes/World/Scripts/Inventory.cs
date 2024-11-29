using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MessagePack;
using System;

[Serializable]
public class Item
{
    public string Name;
}

[Serializable]
public class Character
{
    public string Name;
    public int Experience;
    public int Health;
    public int Mana;
}

[MessagePackObject(keyAsPropertyName: true)]
public class Inventory
{
    public List<Item> Items;

}
