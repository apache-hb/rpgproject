using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
using UnityEngine;

public class InteractBehaviour : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI interactText;

    public string InteractText
    {
        get => interactText.text;
        set => interactText.text = value;
    }

    void OnValidate()
    {
        Assert.IsNotNull(interactText, nameof(interactText));
    }
}
