using UnityEditor;
using UnityEngine;

namespace Healm.EditorTools
{
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
            SetRect(position);
            DrawProperty(property);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            AddPropertyDataWithPath(property.propertyPath);
            var attr = attribute as VerticalLayoutAttribute;
            if (attr.m_EOL)
            {
                return EditorGUIUtility.singleLineHeight;
            }
            if (s_numberOfLines > 2)
            {
                return EditorGUIUtility.singleLineHeight;
            }
            return EditorGUIUtility.singleLineHeight * 2 + EditorGUIUtility.standardVerticalSpacing * 1;
        }
    }
}