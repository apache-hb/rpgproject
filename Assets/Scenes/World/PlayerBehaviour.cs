using System;
using System.Buffers.Text;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Eflatun.SceneReference;
using MessagePack;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public interface IInteract
{
    string InteractText { get; }

    void Interact(PlayerBehaviour player);
    void Exit(PlayerBehaviour player);
}

[Serializable]
[MessagePackObject(keyAsPropertyName: true)]
public class WorldState
{
    public List<CharacterInfo> characters;
}

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] private SceneReference mainMenuScene;
    [SerializeField] private SceneReference encounterScene;

    [SerializeField] private GameObject menuDisplayObject;
    [SerializeField] private InventoryBehaviour inventoryDisplayObject;
    [SerializeField] private GameObject encounterDisplayObject;
    [SerializeField] private InteractBehaviour interactDialogue;

    [SerializedDictionary("Position", "Encounter")]
    [SerializeField] private SerializedDictionary<Transform, PartyInfo> partyPositions;

    [Range(1f, 1000f)]
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private Rigidbody2D playerRigidbody;

    [Range(0.01f, 20f)]
    [SerializeField] private float followSpeed = 1f;
    [SerializeField] private GameObject followCamera;

    [SerializeField] private List<Transform> cameraBounds = new();

    [SerializeField] private PartyInfo defaultEnemyPartyInfo;
    [SerializeField] private PartyInfo defaultPlayerPartyInfo;

    public CharacterInfo Character => playerPartyInfo.PrimaryCharacter;

    private PartyInfo enemyPartyInfo;
    private PartyInfo playerPartyInfo;
    private float2 movementInput = float2.zero;
    private float initialTimeScale = 1f;

    private float2 minCameraBounds;
    private float2 maxCameraBounds;

    private IInteract currentInteractable;

    public bool HasInteractable => currentInteractable != null;
    public bool IsSavedGame => PlayerPrefs.GetInt("HasSave", 0) == 1;

    public bool InventoryActive
    {
        get => inventoryDisplayObject.gameObject.activeSelf;
        set => inventoryDisplayObject.gameObject.SetActive(value);
    }

    private bool PauseMenuActive
    {
        get => menuDisplayObject.activeSelf;
        set => menuDisplayObject.SetActive(value);
    }

    private bool InteractDialogueActive
    {
        get => interactDialogue.gameObject.activeSelf;
        set => interactDialogue.gameObject.SetActive(value);
    }

    public static PlayerBehaviour Instance { get; private set; }

    private bool IsEncounterDataValid => enemyPartyInfo != null && playerPartyInfo != null;

    private void PauseGame(bool pause)
    {
        if (pause)
        {
            initialTimeScale = Time.timeScale;
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = initialTimeScale;
        }
    }

    private bool SetGlobalInstance()
    {
        if (Instance == null)
        {
            Instance = this;
            return true;
        }

        return false;
    }

    void Start()
    {
        if (!SetGlobalInstance())
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        if (defaultEnemyPartyInfo != null && enemyPartyInfo == null)
        {
            enemyPartyInfo = defaultEnemyPartyInfo;
        }

        if (defaultPlayerPartyInfo != null && playerPartyInfo == null)
        {
            playerPartyInfo = defaultPlayerPartyInfo;
        }

        encounterDisplayObject.SetActive(IsEncounterDataValid);

        PauseMenuActive = false;
        InventoryActive = false;
        InteractDialogueActive = false;

        minCameraBounds = new float2(float.MaxValue, float.MaxValue);
        maxCameraBounds = new float2(float.MinValue, float.MinValue);

        foreach (var bound in cameraBounds)
        {
            float3 position = bound.position;
            minCameraBounds = math.min(minCameraBounds, position.xy);
            maxCameraBounds = math.max(maxCameraBounds, position.xy);
        }
    }

    void Update()
    {
        if (followCamera != null && !IsEncounterDataValid)
        {
            float2 position = playerRigidbody.position;
            float x = Mathf.Clamp(position.x, minCameraBounds.x, maxCameraBounds.x);
            float y = Mathf.Clamp(position.y, minCameraBounds.y, maxCameraBounds.y);
            float3 target = new(x, y, followCamera.transform.position.z);
            followCamera.transform.position = math.lerp(followCamera.transform.position, target, followSpeed * Time.deltaTime);
        }
    }

    void FixedUpdate()
    {
        // using AddForce feels horrible, just set the velocity directly
        playerRigidbody.velocity = movementInput * movementSpeed;
    }

    public void StartEncounter(PartyInfo partyInfo)
    {
        enemyPartyInfo = partyInfo;
        SceneManager.LoadSceneAsync(encounterScene.BuildIndex);
    }

    public void OnInputMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    private void ToggleInventory()
    {
        if (PauseMenuActive) return;

        InventoryActive = !InventoryActive;
        PauseGame(InventoryActive);
    }

    private void TogglePauseMenu()
    {
        if (InventoryActive)
        {
            ToggleInventory();
            InteractDialogueActive = false;
        }
        else
        {
            PauseMenuActive = !PauseMenuActive;
            PauseGame(PauseMenuActive);
        }
    }

    public void OnInventoryAction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ToggleInventory();
        }
    }

    public void OnInventoryButtonClicked()
    {
        ToggleInventory();
    }

    public void OnShowMenuAction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            TogglePauseMenu();
        }
    }

    public void OnShowMenuButtonClicked()
    {
        PauseMenuActive = !PauseMenuActive;
        PauseGame(PauseMenuActive);
    }

    public void OnQuitButtonClicked()
    {
        SceneManager.LoadSceneAsync(mainMenuScene.BuildIndex);
        Destroy(gameObject);
    }

    public void OnInteractKeyPressed(InputAction.CallbackContext context)
    {
        if (context.performed && HasInteractable)
        {
            currentInteractable.Interact(this);
            InteractDialogueActive = false;
        }
    }

    public void OnSaveButtonClicked()
    {
        // unity says player prefs arent meant to be used to save game data
        // i dont care, i will be using player prefs to save game data

        WorldState state = new()
        {
            characters = playerPartyInfo.characters
        };

        PlayerPrefs.SetString("SaveData", Convert.ToBase64String(MessagePackSerializer.Serialize(state)));
        PlayerPrefs.SetInt("HasSave", 1);
    }

    public void LoadGameFromSave()
    {
        if (IsSavedGame)
        {
            string saveData = PlayerPrefs.GetString("SaveData");
            WorldState state = MessagePackSerializer.Deserialize<WorldState>(Convert.FromBase64String(saveData));

            playerPartyInfo = new() { characters = state.characters, PrimaryCharacter = state.characters[0] };
        }
    }

    public void EnterInteractArea(IInteract interactable)
    {
        if (HasInteractable) return;

        currentInteractable = interactable;
        InteractDialogueActive = true;
        interactDialogue.InteractText = interactable.InteractText;
    }

    public void ExitInteractArea(IInteract interactable)
    {
        if (currentInteractable == interactable)
        {
            InteractDialogueActive = false;
            currentInteractable = null;
        }
    }

    void OnValidate()
    {
        Assert.IsNotNull(mainMenuScene);
        Assert.IsNotNull(encounterScene);
        Assert.IsNotNull(menuDisplayObject);
        Assert.IsNotNull(inventoryDisplayObject);
        Assert.IsNotNull(playerRigidbody);
        Assert.IsFalse(cameraBounds.Contains(null));
    }
}
