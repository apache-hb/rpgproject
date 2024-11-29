using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopBehaviour : MonoBehaviour, IInteract
{
    [SerializeField] private CharacterInfo shopKeeper;

    [SerializeField] private GameObject canvas;

    [SerializeField] private VerticalLayoutGroup shopListView;
    [SerializeField] private VerticalLayoutGroup playerListView;

    private List<GameObject> shopItemButtons = new();
    private List<GameObject> playerItemButtons = new();

    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemValue;
    [SerializeField] private TextMeshProUGUI itemWeight;

    private CharacterInfo player;

    private ItemInfo currentItem;
    private bool isCurrentItemOwnedByShop;

    public bool IsActive
    {
        get => canvas.activeSelf;
        set => canvas.SetActive(value);
    }

    public string InteractText => "Talk to shopkeeper";

    public void Interact(PlayerBehaviour player)
    {
        IsActive = true;
        this.player = player.Character;

        BuildItemList(shopKeeper.items, shopItemButtons, shopListView, true);
        BuildItemList(this.player.items, playerItemButtons, playerListView, false);
    }

    public void Exit(PlayerBehaviour player)
    {
        IsActive = false;
        this.player = null;
    }

    private void BuildItemList(List<ItemInfo> items, List<GameObject> objects, VerticalLayoutGroup list, bool isShop)
    {
        foreach (GameObject button in objects)
        {
            Destroy(button);
        }

        objects.Clear();

        foreach (ItemInfo item in items)
        {
            GameObject itemButton = new();
            itemButton.SetActive(true);
            Button button = itemButton.AddComponent<Button>();
            button.onClick.AddListener(() => OnSelectItem(item, isShop));
            TextMeshProUGUI tmp = itemButton.AddComponent<TextMeshProUGUI>();
            tmp.text = item.itemName;
            tmp.alignment = TextAlignmentOptions.Left;
            tmp.GetComponent<RectTransform>().sizeDelta = new Vector2(500, 50);

            objects.Add(itemButton);
            itemButton.transform.SetParent(list.transform);
        }
    }

    public void OnTransactButton()
    {
        if (currentItem == null) return;

        if (isCurrentItemOwnedByShop)
        {
            if (player.CanAfford(currentItem))
            {
                shopKeeper.Buy(currentItem, from: player);
                BuildItemList(shopKeeper.items, shopItemButtons, playerListView, false);
            }
        }
        else
        {
            if (shopKeeper.CanAfford(currentItem))
            {
                player.Buy(currentItem, from: shopKeeper);
                BuildItemList(player.items, playerItemButtons, playerListView, false);
            }
        }
    }

    private void OnSelectItem(ItemInfo item, bool isShopItem)
    {
        currentItem = item;
        isCurrentItemOwnedByShop = isShopItem;

        itemName.text = item.itemName;
        itemValue.text = $"Value: {item.itemValue}";
        itemWeight.text = $"Weight: {item.itemWeight}";
    }
}
