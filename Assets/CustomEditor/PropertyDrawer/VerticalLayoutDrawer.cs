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
        SetRect(position);
        DrawProperty(property);

    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // return 0;
        AddPropertyDataWithPath(property.propertyPath);
        var attr = attribute as VerticalLayoutAttribute;
        return attr.m_EOL
            ? EditorGUIUtility.singleLineHeight - EditorGUIUtility.standardVerticalSpacing
            // : EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            : EditorGUIUtility.singleLineHeight - EditorGUIUtility.standardVerticalSpacing;
            // : EditorGUIUtility.singleLineHeight;
            
            // ? EditorGUIUtility.singleLineHeight - EditorGUIUtility.standardVerticalSpacing
        // ? EditorGUIUtility.singleLineHeight * (s_numberOfLines - 1)
        // : EditorGUIUtility.singleLineHeight - EditorGUIUtility.standardVerticalSpacing;
        // return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
    }
}