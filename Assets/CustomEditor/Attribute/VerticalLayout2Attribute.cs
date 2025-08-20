using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
public class VerticalLayout2Attribute : GroupAttribute
{
    public string GroupName;
    public VerticalLayout2Attribute(string groupName)
    {
        GroupName = groupName;
    }
}
