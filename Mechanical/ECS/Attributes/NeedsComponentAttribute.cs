using System;
using System.Collections.Generic;
using System.Text;

namespace Mechanical
{
    /// <summary>
    /// Add this attribute to any component that needs another component to work.
    /// 
    /// Until the component(s) that are needed is added, it will not be added.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class NeedsComponentAttribute : Attribute
    {
        public Type[] TypesNeeded { get; set; }

        public NeedsComponentAttribute(params Type[] components)
        {
            TypesNeeded = components;
        }

    }
}
