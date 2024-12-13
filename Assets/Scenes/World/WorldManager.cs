using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using Eflatun.SceneReference;
using GameState;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameState
{
    [Serializable]
    public enum ItemId
    {
        None,
        Sword,
        Bow,
        Bomb,
        SmallHealth,
        MediumHealth,
        LargeHealth,
        SmallEnergy,
        MediumEnergy,
        LargeEnergy,
    }

    [Serializable]
    public enum AbilityId
    {
        None,
        Attack,
        Defend,
        Heal,
        Fireball,
        IceBlast,
        Thunderbolt,
    }

    [Serializable]
    public enum ShopId
    {
        None,
        GeneralStore,
        Blacksmith,
        Alchemist,
    }

    [Serializable]
    public enum EncounterId
    {
        None,
        Encounter0,
        Encounter1,
        Encounter2,
        Boss,
    }

    [Serializable]
    public class Item
    {
        public ItemId id = ItemId.None;
        public string description = "Item Description";
        public int value = 1;
        public int weight = 1;

        public string Name => id.ToString();
        public Sprite Sprite => WorldManager.GetItemSprite(id);
    }

    [Serializable]
    public class Ability
    {
        public string name = "Ability";
        public string description = "Ability Description";
    }

    [Serializable]
    public class Character
    {
        public string name = "Character Name";
        public List<Item> items = new();
        public List<Ability> abilities = new();

        public int health = 1;
        public int energy = 1;
        public int money = 1;

        public int maxHealth = 1;
        public int maxEnergy = 1;

        public bool IsAlive => health > 0;

        public bool CanAfford(Item item) => money >= item.value;

        public void Buy(Item item, Character from)
        {
            if (from.items.Remove(item))
            {
                items.Add(item);
                money -= item.value;
                from.money += item.value;
            }
        }
    }

    [Serializable]
    public class Party
    {
        public float3 position = new();
        public List<Character> characters = new();
        public bool IsDefeated => characters.All(c => c.IsAlive);
    }

    [Serializable]
    public class World
    {
        public Party party = new();
        public Dictionary<ShopId, Character> shops = new();
        public Dictionary<EncounterId, Party> encounters = new();

        public static World NewDefaultWorld()
        {
            return new World()
            {
                party = new()
                {
                    characters = new List<Character>()
                    {
                        new()
                        {
                            name = "Player",
                            items = new List<Item>()
                            {
                                new() { id = ItemId.Sword, value = 10, weight = 3 },
                                new() { id = ItemId.Bow, value = 15, weight = 2 },
                                new() { id = ItemId.Bomb, value = 6, weight = 4 },
                                new() { id = ItemId.SmallHealth, value = 25, weight = 1 },
                                new() { id = ItemId.SmallEnergy, value = 22, weight = 1 },
                            },
                            abilities = new List<Ability>()
                            {
                                new() { name = "Attack" },
                                new() { name = "Defend" },
                                new() { name = "Heal" },
                            },
                            health = 100,
                            energy = 100,
                            money = 100,
                            maxHealth = 100,
                            maxEnergy = 100,
                        }
                    }
                },
                shops = new()
                {
                    { 
                        ShopId.Blacksmith,
                        new() { name = "Blacksmith" } 
                    },
                    { 
                        ShopId.GeneralStore, 
                        new() { name = "General Store" }
                    },
                    { 
                        ShopId.Alchemist, 
                        new() { name = "Alchemist" } 
                    },
                }
            };
        }
    }
}

public delegate void OnItemClick(GameState.Item item);

[Serializable]
public class InventoryDisplay
{
    [SerializeField] private VerticalLayoutGroup items;
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI name;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private TextMeshProUGUI weight;
    [SerializeField] private TextMeshProUGUI value;

    private List<GameObject> buttons = new();
    private GameState.Item selected = null;
    private GameState.Character character = null;

    public event OnItemClick OnItemClick;

    public GameState.Item SelectedItem
    {
        get => selected;
        set => SelectItem(value, true);
    }

    public GameState.Character Character
    {
        get => character;
        set => SetCharacter(value);
    }

    public void Update()
    {
        BuildItemList();
    }

    private void SetCharacter(GameState.Character character)
    {
        this.character = character ?? throw new ArgumentNullException(nameof(character));
        BuildItemList();

        SelectItem(character.items.FirstOrDefault(), false);
    }

    private void BuildItemList()
    {
        foreach (GameObject button in buttons)
        {
            GameObject.Destroy(button);
        }

        buttons.Clear();

        foreach (GameState.Item item in character.items)
        {
            GameObject itemButton = new();
            itemButton.SetActive(true);
            Button button = itemButton.AddComponent<Button>();
            button.onClick.AddListener(() => SelectItem(item, true));
            TextMeshProUGUI tmp = itemButton.AddComponent<TextMeshProUGUI>();
            tmp.text = item.Name;
            tmp.alignment = TextAlignmentOptions.Left;
            tmp.GetComponent<RectTransform>().sizeDelta = new Vector2(500, 50);

            buttons.Add(itemButton);
            itemButton.transform.SetParent(items.transform);
        }
    }

    private void SelectItem(GameState.Item item, bool notify)
    {
        if (item != null)
        {
            selected = item;

            image.sprite = item.Sprite;
            name.text = item.Name;
            
            if (description != null)
            {
                description.text = item.description;
            }

            weight.text = $"Weight: {item.weight}";
            value.text = $"Value: ${item.value}";

            if (notify)
            {
                OnItemClick.Invoke(item);
            }
        }
    }
}

public class WorldManager : MonoBehaviour
{
    public static WorldManager Instance { get; private set; }

    [SerializedDictionary("Item", "Sprite")]
    [SerializeField]
    private SerializedDictionary<GameState.ItemId, Sprite> itemSprites;

    [SerializeField]
    private SceneReference mainMenuScene;

    [SerializeField]
    private SceneReference encounterScene;

    [SerializeField]
    private SceneReference worldScene;

    private GameState.World world = GameState.World.NewDefaultWorld();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static Sprite GetItemSprite(GameState.ItemId itemId) => Instance.itemSprites[itemId];

    public static GameState.World World
    {
        get => Instance.world;
        set => Instance.world = value;
    }

    public static GameState.Party Party 
    { 
        get => World.party; 
        set => World.party = value; 
    }

    public static GameState.Character PlayerCharacter
    {
        get => Party.characters.First();
    }

    public static Dictionary<ShopId, GameState.Character> Shops
    {
        get => World.shops;
    }

    public static float3 PlayerPosition
    {
        get => Party.position;
        set => Party.position = value;
    }

    public static void LoadMainMenu() => SceneManager.LoadScene(Instance.mainMenuScene.BuildIndex);
    public static void LoadEncounter() => SceneManager.LoadScene(Instance.encounterScene.BuildIndex);
    public static void LoadWorld() => SceneManager.LoadScene(Instance.worldScene.BuildIndex);

    private static string SaveKey = "Save";

    public static void SaveGame()
    {
        PlayerPrefs.SetString(SaveKey, JsonUtility.ToJson(World));
    }

    public static bool HasSave => PlayerPrefs.HasKey(SaveKey);

    public static void LoadGame()
    {
        if (HasSave)
        {
            World = JsonUtility.FromJson<GameState.World>(PlayerPrefs.GetString(SaveKey));
        }
    }

    [ContextMenu("Reset World")]
    public void ResetWorld()
    {
        World = GameState.World.NewDefaultWorld();
    }
}
