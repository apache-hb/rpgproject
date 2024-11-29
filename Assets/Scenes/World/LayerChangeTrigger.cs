using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerChangeTrigger : MonoBehaviour
{
    [SerializeField] private LayerSortInfo layer;

    void OnTriggerEnter2D(Collider2D other)
    {
        layer.ApplyLayer(other.gameObject);
    }
}
