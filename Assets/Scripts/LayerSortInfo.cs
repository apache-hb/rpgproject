using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LayerSortInfo
{
    [SerializeField]
    private int sortingLayerId = 0;

    [Range(1f, 31f)]
    [SerializeField]
    private int sortingOrder = 1;

    public int OrderInLayer { get => sortingOrder; set => sortingOrder = value; }
    public SortingLayer Layer { get => SortingLayer.layers[sortingLayerId]; set => sortingLayerId = value.id; }
    public int Id => Layer.id;
    public string Name => Layer.name;

    public void ApplyLayer(GameObject obj)
    {
        obj.layer = Id;

        obj.GetComponent<SpriteRenderer>().sortingLayerName = Name;
        SpriteRenderer[] srs = obj.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sr in srs)
        {
            sr.sortingLayerName = Name;
        }
    }
}
