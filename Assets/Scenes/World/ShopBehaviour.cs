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
    [SerializeField] private Image itemSprite;
    [SerializeField] private TextMeshProUGUI sellButtonText;
    [SerializeField] private TextMeshProUGUI shopTitle;
    [SerializeField] private TextMeshProUGUI playerTitle;

    private CharacterInfo Character => PlayerBehaviour.Instance.Character;

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

        BuildItemList(shopKeeper.Items, shopItemButtons, shopListView, true);
        BuildItemList(Character.Items, playerItemButtons, playerListView, false);
    }

    public void Exit(PlayerBehaviour player)
    {
        IsActive = false;
    }

    void Start()
    {
        IsActive = false;
    }

    private void BuildItemList(List<ItemInfo> items, List<GameObject> objects, VerticalLayoutGroup list, bool isShop)
    {
        foreach (GameObject button in objects)
        {
            button.transform.SetParent(null);
            Destroy(button);
        }

        objects.Clear();

        foreach (ItemInfo item in items)
        {
            // unity loves inserting phantom data into the list
            if (item.itemName.Length == 0) continue;

            GameObject itemButton = new(item.itemName);
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
            if (Character.CanAfford(currentItem))
            {
                Character.Buy(currentItem, from: shopKeeper);
            }
        }
        else
        {
            if (shopKeeper.CanAfford(currentItem))
            {
                shopKeeper.Buy(currentItem, from: Character);
            }
        }

        shopTitle.text = $"Shop ({shopKeeper.currentMoney})";
        playerTitle.text = $"Player ({Character.currentMoney})";
        BuildItemList(Character.Items, playerItemButtons, playerListView, false);
        BuildItemList(shopKeeper.Items, shopItemButtons, shopListView, true);
    }

    public void OnCloseButton()
    {
        PlayerBehaviour.Instance.ExitInteractArea(this);
        IsActive = false;
    }

    private void OnSelectItem(ItemInfo item, bool isShopItem)
    {
        currentItem = item;
        isCurrentItemOwnedByShop = isShopItem;
        if (isCurrentItemOwnedByShop)
        {
            sellButtonText.text = "Buy";
        }
        else
        {
            sellButtonText.text = "Sell";
        }

        itemName.text = item.itemName;
        itemSprite.sprite = item.itemSprite;
        itemValue.text = $"Value: {item.itemValue}";
        itemWeight.text = $"Weight: {item.itemWeight}";
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Triggered by {other.name}");
        PlayerBehaviour player = other.GetComponentInParent<PlayerBehaviour>();
        if (player != null)
        {
            Debug.Log($"Player entered");
            player.EnterInteractArea(this);
            playerTitle.text = $"Player ({Character.currentMoney})";
            shopTitle.text = $"Shop ({shopKeeper.currentMoney})";
        }
        else
        {
            Debug.Log($"Not a player");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log($"Exited by {other.name}");
        PlayerBehaviour player = other.GetComponentInParent<PlayerBehaviour>();
        if (player != null)
        {
            player.ExitInteractArea(this);
        }
    }
}
