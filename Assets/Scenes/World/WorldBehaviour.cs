using Eflatun.SceneReference;
using UnityEngine;
using UnityEngine.Assertions;

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

    void Start()
    {
        menuDisplayObject.SetActive(false);
        inventoryDisplayObject.SetActive(false);
    }

    void FixedUpdate()
    {
        playerRigidbody.AddForce(movementInput * movementSpeed);
    }

    public void OnMoveInput(Vector2 movement)
    {
        movementInput = movement;
    }

    public void OnInventoryButtonClicked()
    {
        bool isInventoryActive = inventoryDisplayObject.activeSelf;
        inventoryDisplayObject.SetActive(!isInventoryActive);
        if (!isInventoryActive)
        {
            initialTimeScale = Time.timeScale;
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = initialTimeScale;
        }
    }

    public void OnShowMenuButtonClicked()
    {
        menuDisplayObject.SetActive(true);
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
