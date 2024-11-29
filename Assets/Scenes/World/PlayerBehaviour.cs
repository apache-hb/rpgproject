using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Eflatun.SceneReference;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] private SceneReference mainMenuScene;
    [SerializeField] private SceneReference encounterScene;

    [SerializeField] private GameObject menuDisplayObject;
    [SerializeField] private GameObject inventoryDisplayObject;
    [SerializeField] private GameObject encounterDisplayObject;

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

    private PartyInfo enemyPartyInfo;
    private PartyInfo playerPartyInfo;
    private float2 movementInput = float2.zero;
    private float initialTimeScale = 1f;

    private float2 minCameraBounds;
    private float2 maxCameraBounds;

    private bool IsInventoryActive => inventoryDisplayObject.activeSelf;
    private bool IsPauseMenuActive => menuDisplayObject.activeSelf;

    public static PlayerBehaviour Instance { get; private set; }

    private bool IsEncounterActive => enemyPartyInfo != null && playerPartyInfo != null;

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

        menuDisplayObject.SetActive(false);
        inventoryDisplayObject.SetActive(false);
        encounterDisplayObject.SetActive(IsEncounterActive);

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
        if (followCamera != null && !IsEncounterActive)
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
        if (IsPauseMenuActive) return;

        inventoryDisplayObject.SetActive(!IsInventoryActive);
        PauseGame(IsInventoryActive);
    }

    private void TogglePauseMenu()
    {
        if (IsInventoryActive)
        {
            ToggleInventory();
        }
        else
        {
            menuDisplayObject.SetActive(!IsPauseMenuActive);
            PauseGame(IsPauseMenuActive);
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
        menuDisplayObject.SetActive(!IsPauseMenuActive);
        PauseGame(IsPauseMenuActive);
    }

    public void OnQuitButtonClicked()
    {
        SceneManager.LoadSceneAsync(mainMenuScene.BuildIndex);
        Destroy(gameObject);
    }

    public void OnSaveButtonClicked()
    {
        // unity says player prefs arent meant to be used to save game data
        // i dont care, i will be using player prefs to save game data

        PlayerPrefs.SetInt("HasSave", 1);
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
