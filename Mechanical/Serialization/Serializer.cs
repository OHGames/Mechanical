/*
 * 
 * This file is part of the Mechanical Game Engine and is licensed under the MIT license.
 * https://github.com/OHGames/Mechanical
 * 
 * By O. H. Games
 * 
 * Note: some files contain code from other sources so see https://github.com/OHGames/Mechanical/blob/main/USED_CODE_LICENSES.txt for more info.
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using System.Reflection;

namespace Mechanical
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

        /// <summary>
        /// Serialize an object.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="obj">The object to serialize.</param>
        /// <param name="settings">The Data Contract settings.</param>
        /// <returns>A string with the serialized object.</returns>
        //https://stackoverflow.com/a/5010491 credit
        public static string Serialize<T>(T obj, DataContractSerializerSettings settings = null)
        {
            Type t = typeof(T);
            if (settings != null)
            {
                DataContractSerializer = new DataContractSerializer(t, settings);
            }
            else
            {
                DataContractSerializer = new DataContractSerializer(t, new DataContractSerializerSettings() { PreserveObjectReferences = true, KnownTypes = KnownTypes });
            }

            string serialized;

            using (MemoryStream ms = new MemoryStream())
            {
                DataContractSerializer.WriteObject(ms, obj);
                serialized = Encoding.UTF8.GetString(ms.ToArray());
            }

            return serialized;

        }

        /// <summary>
        /// Deserialize an object.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="data">The serialized data.</param>
        /// <param name="settings">The Data Contract settings.</param>
        /// <returns>The deserialized object.</returns>
        //https://stackoverflow.com/a/5010491 credit
        public static T Deserialize<T>(string data, DataContractSerializerSettings settings = null)
        {
            Type t = typeof(T);
            if (settings != null)
            {
                DataContractSerializer = new DataContractSerializer(t, settings);
            }
            else
            {
                DataContractSerializer = new DataContractSerializer(t, new DataContractSerializerSettings() { PreserveObjectReferences = true, KnownTypes = KnownTypes });
            }

            T obj;

            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(data)))
            {
                XmlDictionaryReader xml = XmlDictionaryReader.CreateTextReader(ms, Encoding.UTF8, new XmlDictionaryReaderQuotas(), null);
                obj = (T)DataContractSerializer.ReadObject(xml);
            }

            return obj;

        }

        /// <summary>
        /// Manually add a type that will be recognised.
        /// </summary>
        /// <param name="type">The type to add.</param>
        public static void AddType(Type type)
        {
            if (!KnownTypes.Contains(type))
            {
                KnownTypes.Add(type);
            }
        }

        /// <summary>
        /// Manually remove a type to be removed.
        /// </summary>
        /// <param name="type">The type to remove.</param>
        public static void RemoveType(Type type)
        {
            if (KnownTypes.Contains(type))
            {
                KnownTypes.Remove(type);
            }
        }

        /// <summary>
        /// Get all of the types in the assemblies that inherit <see cref="DataContractAttribute"/> and add them to the list of types to serialize. 
        /// </summary>
        /// <param name="assemblies">The list of assembalies that will be added to the typelist.</param>
        public static void GrabTypes(Assembly[] assemblies)
        {
            assemblies.Concat(new Assembly[] { Assembly.GetExecutingAssembly() });

            // loop through assembalies.
            for (int i = 0; i < assemblies.Length; i++)
            {
                Type[] types = assemblies[i].GetTypes();
                for (int j = 0; j < types.Length; j++)
                {
                    if (types[j].GetCustomAttribute<DataContractAttribute>() != null)
                    {
                        if (!KnownTypes.Contains(types[j]))
                        {
                            KnownTypes.Add(types[j]);
                        }
                    }
                }
            }
        }

    }
}
