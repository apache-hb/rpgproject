using UnityEngine;

[CreateAssetMenu(fileName = "ItemInfo", menuName = "ScriptableObjects/ItemInfo", order = 1)]
public class ItemInfo : ScriptableObject
{
    public string itemName;
    public string itemDescription;
    public float itemWeight;
    public Sprite itemSprite;
    public int itemValue;
}
