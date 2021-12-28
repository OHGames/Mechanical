using System;
using System.Collections.Generic;
using System.Text;

namespace Mechanical
{
    /// <summary>
    /// The editor tag attribute will set tags to be used when serching for an entity class in the editor.
    /// </summary>
    public class EditorTagAttribute : Attribute
    {
        /// <summary>
        /// The tags.
        /// </summary>
        public string[] Tags { get; private set; }

        public EditorTagAttribute(string[] tags)
        {
            Tags = tags;
        }
    }
}
