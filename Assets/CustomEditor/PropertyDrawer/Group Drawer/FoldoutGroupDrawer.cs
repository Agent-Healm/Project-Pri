using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Codice.Client.Common;
using Unity.Properties;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(FoldoutGroupAttribute), false)]
public class FoldoutGroupDrawer : PropertyDrawer
{
    // Track which groups are expanded
    private static Dictionary<string, bool> foldoutStates = new();

    // Track if we've already drawn a group in this frame
    private static HashSet<string> drawnGroups = new();

    private static List<string> props = new();

    private static bool isDrawing = false;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var groupName = ((FoldoutGroupAttribute)attribute).groupName;

        // Skip if already drawn
        if (drawnGroups.Contains(groupName)){
            Debug.Log($"Drawing field");
            return EditorGUIUtility.singleLineHeight;
        }
        // return 0f; 

        SetupFold(property);

        // If collapsed → only header
        if (!foldoutStates.TryGetValue(groupName, out bool expanded) || !expanded)
        {
            Debug.Log($"create foldout heading");
            return EditorGUIUtility.singleLineHeight;
        }
        // Expanded → sum heights of only fields in this group
        // Debug.Log($"Returned default height");
        // return GetGroupHeight(property, groupName);
        return 36f;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var groupName = ((FoldoutGroupAttribute)attribute).groupName;
        // if ()            EditorGUI.PropertyField(position, property);
        if (property.name.Equals(props[0]))
        {
            foldoutStates[groupName] = EditorGUI.Foldout(
                new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight),
                foldoutStates[groupName], "FoldOutGroup", true);
            
        }
 
        // if (isDrawing || foldoutStates[groupName])
        if (isDrawing || foldoutStates[groupName])
        {
            var rect = position;
            rect.y += EditorGUIUtility.singleLineHeight;
            // EditorGUI.DrawRect(rect, new(0.5f, 1, 0));
            EditorGUI.PropertyField(rect, property);
            return;
        }

        if (!foldoutStates[groupName])
            return;

        // Draw all fields in this group
        EditorGUI.indentLevel++;

        isDrawing = true;
        float y = position.y + EditorGUIUtility.singleLineHeight + 2;
        foreach (var prop in props)
        {
            Debug.Log($"Drawing for {prop}");
            var thisProperty = property.serializedObject.FindProperty(prop);
            var rect = position;
            rect.y = y + 40f;
            // rect.height = EditorGUI.GetPropertyHeight(thisProperty, true);
            rect.height = EditorGUIUtility.singleLineHeight;
            Debug.Log($"{thisProperty.name} height: {rect.height}");
            // y += 
            // float height = EditorGUI.GetPropertyHeight(property, true);
            // EditorGUI.PropertyField(new(position.x, position.y, position.width, height), iterator, true);
            // y += height + 2;
            // EditorGUI.PropertyField(rect, property, false);
            EditorGUI.PropertyField(rect, thisProperty, false);
            y += rect.height + 2;
            Debug.Log($"Rect: {rect}");
        }
        EditorGUI.indentLevel--;
        isDrawing = false;

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
    [InitializeOnLoadMethod]
    private static void ResetDrawnGroups()
    {
        // EditorApplication.update += () => drawnGroups.Clear();

    }
}
