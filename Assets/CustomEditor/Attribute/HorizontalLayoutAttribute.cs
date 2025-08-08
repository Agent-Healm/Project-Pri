using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
public class HorizontalLayoutAttribute : PropertyAttribute
{
    public string m_groupName;
    public bool m_EOL;
    public HorizontalLayoutAttribute(string groupName, bool eol = false)
    {
        m_groupName = groupName;
        m_EOL = eol;
    }
}
