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

    [Range(1f, 100f)]
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private Rigidbody2D playerRigidbody;

    private Vector2 movementInput = Vector2.zero;
    private float initialTimeScale = 1f;

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
    }

    void FixedUpdate()
    {
        playerRigidbody.AddForce(movementInput * movementSpeed);
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
    }
}
