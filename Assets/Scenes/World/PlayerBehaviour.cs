using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Eflatun.SceneReference;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] private SceneReference mainMenuScene;
    [SerializeField] private SceneReference encounterScene;

    [SerializeField] private GameObject menuDisplayObject;
    [SerializeField] private GameObject inventoryDisplayObject;

    [SerializedDictionary("Position", "Encounter")]
    [SerializeField] private SerializedDictionary<Transform, PartyInfo> partyPositions;

    [Range(1f, 1000f)]
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private Rigidbody2D playerRigidbody;

    [Range(0.01f, 20f)]
    [SerializeField] private float followSpeed = 1f;
    [SerializeField] private GameObject followCamera;

    [SerializeField] private List<Transform> cameraBounds = new();

    private float2 movementInput = float2.zero;
    private float initialTimeScale = 1f;

    private float2 minCameraBounds;
    private float2 maxCameraBounds;

    private bool IsInventoryActive => inventoryDisplayObject.activeSelf;
    private bool IsPauseMenuActive => menuDisplayObject.activeSelf;

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

    void Start()
    {
        menuDisplayObject.SetActive(false);
        inventoryDisplayObject.SetActive(false);

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
        if (followCamera != null)
        {
            float2 position = playerRigidbody.position;
            float x = Mathf.Clamp(position.x, minCameraBounds.x, maxCameraBounds.x);
            float y = Mathf.Clamp(position.y, minCameraBounds.y, maxCameraBounds.y);
            float3 target = new float3(x, y, followCamera.transform.position.z);
            followCamera.transform.position = math.lerp(followCamera.transform.position, target, followSpeed * Time.deltaTime);
        }
    }

    void FixedUpdate()
    {
        // using AddForce feels horrible, just set the velocity directly
        playerRigidbody.velocity = movementInput * movementSpeed;

    }

    public void OnInputMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    public void OnInventoryButtonClicked(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            inventoryDisplayObject.SetActive(!IsInventoryActive);
            PauseGame(IsInventoryActive);
        }
    }

    public void OnShowMenuButtonClicked(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            menuDisplayObject.SetActive(!IsPauseMenuActive);
            PauseGame(IsPauseMenuActive);
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
