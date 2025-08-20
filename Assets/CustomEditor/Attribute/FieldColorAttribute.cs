using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Healm.Inspector
{
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = true)]
    public class FieldColorAttribute : PropertyAttribute
    {
        public Color m_backgroundColor;
        private static Color s_backgroundColor;
        public FieldColorAttribute(float r = 0, float g = 0, float b = 0, float a = 1)
        {
            m_backgroundColor = new Color(r, g, b, a);
        }
        public void Apply(Rect rect)
        {
            GUI.color = new(5, 5, 5, 1);
            s_backgroundColor = GUI.backgroundColor;
            GUI.backgroundColor = m_backgroundColor * 0.2f;
            EditorGUI.DrawRect(rect, m_backgroundColor * 0.8f);
        }

        public void Revert()
        {
            GUI.color = new(1, 1, 1, 1);
            GUI.backgroundColor = s_backgroundColor;
            s_backgroundColor = new(1,1,1,1);
        }
    }
}