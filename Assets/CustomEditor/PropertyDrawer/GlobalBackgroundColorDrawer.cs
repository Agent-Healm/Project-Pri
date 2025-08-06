
using System;
using UnityEditor;
using UnityEngine;

namespace Healm.EditorTools
{
    [CustomPropertyDrawer(typeof(int), true)]
    [CustomPropertyDrawer(typeof(float), true)]
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
                attr.m_backgroundColor *= 1.3f;
                // GUI.color = new(1.5f, 1.5f, 1.5f ,1);
                GUI.backgroundColor = attr.m_backgroundColor * 0.2f;
                EditorGUI.DrawRect(position, attr.m_backgroundColor * 0.6f);
                EditorGUI.PropertyField(position, property, label, true);

                GUI.backgroundColor = l_originalColor;
                // GUI.color = new(1,1,1,1);
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