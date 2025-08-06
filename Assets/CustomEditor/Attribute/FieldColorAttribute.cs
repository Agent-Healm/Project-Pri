using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Healm.EditorTools
{
    [AttributeUsage(System.AttributeTargets.Field, Inherited = false, AllowMultiple = true)]
    public class FieldColorAttribute : PropertyAttribute
    {
        public Color m_backgroundColor;
        public FieldColorAttribute(float r = 0, float g = 0, float b = 0 , float a = 1){
            m_backgroundColor = new Color(r, g, b, a);
        }
    }
}