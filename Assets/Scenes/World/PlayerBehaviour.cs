using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

public interface IInteract
{
    string InteractText { get; }

    void Interact(PlayerBehaviour player);
    void Exit(PlayerBehaviour player);
}

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject menuDisplayObject;
    [SerializeField] private InventoryBehaviour inventoryDisplayObject;
    [SerializeField] private GameObject encounterDisplayObject;
    [SerializeField] private InteractBehaviour interactDialogue;

    [Range(1f, 1000f)]
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private Rigidbody2D playerRigidbody;

    [Range(0.01f, 20f)]
    [SerializeField] private float followSpeed = 1f;
    [SerializeField] private GameObject followCamera;

    [SerializeField] private List<Transform> cameraBounds = new();

    public GameState.Character Character => WorldManager.PlayerCharacter;

    private float2 movementInput = float2.zero;
    private float initialTimeScale = 1f;

    private float2 minCameraBounds;
    private float2 maxCameraBounds;

    private IInteract currentInteractable;

    public bool HasInteractable => currentInteractable != null;

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

    private bool IsEncounterDataValid => false;

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
        WorldManager.PlayerPosition = transform.position;
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
        playerRigidbody.linearVelocity = movementInput * movementSpeed;
    }

    public void StartEncounter(GameState.EncounterId id)
    {
        WorldManager.LoadEncounter();
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
        WorldManager.LoadMainMenu();
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
        WorldManager.SaveGame();
    }

    public void LoadGameFromSave()
    {
        WorldManager.LoadGame();
        transform.position = WorldManager.PlayerPosition;
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
        Assert.IsNotNull(menuDisplayObject);
        Assert.IsNotNull(inventoryDisplayObject);
        Assert.IsNotNull(playerRigidbody);
        Assert.IsFalse(cameraBounds.Contains(null));
    }
}
