using System;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Compatibility;
using Unity.Properties;
using UnityEditor;
using UnityEngine;
public class LayoutDrawer : PropertyDrawer
{
    private static List<VerticalField> s_verticalFields = new();
    protected static int s_numberOfLines = 1;
    private static string s_lastAddedPropPath;

    public static void AddVerticalFields(string propPath)
    {
        List<PropertyData> propertyDatas = new List<PropertyData>() { new PropertyData(propPath) };
        VerticalField verticalField = new(propertyDatas);
        s_verticalFields.Add(verticalField);
        s_lastAddedPropPath = propPath;
    }
    public static void AddPropertyDataWithPath(string propPath)
    {
        foreach (var verticalField in s_verticalFields)
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
        float rowLength = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        position.y -= rowLength * (s_numberOfLines - 1);
        int numberOfColumn = s_verticalFields.Count;
        float fieldWidth = position.width / numberOfColumn;

        for (int col = 0; col < s_verticalFields.Count; col++)
        {
            List<PropertyData> propertyDatas = s_verticalFields[col].propertyData;
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
        // Debug.Log($"DRAWProperty {property.propertyPath} from {Event.current.type} at {Time.realtimeSinceStartup}");

        PropertyData propertyData;
        for (int col = 0; col < s_verticalFields.Count; col++)
        {
            List<PropertyData> propertyDatas = s_verticalFields[col].propertyData;
            for (int row = 0; row < propertyDatas.Count; row++)
            {
                propertyData = propertyDatas[row];
                SafeSiblingDrawer.DrawSiblingProperty(propertyData.rect, property, propertyData.propertyPath);
            }
        }
        s_verticalFields.Clear();
        s_numberOfLines = 1;
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
    public Rect rect;
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

        var fieldInfo = current.serializedObject.targetObject.GetType()
        .GetField(siblingName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

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
        DrawSerializePropertyInline(position, sibling);

        EditorGUIUtility.labelWidth = originalLabelWidth;

        for (int i = appliedAttrs.Count - 1; i >= 0; i--)
        {
            var revertMethod = appliedAttrs[i].GetType().GetMethod("Revert");
            revertMethod?.Invoke(appliedAttrs[i], null);
        }
        current.serializedObject.ApplyModifiedProperties();
    }

    public static void DrawSerializePropertyInline(Rect position, SerializedProperty sibling)
    {
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

            case SerializedPropertyType.Color:
                sibling.colorValue = EditorGUI.ColorField(position, sibling.displayName, sibling.colorValue);
                break;
            case SerializedPropertyType.LayerMask:
                sibling.intValue = EditorGUI.LayerField(position, sibling.displayName, sibling.intValue);
                break;
            case SerializedPropertyType.Enum:
                sibling.enumValueIndex = EditorGUI.Popup(position, sibling.displayName, sibling.enumValueIndex, sibling.enumDisplayNames);
                break;
            case SerializedPropertyType.Vector2:
                sibling.vector2Value = EditorGUI.Vector2Field(position, GUIContent.none, sibling.vector2Value);
                break;
            case SerializedPropertyType.Vector3:
                sibling.vector3Value = EditorGUI.Vector3Field(position, GUIContent.none, sibling.vector3Value);
                break;
            case SerializedPropertyType.Vector4:
                sibling.vector4Value = EditorGUI.Vector4Field(position, GUIContent.none, sibling.vector4Value);
                break;
            case SerializedPropertyType.Rect:
                sibling.rectValue = EditorGUI.RectField(position, sibling.displayName, sibling.rectValue);
                break;
            case SerializedPropertyType.Bounds:
                sibling.boundsValue = EditorGUI.BoundsField(position, sibling.displayName, sibling.boundsValue);
                break;

            case SerializedPropertyType.Quaternion:
                var quat = sibling.quaternionValue;
                var euler = EditorGUI.Vector3Field(position, GUIContent.none, quat.eulerAngles);
                sibling.quaternionValue = Quaternion.Euler(euler);
                break;
            case SerializedPropertyType.Vector2Int:
                sibling.vector2IntValue = EditorGUI.Vector2IntField(position, GUIContent.none, sibling.vector2IntValue);
                break;
            case SerializedPropertyType.Vector3Int:
                sibling.vector3IntValue = EditorGUI.Vector3IntField(position, GUIContent.none, sibling.vector3IntValue);
                break;
            case SerializedPropertyType.RectInt:
                sibling.rectIntValue = EditorGUI.RectIntField(position, sibling.displayName, sibling.rectIntValue);
                break;
            case SerializedPropertyType.BoundsInt:
                sibling.boundsIntValue = EditorGUI.BoundsIntField(position, sibling.displayName, sibling.boundsIntValue);
                break;

            // case SerializedPropertyType.Character:
            //     var charValue = (char)sibling.intValue;
            //     var newCharStr = EditorGUI.TextField(position, charValue.ToString());
            //     sibling.intValue = (!string.IsNullOrEmpty(newCharStr) ? newCharStr[0] : '\0');
            //     break;
            case SerializedPropertyType.AnimationCurve:
                sibling.animationCurveValue = EditorGUI.CurveField(position, sibling.displayName, sibling.animationCurveValue);
                break;

            case SerializedPropertyType.ObjectReference:
                Type fieldType = sibling.GetDeclaredType();
                EditorGUI.BeginChangeCheck();

                UnityEngine.Object pickedObj = EditorGUI.ObjectField(position, sibling.displayName,
                                                sibling.objectReferenceValue, fieldType, true);
                sibling.objectReferenceValue = pickedObj;
                break;

            case SerializedPropertyType.ManagedReference:
                // Inline display for managed refs is tricky — just show type name
                EditorGUI.LabelField(position, sibling.managedReferenceFullTypename ?? "(NULL)");
                break;
            default:
                EditorGUI.LabelField(position, $"{sibling.displayName} (type not supported)");
                break;
        }
    }

    /// <summary>
    /// Gets the declared C# type for the given SerializedProperty.
    /// Handles nested fields, arrays, and lists.
    /// </summary>
    public static Type GetDeclaredType(this SerializedProperty property)
    {
        Type parentType = property.serializedObject.targetObject.GetType();
        string path = property.propertyPath.Replace(".Array.data[", "[");

        Type currentType = parentType;
        FieldInfo fieldInfo = null;

        foreach (string element in path.Split('.'))
        {
            if (element.Contains("["))
            {
                string fieldName = element.Substring(0, element.IndexOf("["));
                fieldInfo = GetFieldInfo(currentType, fieldName);
                currentType = GetElementType(fieldInfo.FieldType);
            }
            else
            {
                fieldInfo = GetFieldInfo(currentType, element);
                currentType = fieldInfo.FieldType;
            }
        }

        return currentType;
    }

    private static FieldInfo GetFieldInfo(Type type, string name)
    {
        const BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
        var fieldInfo = type.GetField(name, flags);
        if (fieldInfo == null)
            throw new MissingFieldException($"Field '{name}' not found in type '{type}'");
        return fieldInfo;
    }

    private static Type GetElementType(Type collectionType)
    {
        if (collectionType.IsArray)
            return collectionType.GetElementType();
        if (collectionType.IsGenericType && collectionType.GetGenericTypeDefinition() == typeof(System.Collections.Generic.List<>))
            return collectionType.GetGenericArguments()[0];
        return typeof(object);
    }

}