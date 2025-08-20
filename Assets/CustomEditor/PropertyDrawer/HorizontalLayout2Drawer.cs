using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(HorizontalLayout2Attribute))]
public class HorizontalLayout2Drawer : GroupDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Test();
        var attr = attribute as HorizontalLayout2Attribute;

        if (tree == null)
        {
            SetupTree(property);
        }

        // var props = tree.children;

        if (property.name != (tree.children[0] as Leaf).id)
        {
            return;
            // if (!isDrawn)
            // {
            //     // EditorGUI.PropertyField(position, property, false);
            // }
            // return;
        }

        // isDrawn = false;
        EditorGUIUtility.labelWidth = 50;
        SetRect(position, attr.GroupName);
        DrawField2(property, tree);

        // for (int i = 0; i < tree.children.Count; i++)
        // {
        //     string propName;
        //     if (tree.children[i] is Leaf leaf)
        //     {
        //         propName = leaf.id;
        //     }
        //     else
        //     {
        //         continue;
        //     }
        //     var path = property.propertyPath.Replace(property.name, propName);
        //     var seriProp = property.serializedObject.FindProperty(path);

        //     var rect = outRects[propName];
        //     EditorGUI.DrawRect(rect, new(0, 0.1f * (i + 1), 0.2f * (i + 1), 0.5f));
        //     // EditorGUI.PropertyField(rect, seriProp, false);

        //     if (i == tree.children.Count - 1)
        //     {
        //         isDrawn = true;
        //     }
        // }
        // DrawField(property, tree, new(position.x, position.y, position.width, 80));
        // DrawField2(property, tree);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var attr = attribute as HorizontalLayout2Attribute;

        if (tree?.groupName != attr.GroupName)
        {
            // return EditorGUIUtility.singleLineHeight;
            return 100f; 
        }
        if (tree.children[0] is Leaf leaf) {
            if (property.name != leaf.id)
            {
                return 0f;
            }
        }
        return EditorGUIUtility.singleLineHeight;
    }

    private void Test()
    {
        
        var root =
        GroupLayout.Horizontal(
            "ok",
            GroupLayout.Field("x1"),
            GroupLayout.Vertical(
                "ok",
                GroupLayout.Field("x2"),
                GroupLayout.Horizontal(
                    "ok",
                    GroupLayout.Field("x3"),
                    GroupLayout.Vertical(
                        "ok",
                        GroupLayout.Field("x4"),
                        GroupLayout.Field("x5")
                    )
                ),
                GroupLayout.Field("x7")
            ),
            GroupLayout.Field("x6")
        );

        var rects = new Dictionary<string, Rect>();
        GroupLayout.Compute(root, new Rect(0, 0, 1000, 600), rects);

        foreach (var kv in rects)
            Debug.Log($"{kv.Key} -> {kv.Value}");
    }
}
