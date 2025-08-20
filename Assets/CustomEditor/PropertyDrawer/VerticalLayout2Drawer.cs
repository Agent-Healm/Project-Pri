using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(VerticalLayout2Attribute))]
public class VerticalLayout2Drawer : GroupDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // base.OnGUI(position, property, label);
        // var attr = attribute as VerticalLayout2Attribute;
        return;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var attr = attribute as VerticalLayout2Attribute;

        if (tree?.groupName != attr.GroupName)
        {
            return EditorGUIUtility.singleLineHeight;
        }
        return 0f; 
        // if (tree.children[0] is Leaf leaf) {
        //     if (property.name != leaf.id)
        //     {
        //         return 0f;
        //     }
        // }
        // return EditorGUIUtility.singleLineHeight;
        // return 0;
    }
}
