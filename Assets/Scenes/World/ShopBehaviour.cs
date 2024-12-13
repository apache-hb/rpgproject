using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopBehaviour : MonoBehaviour, IInteract
{
    [SerializeField] private InventoryDisplay shopInventory;
    [SerializeField] private InventoryDisplay playerInventory;
    [SerializeField] private GameState.ShopId shopId;

    [SerializeField] private GameObject canvas;

    [SerializeField] private TextMeshProUGUI sellText;
    [SerializeField] private TextMeshProUGUI shopTitle;
    [SerializeField] private TextMeshProUGUI playerTitle;

    public GameState.Character Character => WorldManager.PlayerCharacter;
    public GameState.Character ShopKeeper => WorldManager.Shops[shopId];

    private GameState.Item selectedItem;
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

        UpdateShopDisplay();
    }

    public void Exit(PlayerBehaviour player)
    {
        IsActive = false;
    }

    void Start()
    {
        shopInventory.OnItemClick += (item) => OnSelectItem(item, true);
        playerInventory.OnItemClick += (item) => OnSelectItem(item, false);

        Debug.Log($"Shopkeeper: {shopId} {ShopKeeper == null}");
        shopInventory.Character = ShopKeeper;
        playerInventory.Character = Character;

        IsActive = false;
    }

    public void OnTransactButton()
    {
        if (selectedItem == null) return;

        if (isCurrentItemOwnedByShop)
        {
            if (Character.CanAfford(selectedItem))
            {
                Character.Buy(selectedItem, from: ShopKeeper);
            }
        }
        else
        {
            if (ShopKeeper.CanAfford(selectedItem))
            {
                ShopKeeper.Buy(selectedItem, from: Character);
            }
        }

        UpdateShopDisplay();
    }

    public void OnCloseButton()
    {
        PlayerBehaviour.Instance.ExitInteractArea(this);
        IsActive = false;
    }

    private void OnSelectItem(GameState.Item item, bool isShopItem)
    {
        selectedItem = item;
        isCurrentItemOwnedByShop = isShopItem;
        if (isCurrentItemOwnedByShop)
        {
            sellText.text = "Buy";
        }
        else
        {
            sellText.text = "Sell";
        }
    }

    private void UpdateShopDisplay()
    {
        shopTitle.text = $"Shop ({ShopKeeper.money})";
        playerTitle.text = $"Player ({Character.money})";

        shopInventory.Character = ShopKeeper;
        playerInventory.Character = Character;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerBehaviour player = other.GetComponentInParent<PlayerBehaviour>();
        if (player != null)
        {
            player.EnterInteractArea(this);
            playerTitle.text = $"Player ({Character.money})";
            shopTitle.text = $"Shop ({ShopKeeper.money})";
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        PlayerBehaviour player = other.GetComponentInParent<PlayerBehaviour>();
        if (player != null)
        {
            player.ExitInteractArea(this);
        }
    }
}
