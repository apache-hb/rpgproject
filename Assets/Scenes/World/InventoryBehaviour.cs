using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryBehaviour : MonoBehaviour
{
    [SerializeField] 
    private InventoryDisplay inventory;

    // Start is called before the first frame update
    void Start()
    {
        inventory.Character = WorldManager.PlayerCharacter;
    }
}
