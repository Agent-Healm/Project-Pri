using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(VerticalLayoutAttribute))]
public class VerticalLayoutDrawer : LayoutDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var attr = attribute as VerticalLayoutAttribute;
        if (!attr.m_EOL)
        {
            return;
        }
        // position.y += EditorGUIUtility.singleLineHeight;
        SetRect(position, property);
        DrawProperty(property);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        AddPropertyDataWithPath(property.propertyPath);
        var attr = attribute as VerticalLayoutAttribute;
        // return attr.m_EOL
        //     ? EditorGUIUtility.singleLineHeight
        //     : EditorGUIUtility.singleLineHeight;
        // - EditorGUIUtility.standardVerticalSpacing
        // : EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        // : EditorGUIUtility.singleLineHeight + 2 * EditorGUIUtility.standardVerticalSpacing;

        if (attr.m_EOL)
        {
            return EditorGUIUtility.singleLineHeight;
        }
        // return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing * 1.5f;
        if (s_numberOfLines > 2)
        {
            // return EditorGUIUtility.singleLineHeight;
            return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing * 1;
        }
        return EditorGUIUtility.singleLineHeight * 2 + EditorGUIUtility.standardVerticalSpacing * 2;
        // return EditorGUIUtility.singleLineHeight * 2;
        // return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
    }
}