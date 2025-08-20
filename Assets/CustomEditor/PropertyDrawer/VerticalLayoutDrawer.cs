using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Healm.Inspector
{
    [CustomPropertyDrawer(typeof(VerticalLayoutAttribute))]
    public class VerticalLayoutDrawer : GroupDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            return;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return GetPropertyHeight_Internal(property);
        }
    }
}