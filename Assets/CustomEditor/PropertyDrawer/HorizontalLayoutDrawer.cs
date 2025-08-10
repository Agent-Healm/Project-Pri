using System.Xml.Schema;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(HorizontalLayoutAttribute))]
public class HorizontalLayoutDrawer : LayoutDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var attr = attribute as HorizontalLayoutAttribute;
        if (!attr.m_EOL)
        {
            return;
        }
        SetRect(position);
        DrawProperty(property);
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        AddVerticalFields(property.propertyPath);
        var attr = attribute as HorizontalLayoutAttribute;
        if (attr.m_EOL)
        {
            return EditorGUIUtility.singleLineHeight;
        }
        return - EditorGUIUtility.standardVerticalSpacing;
    }
}
