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
            colorAttr.Apply(position);
            EditorGUI.PropertyField(position, property, label, true);

            colorAttr.Revert();
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

    }   
}
