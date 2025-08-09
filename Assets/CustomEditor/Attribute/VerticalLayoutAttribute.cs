using System;
using System.Collections;
using System.Collections.Generic;
using Codice.Client.Common;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
public class VerticalLayoutAttribute : PropertyAttribute
{
    public string m_groupName;
    public bool m_EOL;

    public VerticalLayoutAttribute(string groupName, bool EOL = false)
    {
        m_groupName = groupName;
        m_EOL = EOL;
    }
}
