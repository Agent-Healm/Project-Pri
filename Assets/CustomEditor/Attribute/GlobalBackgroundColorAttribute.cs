using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Healm.Inspector
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class GlobalBackgroundColorAttribute : PropertyAttribute
    {
        public Color m_backgroundColor;
        public GlobalBackgroundColorAttribute(float r = 0, float g = 0, float b = 0, float a = 1)
        {
            m_backgroundColor = new Color(r, g, b, a);
        }
    }
}