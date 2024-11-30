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

    public int itemValue = 1;
}
