using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Codice.Client.Common;
using Codice.Utils;
using log4net.Util;
using PlasticGui.WorkspaceWindow.Topbar;
using Unity.Properties;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Video;
using Random = UnityEngine.Random;

[CustomPropertyDrawer(typeof(FoldoutGroupAttribute), false)]
public class FoldoutGroupDrawer : PropertyDrawer
{
    // Track which groups are expanded
    private static Dictionary<string, bool> foldoutStates = new();

    // Track if we've already drawn a group in this frame
    private static Dictionary<string, Rect> rects = new();
    private static Dictionary<string, float> heights = new();
    private static List<string> props = new();

    private static bool isDrawing = false;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var groupName = ((FoldoutGroupAttribute)attribute).groupName;

        // Debug.Log($"{property.name} in group {groupName}, isDrawing: {isDrawing}");
        // Debug.Log($"Path: {property.propertyPath}");

        // ✅ Always let Unity handle arrays and their children
        // if (property.isArray && property.propertyType != SerializedPropertyType.String)
        // {
        //     Debug.Log($"I foudn array: {property.name}");
        //     // return EditorGUI.GetPropertyHeight(property, true);
        //     return 100f;
        // }

        // if (property.propertyPath.Contains("Array.data["))
        // {
        //     // Debug.Log($"Array item: {property.name}, curntly: {isDrawing}");
        //     return EditorGUI.GetPropertyHeight(property, true);
        // }

        if (props.Count == 0)
        {
            SetupFold(property);
        }

        // If collapsed → only header
        if (!foldoutStates.TryGetValue(groupName, out bool expanded) || !expanded)
        {
            if (!property.name.Equals(props[0]))
            {
                return 0;
            }
            
            // Debug.Log($"create foldout heading");
            return EditorGUIUtility.singleLineHeight - props.Count * EditorGUIUtility.standardVerticalSpacing;
        }

        if (property.name.Equals(props[0]))
        {
            // // Debug.Log($"create foldout heading");
            float height = EditorGUIUtility.singleLineHeight;
            // float height = 0;
            foreach (var item in props)
            {
                float heightTemp = 0;
                var prop = property.serializedObject.FindProperty(item);
                // height += EditorGUI.GetPropertyHeight(prop, true);
                if (prop.isArray && prop.propertyType != SerializedPropertyType.String)
                {
                    float arrayHeight = EditorGUI.GetPropertyHeight(prop, true);
                    heights[item] = arrayHeight;
                    height += arrayHeight;
                    continue;
                }

                if (prop.hasVisibleChildren)
                {
                    var start = prop.Copy();
                    var end = start.GetEndProperty();
                    bool enterChild = true;
                    if (prop.isExpanded)
                    {
                        while (start.NextVisible(enterChild) && !SerializedProperty.EqualContents(start, end))
                        {
                            var childHeight = EditorGUI.GetPropertyHeight(start, true);
                            heightTemp += childHeight + EditorGUIUtility.standardVerticalSpacing;
                            // Debug.Log($"Child {start.name} height: {childHeight}");
                            enterChild = false;
                        }
                    }
                }
                heightTemp += EditorGUIUtility.singleLineHeight;
                heights[item] = heightTemp;
                height += heightTemp;
                // Debug.Log($"{item} height: {heightTemp}"); 
            }
            return height;
        }
        else
        {
            // Debug.Log($"{property.name} returns 0");
            return 0f;
        }
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var groupName = ((FoldoutGroupAttribute)attribute).groupName;

        if (property.name.Equals(props[0]))
        {
            foldoutStates[groupName] = EditorGUI.Foldout(
                            new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight),
                            foldoutStates[groupName], groupName, true);

            if (!foldoutStates[groupName])
                return;

            isDrawing = true;
            SetRect(position);
            DrawField(property);
            isDrawing = false;
        }

        if (isDrawing)
        {
            // Debug.Log($"Drawing {property.name}");
            var foundRect = rects.TryGetValue(property.name, out var item);
            if (!foundRect)
            {
                item = position;
            }
            PropertyField_Internal(item, property, true);
        }
    }

    private void SetupFold(SerializedProperty property)
    {
        var fieldInfos = property.serializedObject.targetObject.GetType()
        .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        foreach (var fieldInfo in fieldInfos)
        {
            // string path = property.propertyPath.Replace(property.name, fieldInfo.Name);
            string path = property.serializedObject.FindProperty(fieldInfo.Name).name;
            var attr = fieldInfo.GetCustomAttribute<FoldoutGroupAttribute>(false);
            if (attr != null)
            { 
                props.Add(path);
                // props.Add(fieldInfo.Name);
                
                if (!foldoutStates.ContainsKey(attr?.groupName))
                {
                    // foldoutStates[attr?.groupName] = false;
                    foldoutStates[attr?.groupName] = true;
                }
            }
        }
        
        // foreach (var item in props)
        // {
        //     Debug.Log($"Item: {item}");
        // }
    }

    private void SetRect(Rect rect)
    {
        rect.y += EditorGUIUtility.singleLineHeight;
        for (int i = 0; i < props.Count; i++)
        {
            var prop = props[i];
            rect.height = heights[prop];
            // Debug.Log($"Rect height: {rect.height}");
            rects[prop] = rect;
            rect.y += heights[prop] + EditorGUIUtility.standardVerticalSpacing;
        }
        // foreach (var item in rects)
        // {
        //     Debug.Log($"Rect debug {item.Key}: {item.Value}");
        // }
    }

    private void DrawField(SerializedProperty property)
    {
        foreach (var prop in props)
        {
            var thisProperty = property.serializedObject.FindProperty(prop);

            var rect = rects[prop];
            rect.height = heights[prop];
            PropertyField_Internal(rect, thisProperty, true);
        }
    }

    private void PropertyField_Internal(Rect rect, SerializedProperty property, bool includeChildren = true)
    {
        Debug.Log($"Drawing {property.name}");
        // var rng = Random.Range(0f, 1f) ;
        // EditorGUI.DrawRect(rect, new Color(0.2f, 1f, 0.2f, 0.3f * rng));
        EditorGUI.PropertyField(rect, property, includeChildren);
    }

}

[Serializable]
public class MyData
{
    public int x;
    public float y;
    public string z;
    // public int[] w;
}

[Serializable]
public class MyArray<T>
{
    public T[] items;
}