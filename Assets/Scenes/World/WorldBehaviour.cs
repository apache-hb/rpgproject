using System.Collections.Generic;
using Eflatun.SceneReference;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

public class WorldBehaviour : MonoBehaviour
{
    [SerializeField] private SceneReference mainMenuScene;
    [SerializeField] private SceneReference encounterScene;

    [SerializeField] private GameObject menuDisplayObject;
    [SerializeField] private GameObject inventoryDisplayObject;

    [Range(1f, 1000f)]
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private Rigidbody2D playerRigidbody;

    [SerializeField] private GameObject followCamera;

    [SerializeField] private List<Transform> cameraBounds = new();

    private Vector2 movementInput = Vector2.zero;
    private float initialTimeScale = 1f;

    private Vector2 minCameraBounds;
    private Vector2 maxCameraBounds;

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

        minCameraBounds = Vector2.positiveInfinity;
        maxCameraBounds = Vector2.negativeInfinity;

        foreach (var bound in cameraBounds)
        {
            minCameraBounds.x = Mathf.Min(minCameraBounds.x, bound.position.x);
            minCameraBounds.y = Mathf.Min(minCameraBounds.y, bound.position.y);

            maxCameraBounds.x = Mathf.Max(maxCameraBounds.x, bound.position.x);
            maxCameraBounds.y = Mathf.Max(maxCameraBounds.y, bound.position.y);
        }
    }

    void FixedUpdate()
    {
        playerRigidbody.velocity = movementInput * movementSpeed;

        if (followCamera != null)
        {
            Vector3 position = playerRigidbody.position;
            float x = Mathf.Clamp(position.x, minCameraBounds.x, maxCameraBounds.x);
            float y = Mathf.Clamp(position.y, minCameraBounds.y, maxCameraBounds.y);
            followCamera.transform.position = new Vector3(x, y, followCamera.transform.position.z);
        }
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
