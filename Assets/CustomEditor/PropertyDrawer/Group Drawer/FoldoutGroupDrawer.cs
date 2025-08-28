using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Codice.Client.Common;
using PlasticGui.WorkspaceWindow.Topbar;
using Unity.Properties;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(FoldoutGroupAttribute), false)]
public class FoldoutGroupDrawer : PropertyDrawer
{
    // Track which groups are expanded
    private static Dictionary<string, bool> foldoutStates = new();

    // Track if we've already drawn a group in this frame
    // private static HashSet<string> drawnGroups = new();
    // private static Rect firstRect;
    private static Dictionary<string, Rect> rects = new();
    private static List<string> props = new();

    private static bool isDrawing = false;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var groupName = ((FoldoutGroupAttribute)attribute).groupName;

        // Skip if already drawn
        // if (drawnGroups.Contains(groupName)){
        //     Debug.Log($"Drawing field");
        //     return EditorGUIUtility.singleLineHeight;
        // }
        // return 0f; 
        if (props.Count == 0)
        {
            SetupFold(property);
        }

        // If collapsed → only header
        if (!foldoutStates.TryGetValue(groupName, out bool expanded) || !expanded)
        {
            // Debug.Log($"prop count: {props.Count}");
            if (property.name.Equals(props[0]))
            {
                Debug.Log($"create foldout heading");
                return EditorGUIUtility.singleLineHeight - (props.Count) * EditorGUIUtility.standardVerticalSpacing;
            }
            else { return 0f; }
        }
        // Expanded → sum heights of only fields in this group
        Debug.Log($"Returned default height");
        // return GetGroupHeight(property, groupName);
        // return EditorGUIUtility.singleLineHeight * (props.Count + 1);
        // return 36f;
        // return 18f;
        // return 0f;
        if (property.name.Equals(props[0]))
        {
            Debug.Log($"create foldout heading");
            // return EditorGUIUtility.singleLineHeight;
            return EditorGUIUtility.singleLineHeight * (props.Count + 1);

            // return 36f;
        }
        else { return 0f; }
        // else { return 18f; }
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var groupName = ((FoldoutGroupAttribute)attribute).groupName;

        if (property.name.Equals(props[0]))
        {
            foldoutStates[groupName] = EditorGUI.Foldout(
                new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight),
                foldoutStates[groupName], "FoldOutGroup", true);
        }

        if (!foldoutStates[groupName])
            return;

        if (isDrawing)
        {
            var rect = rects[property.name];
            EditorGUI.PropertyField(rect, property);
        }
        

        if (property.name.Equals(props[0]))
        {
            SetRect(position);
            Debug.Log($"I only executed once from {property.name}");
            DrawField(property);
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
            rects[prop] = rect;
            rect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
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
            EditorGUI.PropertyField(rect, thisProperty, false);
        }
        isDrawing = false;
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
