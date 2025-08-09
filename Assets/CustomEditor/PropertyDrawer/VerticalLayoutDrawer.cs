using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(VerticalLayoutAttribute))]
public class VerticalLayoutDrawer : LayoutDrawer
{
    // private static readonly List<string> propertyPaths = new();
    // private static SerializedObject currentSO;

    // public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    // {
    //     // base.OnGUI(position, property, label);
    //     var attr = attribute as VerticalLayoutAttribute;
    //     if (currentSO == null)
    //     {
    //         currentSO = property.serializedObject;
    //     }
    //     propertyPaths.Add(property.propertyPath);
    //     // Debug.Log($"Added property : {property.propertyPath}");
    //     if (!attr.m_EOL)
    //     {
    //         return;
    //     }
    //     // PropertyData headPropertyData = HorizontalLayoutDrawer.FindProp("c");
    //     // Debug.Log($"property data {headPropertyData.propertyPath}");
    //     float fieldHeight = EditorGUIUtility.singleLineHeight;
    //     // Rect fieldRect = headPropertyData.rect;
    //     position.x += position.width * 2 / 3;

    //     for (int i = 0; i < propertyPaths.Count; i++)
    //     {
    //         // propertyPaths[i].rect = fieldRect;
    //         // fieldRect.y += fieldHeight;
    //         SafeSiblingDrawer.DrawSiblingProperty(position, property, propertyPaths[i]);
    //         // SafeSiblingDrawer.DrawSiblingProperty(fieldRect, property, propertyPaths[i]);
    //         // EditorGUI.PropertyField(position,propertyPaths[i]);
    //         position.y += fieldHeight;
    //     }
    //     // EditorGUILayout.Space();
    //     foreach (var path in propertyPaths)
    //     {
    //         Debug.Log($"Vertical layout : {path}");
    //     }
    //     GUILayout.Space(fieldHeight * (propertyPaths.Count - 1));
    //     propertyPaths.Clear();
    //     currentSO = null;
    // }
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var attr = attribute as VerticalLayoutAttribute;
        // AddVerticalFields(property.propertyPath);

        AddPropertyDataWithPath(property.propertyPath, "");
        if (!attr.m_EOL)
        {
            return;
        }
        SetRect(position);

        // foreach (var verticalField in _verticalFields)
        // {
        //     foreach (var propertyData in verticalField.propertyData)
        //     {
        //         Debug.Log($"Property added : {propertyData.propertyPath}");
        //         Debug.Log($"Rect : {propertyData.rect}");
        //     }
        // }
        DrawProperty(property);
        EditorGUILayout.Space(EditorGUIUtility.singleLineHeight * (s_numberOfLines - 1));        
        s_numberOfLines = 1;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var attr = attribute as VerticalLayoutAttribute;
        return attr.m_EOL
        ? EditorGUIUtility.singleLineHeight
        : 0;
    }
}