using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
[CustomPropertyDrawer(typeof(HorizontalLayoutAttribute))]
public class HorizontalLayoutDrawer : LayoutDrawer
{
    // private static readonly List<PropertyData> propertyPaths = new();
    // private static SerializedObject currentSO;
    // public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    // {
    //     var attr = attribute as HorizontalLayoutAttribute;

    //     if (currentSO == null)
    //     {
    //         currentSO = property.serializedObject;
    //     }

    //     propertyPaths.Add(new PropertyData(property.propertyPath));

    //     if (!attr.m_EOL)
    //     {
    //         return;
    //     }

    //     float widthPerProp = position.width / propertyPaths.Count;

    //     Rect fieldRect = new Rect(position.x, position.y, widthPerProp, position.height);

    //     for (int i = 0; i < propertyPaths.Count; i++)
    //     {
    //         propertyPaths[i].rect = fieldRect;
    //         SafeSiblingDrawer.DrawSiblingProperty(fieldRect, property, propertyPaths[i].propertyPath);
    //         fieldRect.x += widthPerProp;
    //     }
    //     propertyPaths.Clear();
    //     currentSO = null;
    // }
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var attr = attribute as HorizontalLayoutAttribute;

        // if (currentSO == null)
        // {
        //     currentSO = property.serializedObject;
        // }

        // propertyPaths.Add(new PropertyData(property.propertyPath));
        AddVerticalFields(property.propertyPath);
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
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var attr = attribute as HorizontalLayoutAttribute;
        return attr.m_EOL
            ? EditorGUIUtility.singleLineHeight
            : 0f;
    }
}
