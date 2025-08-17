using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Healm.EditorTools
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
    public class LabelSizeAttribute : PropertyAttribute
    {
        public float LabelWidth { get; }
        public string LabelName { get; }
        private static float s_width;
        public LabelSizeAttribute(float labelWidth = 100f, string labelName = null)
        {
            LabelWidth = labelWidth;
            LabelName = labelName;
        }

        public void Apply()
        {
            s_width = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = LabelWidth;
        }
        public void Revert()
        {
            EditorGUIUtility.labelWidth = s_width;
            s_width = default;
        }
    }
}