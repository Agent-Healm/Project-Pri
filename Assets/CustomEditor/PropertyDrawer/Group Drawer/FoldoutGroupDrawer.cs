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
            
            Debug.Log($"create foldout heading");
            return EditorGUIUtility.singleLineHeight - props.Count * EditorGUIUtility.standardVerticalSpacing;
        }

        // only first gets height
        // Debug.Log($"Returned default height");
        if (property.name.Equals("data"))
        {
            return EditorGUIUtility.singleLineHeight;
            // return 100f;
        }
        if (property.name.Equals(props[0]))
        {
            // // Debug.Log($"create foldout heading");
            float height = EditorGUIUtility.singleLineHeight;
            foreach (var item in props)
            {
                float heightTemp = 0;
                var prop = property.serializedObject.FindProperty(item);
                // 
                // height += EditorGUI.GetPropertyHeight(prop, true);
                if (prop.hasVisibleChildren)
                {
                    // height += 0;
                    // height += EditorGUIUtility.singleLineHeight;
                    // Debug.Log($"{prop.name} has children");
                    // var customHeight = EditorGUI.GetPropertyHeight(prop, true);
                    // Debug.Log($"Height with children: {customHeight}");

                    // height += 0;

                    var start = prop.Copy();
                    var end = start.GetEndProperty();

                    // Debug.Log($"Start: {start.name} end: {end.name}");
                    bool enterChild = true;
                    // if (prop.isExpanded)
                    // Debug.Log($"expanded: {prop.isExpanded}");
                    if (prop.isExpanded)
                    {
                        // Debug.Log($"{prop.name} is a {prop.propertyType}");
                        // if (prop.propertyType == SerializedPropertyType.Generic)
                        // {
                        // Debug.Log($"{prop.name} is an array? : {prop.isArray}");
                        if (prop.isArray)
                        {
                            // heightTemp += 80f;
                            // var tempHeight = EditorGUI.GetPropertyHeight(prop, true);
                            // Debug.Log($"array height: {tempHeight}");
                            // heightTemp += tempHeight;
                            // heightTemp += 120f;

                            // var sizeProp = prop.FindPropertyRelative("Array.size");
                            // heightTemp += EditorGUI.GetPropertyHeight(sizeProp, true) + EditorGUIUtility.standardVerticalSpacing;

                            // Debug.Log($"array.size size: {EditorGUI.GetPropertyHeight(sizeProp, true)}");
                            // Debug.Log($"This array has sizeof: {prop.arraySize}");

                            // for (int i = 0; i < prop.arraySize; i++)
                            {
                                // var element = prop.GetArrayElementAtIndex(i);
                                // Debug.Log($"child-{i} size: {EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing}");
                                // height += EditorGUI.GetPropertyHeight(element, true) + EditorGUIUtility.standardVerticalSpacing;
                                // heightTemp += EditorGUI.GetPropertyHeight(element, true) + EditorGUIUtility.standardVerticalSpacing;
                                // var propHeight = EditorGUIUtility.singleLineHeight;

                                // var propHeight = EditorGUI.GetPropertyHeight(prop, true);
                                // // var propHeight = 0f;
                                // Debug.Log($"total child height: {propHeight}");
                                // heightTemp += propHeight + EditorGUIUtility.standardVerticalSpacing;
                            }
                            // Debug.Log($"Array height total: {heightTemp} ");

                            while (start.NextVisible(enterChild) && !SerializedProperty.EqualContents(start, end))
                            {
                                var childHeight = EditorGUI.GetPropertyHeight(start, true);
                                heightTemp += childHeight + EditorGUIUtility.standardVerticalSpacing;
                                Debug.Log($"Child {start.name} height: {childHeight}");
                                enterChild = false;
                            }
                        }
                        else
                        {
                            while (start.NextVisible(enterChild) && !SerializedProperty.EqualContents(start, end))
                            {
                                var childHeight = EditorGUI.GetPropertyHeight(start, true);
                                heightTemp += childHeight + EditorGUIUtility.standardVerticalSpacing;
                                // Debug.Log($"Child {start.name} height: {childHeight}");
                                enterChild = false;
                            }
                        }
                        // }
                    }
                }
                // else
                // {
                heightTemp += EditorGUIUtility.singleLineHeight
                ;

                heights[item] = heightTemp;
                height += heightTemp;
                // }
            }
            return height;
        }
        else { return 0f; }
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

            SetRect(position);
            // Debug.Log($"I only executed once from {property.name}");
            DrawField(property);
        }

        // if (!foldoutStates[groupName])
        //     return;

        if (isDrawing)
        {
            // Debug.Log($"Drawing {property.name}");
            var foundRect = rects.TryGetValue(property.name, out var item);
            if (!foundRect)
            {
                Debug.Log($"Rects for {property.name} not found");
                item = position;
                // item.height = 80f;
                if (property.name.Equals("data"))
                {
                    // item.height = 100f;
                    // item = rects["size"];
                    Debug.Log($"Array item: {property}");
                    // return;
                }
            }
            // if (property.isArray)
            // {
            //     PropertyField_Internal(item, property, true);
            // }
            PropertyField_Internal(item, property, true);
        }

        // if (property.name.Equals(props[0]))
        // {
        //     SetRect(position);
        //     Debug.Log($"I only executed once from {property.name}");
        //     DrawField(property);
        // }
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
        isDrawing = true;
        foreach (var prop in props)
        {
            Debug.Log($"Drawing for {prop}");
            var thisProperty = property.serializedObject.FindProperty(prop);
            var rect = rects[prop];
            rect.height = heights[prop];
            Debug.Log($"{prop} has height: {heights[prop]}");
            PropertyField_Internal(rect, thisProperty, true);
        }
        isDrawing = false;
    }

    private void PropertyField_Internal(Rect rect, SerializedProperty property, bool includeChildren = true)
    {
        // Debug.Log($"Drawing {property.name} with {rect}");
        if (property.isArray && property.propertyType != SerializedPropertyType.String)
        {
            // Debug.Log($"try skipping array drawing");
            // Debug.Log($"{property.name} debug: {rect}");
            // return;

            float y = rect.y;
            float x = rect.x;
            float width = rect.width;

            // Header
            Rect headerRect = new Rect(x, y, width, EditorGUIUtility.singleLineHeight);
            property.isExpanded = EditorGUI.Foldout(headerRect, property.isExpanded, property.displayName, true);
            y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            if (!property.isExpanded)
                return;

            // Size field
            var sizeProp = property.FindPropertyRelative("Array.size");
            float sizeHeight = EditorGUI.GetPropertyHeight(sizeProp, true);
            // Debug.Log($"size height: {sizeHeight}");
            EditorGUI.PropertyField(new Rect(x, y, width, sizeHeight), sizeProp, true);
            y += sizeHeight + EditorGUIUtility.standardVerticalSpacing;

            // Elements
            for (int i = 0; i < property.arraySize; i++)
            {
                var element = property.GetArrayElementAtIndex(i);
                // Debug.Log($"child name: {element.name}");
                float elemHeight = EditorGUI.GetPropertyHeight(element, true);
                // float elemHeight = EditorGUIUtility.singleLineHeight;
                // Debug.Log($"child-{i} height: {elemHeight}");
                EditorGUI.PropertyField(new Rect(x, y, width, elemHeight), element, true);
                y += elemHeight + EditorGUIUtility.standardVerticalSpacing;
            }

        }
        else
        {
            
        EditorGUI.PropertyField(rect, property, includeChildren);
        }
    }







    private float GetGroupHeight(SerializedProperty property, string groupName)
    {
        float height = EditorGUIUtility.singleLineHeight;
        foreach (var item in props)
        {
            height += EditorGUI.GetPropertyHeight(property.serializedObject.FindProperty(item));
        }
        // var iterator = property.serializedObject.GetIterator();
        // iterator.NextVisible(true);

        // do
        // {
        //     var fi = property.serializedObject.targetObject
        //         .GetType()
        //         .GetField(iterator.name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        //     if (fi == null) continue;

        //     var attr = (FoldoutGroupAttribute)System.Attribute.GetCustomAttribute(fi, typeof(FoldoutGroupAttribute));
        //     if (attr != null && attr.groupName == groupName)
        //         height += EditorGUI.GetPropertyHeight(iterator, true) + 2;
        // }
        // while (iterator.NextVisible(false));

        return height;
        // }
    }

    // Utility: count number of fields with same group
    private int CountGroupFields(SerializedProperty property, string groupName)
    {
        int count = 0;
        var iterator = property.serializedObject.GetIterator();
        iterator.NextVisible(true);

        do
        {
            var attrs = fieldInfo.GetCustomAttributes(typeof(FoldoutGroupAttribute), false);
            foreach (var attr in attrs)
            {
                if (((FoldoutGroupAttribute)attr).groupName == groupName)
                {
                    count++;
                    break;
                }
            }
        }
        while (iterator.NextVisible(false));

        return count;
    }

    // Reset before repaint so groups can be redrawn
    // [InitializeOnLoadMethod]
    private static void ResetDrawnGroups()
    {
        // EditorApplication.update += () => drawnGroups.Clear();

    }
}

[Serializable]
public class MyData
{
    public int x;
    public float y;
    public string z;
}