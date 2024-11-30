using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryBehaviour : MonoBehaviour
{
    [SerializeField] private VerticalLayoutGroup itemListObject;
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemNameLabel;
    [SerializeField] private TextMeshProUGUI itemDescriptionLabel;
    [SerializeField] private TextMeshProUGUI itemWeightLabel;
    [SerializeField] private TextMeshProUGUI itemValueLabel;

    private List<GameObject> itemButtons = new();
    private CharacterInfo inventory;

    public List<ItemInfo> Items => Inventory.Items;
    public CharacterInfo Inventory => PlayerBehaviour.Instance.Character;

    // Start is called before the first frame update
    void Start()
    {
        BuildItemList();
        OnSelectItem(Items[0]);
    }

    void Update()
    {
        BuildItemList();
        OnSelectItem(Items[0]);
    }

    private void BuildItemList()
    {
        foreach (GameObject button in itemButtons)
        {
            Destroy(button);
        }

        itemButtons.Clear();

        foreach (ItemInfo item in Items)
        {
            GameObject itemButton = new();
            itemButton.SetActive(true);
            Button button = itemButton.AddComponent<Button>();
            button.onClick.AddListener(() => OnSelectItem(item));
            TextMeshProUGUI tmp = itemButton.AddComponent<TextMeshProUGUI>();
            tmp.text = item.itemName;
            tmp.alignment = TextAlignmentOptions.Left;
            tmp.GetComponent<RectTransform>().sizeDelta = new Vector2(500, 50);

            itemButtons.Add(itemButton);
            itemButton.transform.SetParent(itemListObject.transform);
        }
    }

    public void OnSelectItem(ItemInfo item)
    {
        itemImage.sprite = item.itemSprite;
        itemNameLabel.text = item.itemName;
        itemDescriptionLabel.text = item.itemDescription;
        itemWeightLabel.text = $"Weight: {item.itemWeight}";
        itemValueLabel.text = $"Value: {item.itemValue}";
    }
}
