using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Healm.Inspector{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
    public class HorizontalLayoutAttribute : GroupAttribute
    {
        public HorizontalLayoutAttribute(string groupName)
        {
            GroupName = groupName;
        }
    }
}