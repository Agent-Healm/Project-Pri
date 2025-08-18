using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Codice.Client.Common;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(HorizontalLayout2Attribute), useForChildren:false  )]
public class HorizontalLayout2Drawer : PropertyDrawer
{
    private static Dictionary<string, List<string>> s_dict = new();
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var attr = attribute as HorizontalLayout2Attribute;

        if (!s_dict.ContainsKey(attr.GroupName))
        {
            GetNumberOfField(property);
        }
        var props = s_dict[attr.GroupName];
        if (props[0] != property.name)
        {
            EditorGUI.PropertyField(position, property, false);
            return;
        }

        float fieldWidth = position.width / props.Count;

        EditorGUIUtility.labelWidth = 50;
        for (int i = 0; i < props.Count; i++)
        {
            string propName = props[i];
            var path = property.propertyPath.Replace(property.name, propName);
            var seriProp = property.serializedObject.FindProperty(path);

            var rect = new Rect(position.x + i * fieldWidth,
                                position.y,
                                fieldWidth,
                                EditorGUIUtility.singleLineHeight);
            EditorGUI.DrawRect(rect, new(i, i, 0, 0.5f));
            EditorGUI.PropertyField(rect, seriProp, false);
            Debug.Log($"Drawing prop for : {propName}");
        }
        foreach (var dict in s_dict)
        {
            Debug.Log($"{dict.Key} : {dict.Value.Count}");
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var attr = attribute as HorizontalLayout2Attribute;
        if (!s_dict.ContainsKey(attr.GroupName))
        {
            return EditorGUIUtility.singleLineHeight;
        }
        var props = s_dict[attr.GroupName];

        // Only the first property takes space
        if (props[0] != property.name)
        {
            return 0;
        }
        return EditorGUIUtility.singleLineHeight;
    }

    private void GetNumberOfField(SerializedProperty property)
    {
        // Debug.Log("Invoked GetNumberOf FIeld");
        FieldInfo[] fieldInfos = property.serializedObject.targetObject.GetType()
        .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        // Debug.Log($"Type : {property.serializedObject.targetObject.GetType()}");
        foreach (var fieldInfo in fieldInfos)
        {
            // Debug.Log($"Field : {fieldInfo}");

            var attrs = fieldInfo.GetCustomAttributes(false);
            foreach (var attr in attrs)
            {
                // Debug.Log($"This field has {attr.GetType()}");
                if (attr is HorizontalLayout2Attribute horizontalAttr)
                {
                    // Debug.Log("Field name : " + fieldInfo.Name);
                    if (!s_dict.ContainsKey(horizontalAttr.GroupName))
                    {
                        s_dict[horizontalAttr.GroupName] = new();
                    }
                    // s_dict[horizontalAttr.GroupName].Add(fieldInfo.Name);
                    string path = property.propertyPath.Replace(property.name, fieldInfo.Name);
                    s_dict[horizontalAttr.GroupName].Add(path);
                    break;
                }
            }
        }

    }
}
