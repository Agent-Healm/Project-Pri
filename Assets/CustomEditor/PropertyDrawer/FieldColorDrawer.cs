using System.Collections;
using System.Collections.Generic;
using Codice.CM.Client.Gui;
using UnityEditor;
using UnityEngine;

namespace Healm.EditorTools
{
    [CustomPropertyDrawer(typeof(FieldColorAttribute))]
    public class FieldColorDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            FieldColorAttribute colorAttr = attribute as FieldColorAttribute;
            GUI.color = new(5, 5, 5, 1);
            GUI.backgroundColor = colorAttr.m_backgroundColor;

            EditorGUI.DrawRect(position, colorAttr.m_backgroundColor);
            EditorGUI.PropertyField(position, property, label, true);

            GUI.color = new(1, 1, 1, 1);
            GUI.backgroundColor = new(1, 1, 1, 1);
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
        
    }   
}
