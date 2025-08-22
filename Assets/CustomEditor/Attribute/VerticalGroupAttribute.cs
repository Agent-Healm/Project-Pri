using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Healm.Inspector{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
        public class VerticalGroupAttribute : GroupAttribute
        {
            public VerticalGroupAttribute(string groupName)
            {
                GroupName = groupName;
            }
        }
}