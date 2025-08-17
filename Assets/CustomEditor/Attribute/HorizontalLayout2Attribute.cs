using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
public class HorizontalLayout2Attribute : PropertyAttribute
{
    public string GroupName;
    public HorizontalLayout2Attribute(string groupName)
    {
        GroupName = groupName;
    }
}
