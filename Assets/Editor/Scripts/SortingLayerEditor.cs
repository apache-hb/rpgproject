using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(LayerSortInfo))]
public class LayerSortInfoEditor : PropertyDrawer
{
    List<string> SortingLayerNames => SortingLayer.layers.Select(layer => layer.name).ToList();
    List<int> SortingLayerIDs => SortingLayer.layers.Select(layer => layer.id).ToList();

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        SerializedProperty layer = property.FindPropertyRelative("sortingLayerId");
        SerializedProperty sortingOrder = property.FindPropertyRelative("sortingOrder");

        List<string> names = SortingLayerNames;
        List<int> ids = SortingLayerIDs;

        int choice = EditorGUILayout.IntPopup("Sorting Layer", layer.intValue, names.ToArray(), ids.ToArray());
        layer.intValue = choice;

        EditorGUILayout.PropertyField(sortingOrder);

        EditorGUI.EndProperty();
    }
}
