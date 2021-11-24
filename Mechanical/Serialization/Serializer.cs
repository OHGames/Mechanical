using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization;

namespace Mechanical.Serialization
{
    /// <summary>
    /// This class is a wrapper for the <see cref="DataContractSerializer"/>. (because its funky!!!) >:O
    /// </summary>
    public static class Serializer
    {

        /// <summary>
        /// A list of types that need to be serialized.
        /// </summary>
        public static List<Type> KnownTypes = new List<Type>();

        /// <summary>
        /// The actual serializer.
        /// </summary>
        public static DataContractSerializer DataContractSerializer;

        public void Serialize<T>(T obj, DataContractSerializerSettings settings = null)
        {
            Type t = typeof(T);
            if (settings != null)
            {
                DataContractSerializer = new DataContractSerializer(t, settings);
            }
            else
            {
                DataContractSerializer = new DataContractSerializer(t, KnownTypes);
            }
        }

    }
}
