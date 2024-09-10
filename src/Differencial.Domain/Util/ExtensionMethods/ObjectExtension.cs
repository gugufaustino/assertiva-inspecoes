using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace Differencial.Domain.Util.ExtensionMethods
{
    public static class ObjectExtension
    {
        /// <summary>
        /// Faz a cópia de um objeto.
        /// Nao copia a referencia em memoria, apenas os dados.
        /// </summary>
        /// <typeparam name="T">Tipo de retorno.</typeparam>
        /// <param name="item">Objeto copiado.</param>
        /// <returns>objeto copiado.</returns>
        public static T DeepCloneBinarySerialization<T>(this object item)
        {
            throw new NotImplementedException("necessário mudar a forma de serialização");

            //if (item != null)
            //{
            //    BinaryFormatter formatter = new BinaryFormatter();
            //    MemoryStream stream = new MemoryStream();
            //    formatter.Serialize(stream, item);
            //    stream.Seek(0, SeekOrigin.Begin);
            //    T result = (T)formatter.Deserialize(stream);
            //    stream.Close();
            //    return result;
            //}
            //else
            //    return default(T);
        }


        public static T ShallowClone<T>(this T item)
            where T : ICloneable
        {
            if (item != null)
            {

                var obj = Activator.CreateInstance<T>();
                PropertyInfo[] props = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (var prop in props)
                {
                    var t = prop.PropertyType; 
                    if (prop.CanWrite && (t.IsPrimitive ||  t.IsValueType || t == typeof(string)))
                    {
                        var propName = prop.Name;

                        var valor = item.GetType().GetProperty(propName).GetValue(item, null);

                        prop.SetValue(obj, valor);
                    }
                }

                return obj;
            }
            else
                return default(T);
        }



        /// <summary>
        /// Gets the property value.
        /// </summary>
        /// <param name="src">The source.</param>
        /// <param name="propName">Name of the property.</param>
        /// <returns></returns>
        public static T GetPropertyValue<T>(this object src, string propName)
        {
            var value = src.GetType().GetProperty(propName).GetValue(src, null);
            return (T)Convert.ChangeType(value, typeof(T));
        }


        /// <summary>
        /// Gets an attribute on an enum field value
        /// </summary>
        /// <typeparam name="T">The type of the attribute you want to retrieve</typeparam>
        /// <param name="enumVal">The enum value</param>
        /// <returns>The attribute of type T that exists on the enum value</returns>
        /// <example>string desc = myEnumVariable.GetAttributeOfType<DescriptionAttribute>().Description;</example>
        public static T GetAttributeOfType<T>(this Enum enumVal) where T : Attribute
        {
            var type = enumVal.GetType();
            var memInfo = type.GetMember(enumVal.ToString());
            if (memInfo.Length == 0)
                throw new InvalidCastException("Não existe '" + type.Name + "' para o valor " + enumVal.ToString());

            var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
            return (attributes.Length > 0) ? (T)attributes[0] : null;
        }

        //TODO: refatorar -> criar uma classe AttributeExtension
        /// <summary>
        /// Retorna o DisplayAttribute
        /// </summary>
        /// <param name="enumVal"></param>
        /// <returns></returns>
        public static DisplayAttribute Display(this Enum enumVal)
        {
            var type = enumVal.GetType();
            var memInfo = type.GetMember(enumVal.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(DisplayAttribute), false);
            return (attributes.Length > 0) ? (DisplayAttribute)attributes[0] : null;
        }


        public static string SerializeXML<T>(this T obj)
        {
            var stringwriter = new StringWriter();
            var serializer = new XmlSerializer(typeof(T));
            serializer.Serialize(stringwriter, obj);
            return stringwriter.ToString();
        }

        public static string SerializeXML<T>(this List<T> lstObj)
        {
            var stringwriter = new StringWriter();
            var serializer = new XmlSerializer(typeof(List<T>));
            serializer.Serialize(stringwriter, lstObj);
            return stringwriter.ToString();
        }

        public static T DeserializeXML<T>(string obj)
        {
            var reader = new StringReader(obj);
            var serializer = new XmlSerializer(typeof(T));
            T objResult = (T)serializer.Deserialize(reader);
            return objResult;
        }

        public static bool IsNull<T>(this T obj) where T : class => obj == null;


    }
}