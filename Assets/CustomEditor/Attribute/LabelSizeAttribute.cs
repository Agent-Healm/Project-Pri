using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
public class LabelSizeAttribute : PropertyAttribute
{
    public float LabelWidth { get; }
    public string LabelName { get; }
    public LabelSizeAttribute(float labelWidth = 100f, string labelName = null)
    {
        LabelWidth = labelWidth;
        LabelName = labelName;
    }
}
