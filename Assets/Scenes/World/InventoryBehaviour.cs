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

    [SerializeField] private List<ItemInfo> items = new();
    private List<GameObject> itemButtons = new();
    private ItemInfo currentItem;

    public List<ItemInfo> Items { get => items; set { items = value; BuildItemList(); } }

    // Start is called before the first frame update
    void Start()
    {
        BuildItemList();
        OnSelectItem(items[0]);
    }

    private void BuildItemList()
    {
        foreach (GameObject button in itemButtons)
        {
            Destroy(button);
        }

        itemButtons.Clear();

        foreach (ItemInfo item in items)
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
        currentItem = item;
        itemImage.sprite = item.itemSprite;
        itemNameLabel.text = item.itemName;
        itemDescriptionLabel.text = item.itemDescription;
        itemWeightLabel.text = $"Weight: {item.itemWeight}";
        itemValueLabel.text = $"Value: {item.itemValue}";
    }
}
