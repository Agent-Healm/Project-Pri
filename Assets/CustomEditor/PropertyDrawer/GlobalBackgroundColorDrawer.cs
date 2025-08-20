
using System;
using UnityEditor;
using UnityEngine;

namespace Healm.Inspector
{
    [CustomPropertyDrawer(typeof(int), true)]
    [CustomPropertyDrawer(typeof(float), true)]
    [CustomPropertyDrawer(typeof(Color), true)]
    [CustomPropertyDrawer(typeof(Sprite), true)]
    [CustomPropertyDrawer(typeof(MonoBehaviour), true)]
    [CustomPropertyDrawer(typeof(SerializedProperty), true)]
    public class GlobalBackgroundColorDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            UnityEngine.Object targetObject = property.serializedObject.targetObject;

            var type = targetObject.GetType();
            var attr = Attribute.GetCustomAttribute(type, typeof(GlobalBackgroundColorAttribute)) as GlobalBackgroundColorAttribute;

            if (attr != null)
            {
                Color l_originalColor = GUI.backgroundColor;
                Color l_originalContentColor = GUI.contentColor;
                // GUI.color = new(1,1,1,1);
                GUI.backgroundColor = attr.m_backgroundColor * 0.2f;
                attr.m_backgroundColor *= 1.3f;
                // GUI.contentColor = (attr.m_backgroundColor.r + attr.m_backgroundColor.g + attr.m_backgroundColor.b)

                EditorGUI.DrawRect(position, attr.m_backgroundColor * 0.6f);
                EditorGUI.PropertyField(position, property, label, true);

                GUI.backgroundColor = l_originalColor;
                GUI.contentColor = l_originalContentColor;
            }
            else
            {
                EditorGUI.PropertyField(position, property, label, true);
            }
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label);
        }
    }
}