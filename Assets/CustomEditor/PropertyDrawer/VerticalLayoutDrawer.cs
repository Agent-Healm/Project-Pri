using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(VerticalLayoutAttribute))]
public class VerticalLayoutDrawer : LayoutDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var attr = attribute as VerticalLayoutAttribute;
        if (!attr.m_EOL)
        {
            return;
        }
        position.y += EditorGUIUtility.singleLineHeight;
        SetRect(position);
        DrawProperty(property);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        AddPropertyDataWithPath(property.propertyPath);
        var attr = attribute as VerticalLayoutAttribute;
        // Debug.Log($"Number of line at : {s_numberOfLines}");
        return attr.m_EOL
        ? EditorGUIUtility.singleLineHeight - EditorGUIUtility.standardVerticalSpacing
        : EditorGUIUtility.singleLineHeight - EditorGUIUtility.standardVerticalSpacing;
    }
}