using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Healm.Inspector{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
    public class HorizontalGroupAttribute : GroupAttribute
    {
        public HorizontalGroupAttribute(string groupName)
        {
            GroupName = groupName;
        }
    }
}