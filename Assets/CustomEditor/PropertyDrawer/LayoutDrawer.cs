using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Codice.Client.BaseCommands;
using Unity.Properties;
using UnityEditor;
using UnityEngine;

public class LayoutDrawer : PropertyDrawer
{
    protected static List<VerticalField> _verticalFields = new();
    public static int s_numberOfLines = 1;
    protected static string s_lastAddedPropPath;
    protected static Rect s_initialPosition;

    // public static void Initialize(Rect rect)
    // {
        // if (s_initialPosition.x == 0)
        // {
        //     Debug.Log($"Initialized at {rect.width}, {rect.height}");
        //     s_initialPosition = rect;
        // }
    // }
    public static void AddVerticalFields(string propPath)
    {
        List<PropertyData> propertyDatas = new List<PropertyData>() { new PropertyData(propPath) };
        VerticalField verticalField = new(propertyDatas);
        _verticalFields.Add(verticalField);
        s_lastAddedPropPath = propPath;
    }
    public static void AddPropertyDataWithPath(string propPath)
    {
        foreach (var verticalField in _verticalFields)
        {
            if (verticalField.propertyData[0]?.propertyPath == s_lastAddedPropPath)
            {
                verticalField.propertyData.Add(new(propPath));
                s_numberOfLines = Mathf.Max(s_numberOfLines, verticalField.propertyData.Count);
                return;
            }
        }
    }
    protected void SetRect(Rect position)
    {
        // Debug.Log($"Rect position at {position.x}, {position.y}");
        // Debug.Log($"There are {s_numberOfLines} of lines here");
        // position.y += EditorGUIUtility.standardVerticalSpacing;
        // position.y -= EditorGUIUtility.singleLineHeight * (s_numberOfLines - 1);
        // position.y -= EditorGUIUtility.standardVerticalSpacing * (s_numberOfLines + 1);
        position.y -= EditorGUIUtility.singleLineHeight * (s_numberOfLines - 1);
        int numberOfColumn = _verticalFields.Count;
        float fieldWidth = position.width / numberOfColumn;
        float rowLength = EditorGUIUtility.singleLineHeight;

        for (int col = 0; col < _verticalFields.Count; col++)
        {
            List<PropertyData> propertyDatas = _verticalFields[col].propertyData;
            for (int row = 0; row < propertyDatas.Count; row++)
            {
                propertyDatas[row].rect = new(position.x + col * fieldWidth,
                                             position.y + row * rowLength,
                                             fieldWidth, position.height);

                EditorGUI.DrawRect(propertyDatas[row].rect, new Color(0,0,1));
            }
        }
    }
    protected void DrawProperty(SerializedProperty property)
    {
        PropertyData propertyData;
        for (int col = 0; col < _verticalFields.Count; col++)
        {
            List<PropertyData> propertyDatas = _verticalFields[col].propertyData;
            for (int row = 0; row < propertyDatas.Count; row++)
            {
                propertyData = propertyDatas[row];
                SafeSiblingDrawer.DrawSiblingProperty(propertyData.rect, property, propertyData.propertyPath);
            }
        }
        _verticalFields.Clear();
        // EditorGUILayout.Space(EditorGUIUtility.singleLineHeight * (s_numberOfLines - 1));        
        s_numberOfLines = 1;
    }

    protected float CalculateDrawPropertyHeight(SerializedProperty property)
    {
        float height = 0f;

        float heightTemp;
        PropertyData propertyData;
        SerializedProperty seriProperty;

        for (int col = 0; col < _verticalFields.Count; col++)
        {
            List<PropertyData> propertyDatas = _verticalFields[col].propertyData;
            heightTemp = -EditorGUIUtility.standardVerticalSpacing;

            for (int row = 0; row < propertyDatas.Count; row++)
            {
                propertyData = propertyDatas[row];
                seriProperty = property.serializedObject.FindProperty(propertyData.propertyPath);

                // heightTemp += EditorGUI.GetPropertyHeight(seriProperty, true);
                heightTemp += EditorGUIUtility.singleLineHeight;
                heightTemp += EditorGUIUtility.standardVerticalSpacing;
                // heightTemp = Mathf.Max(propertyData.rect.y + propertyData.rect.height, heightTemp);
                // Debug.Log($"Property height : {EditorGUI.GetPropertyHeight(seriProperty, true)}");
                Debug.Log($"Property height : {heightTemp}");
            }
            height = Mathf.Max(height, heightTemp);
        }

        // Debug.Log($"Final height : {height}");
        return height;
    }
}
public class VerticalField
{
    public List<PropertyData> propertyData;

    public VerticalField(List<PropertyData> propertyData)
    {
        this.propertyData = propertyData;
    }
}

public class PropertyData
{
    public string propertyPath;
    // public string PropertyPath => propertyPath;
    public Rect rect;
    // public Rect PropertyRect => rect;
    public PropertyData(string propertyPath)
    {
        this.propertyPath = propertyPath;
    }
    public PropertyData(string propertyPath, Rect rect)
    {
        this.propertyPath = propertyPath;
        this.rect = rect;
    }
}

public static class SafeSiblingDrawer
{
    public static void DrawSiblingProperty(Rect position, SerializedProperty current, string siblingName)
    {
        float originalLabelWidth = EditorGUIUtility.labelWidth;
        EditorGUIUtility.labelWidth -= EditorGUIUtility.fieldWidth;

        SerializedProperty sibling = current.serializedObject.FindProperty(siblingName);

        if (sibling == null)
        {
            EditorGUI.LabelField(position, $"<Missing: {siblingName}>");

            return;
        }

        var fieldInfo = current.serializedObject.targetObject.GetType().GetField(siblingName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

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

        EditorGUIUtility.labelWidth = originalLabelWidth;

        for (int i = appliedAttrs.Count - 1; i >= 0; i--)
        {
            var revertMethod = appliedAttrs[i].GetType().GetMethod("Revert");
            revertMethod?.Invoke(appliedAttrs[i], null);
        }
    }
}