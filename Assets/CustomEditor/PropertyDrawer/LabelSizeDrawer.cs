using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(LabelSizeAttribute))]
public class LabelSizeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var attr = attribute as LabelSizeAttribute;

        attr.Apply();
        EditorGUI.PropertyField(position, property, new GUIContent(attr.LabelName ?? label.text));
        attr.Revert();
    }
}