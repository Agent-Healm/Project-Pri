using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Healm.EditorTools;
using UnityEditor;
using UnityEngine;
[CustomPropertyDrawer(typeof(HorizontalLayoutAttribute))]
public class HorizontalLayoutDrawer : PropertyDrawer
{
    private static readonly List<string> propertyPaths = new();

    private static SerializedObject currentSO;
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var attr = attribute as HorizontalLayoutAttribute;

        if (currentSO == null)
        {
            currentSO = property.serializedObject;
        }

        propertyPaths.Add(property.propertyPath);

        if (!attr.m_EOL)
        {
            return;
        }
        float widthPerProp = position.width / propertyPaths.Count;

        Rect fieldRect = new Rect(position.x, position.y, widthPerProp, position.height);

        for (int i = 0; i < propertyPaths.Count; i++)
        {
            SafeSiblingDrawer.DrawSiblingProperty(fieldRect, property, propertyPaths[i]);
            fieldRect.x += widthPerProp;
        }
        propertyPaths.Clear();
        currentSO = null;
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var attr = attribute as HorizontalLayoutAttribute;
        return attr.m_EOL
            ? EditorGUIUtility.singleLineHeight
            : 0f;
    }
}

public static class SafeSiblingDrawer
{
    public static void DrawSiblingProperty(Rect position, SerializedProperty current, string siblingName)
    {
        SerializedProperty sibling = current.serializedObject.FindProperty(siblingName);

        if (sibling == null)
        {
            EditorGUI.LabelField(position, $"<Missing: {siblingName}>");
            return;
        }

        // float prevLabelWidth = EditorGUIUtility.labelWidth;
        // float labelWidth = 30f;
        var fieldInfo = current.serializedObject.targetObject.GetType().GetField(siblingName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        // FieldColorAttribute fieldColorAttribute = fieldInfo?.GetCustomAttribute<FieldColorAttribute>();
        // fieldColorAttribute?.Apply(position);

        // LabelSizeAttribute labelSizeAttr = fieldInfo?.GetCustomAttribute<LabelSizeAttribute>();
        // labelSizeAttr?.Apply();

        var customAttrs = fieldInfo?.GetCustomAttributes(false);
        var appliedAttrs = new List<object>();

        if (customAttrs != null)
        {
            foreach (var attr in customAttrs)
            {
                var applyMethod = attr.GetType().GetMethod("Apply");
                if (applyMethod != null)
                {
                    var parameters = applyMethod.GetParameters();
                    if (parameters.Length == 1 && parameters[0].ParameterType == typeof(Rect))
                    {
                        applyMethod.Invoke(attr, new object[] { position });
                    }
                    else if (parameters.Length == 0)
                    {
                        applyMethod.Invoke(attr, null);
                    }
                    appliedAttrs.Add(attr);
                }
            }
        }
        // Draw manually based on property type
        switch (sibling.propertyType)
        {
            case SerializedPropertyType.Integer:
                sibling.intValue = EditorGUI.IntField(position, sibling.displayName, sibling.intValue);
                break;
            case SerializedPropertyType.Float:
                sibling.floatValue = EditorGUI.FloatField(position, sibling.displayName, sibling.floatValue);
                break;
            case SerializedPropertyType.Boolean:
                sibling.boolValue = EditorGUI.Toggle(position, sibling.displayName, sibling.boolValue);
                break;
            case SerializedPropertyType.String:
                sibling.stringValue = EditorGUI.TextField(position, sibling.displayName, sibling.stringValue);
                break;
            case SerializedPropertyType.ObjectReference:
                sibling.objectReferenceValue = EditorGUI.ObjectField(position, sibling.displayName,
                    sibling.objectReferenceValue, typeof(UnityEngine.Object), true);
                break;
            default:
                EditorGUI.LabelField(position, $"{sibling.displayName} (type not supported)");
                break;
        }

        // fieldColorAttribute?.Revert();
        // labelSizeAttr?.Revert();
        for (int i = appliedAttrs.Count - 1; i >= 0; i--)
        {
            var revertMethod = appliedAttrs[i].GetType().GetMethod("Revert");
            revertMethod?.Invoke(appliedAttrs[i], null);
        }
    }
}
