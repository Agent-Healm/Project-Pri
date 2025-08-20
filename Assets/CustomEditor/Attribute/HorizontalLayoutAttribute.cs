using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
public class HorizontalLayoutAttribute : GroupAttribute
{
    public string GroupName;
    public HorizontalLayoutAttribute(string groupName)
    {
        GroupName = groupName;
    }
}
