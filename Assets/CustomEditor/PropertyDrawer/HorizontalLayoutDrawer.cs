using System.Xml.Schema;
using UnityEditor;
using UnityEngine;
namespace Healm.EditorTools
{
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
            // property.serializedObject.Update();
            // EditorGUI.BeginProperty(position, label, property);
            SetRect(position);
            DrawProperty(property);
            // EditorGUI.EndProperty();
            property.serializedObject.ApplyModifiedProperties();
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
}