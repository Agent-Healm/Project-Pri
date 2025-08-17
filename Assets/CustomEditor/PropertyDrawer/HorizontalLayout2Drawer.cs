using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(HorizontalLayout2Attribute))]
public class HorizontalLayout2Drawer : PropertyDrawer
{
    private static Dictionary<string, List<string>> s_dict = new();

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var attr = attribute as HorizontalLayout2Attribute;
            
        if (!s_dict.ContainsKey(attr.GroupName))
        {
            Debug.Log($"No width defined for {attr.GroupName}");
            GetNumberOfField(property);
        }

        // Rect rect = EditorGUILayout.GetControlRect();
        // EditorGUI.prop


        // position.width /= s_dict[attr.GroupName].Count;
        // // float fixedWidth = position.width;
        // position.x += position.width * s_dict[attr.GroupName].IndexOf(property.name);
        // // Debug.Log($"Prop name : {property.name}");
        // EditorGUI.PropertyField(position, property, label);

        // position.x += position.width;
        // EditorGUI.PropertyField(position, property, label);
        // EditorGUI.PropertyField(position, property, label);

        foreach (var dict in s_dict)
        {
            Debug.Log($"{dict.Key} : {dict.Value.Count}");
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }

    private void GetNumberOfField(SerializedProperty property)
    {
        Debug.Log("Invoked GetNumberOf FIeld");
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
                    s_dict[horizontalAttr.GroupName].Add(fieldInfo.Name);
                    break;
                }
            }
        }
        

    }
}
