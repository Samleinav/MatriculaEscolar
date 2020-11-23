using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToolsBaseApp
{
    public static class ConvertTo
    {

        public static T ToClass<T>(Dictionary<string, object> dict)
        {
            return (T)ConverToClass(dict, typeof(T));
        }
        private static object ConverToClass(Dictionary<string, object> dic, Type classToUse)
        {
            Type type = classToUse;
            var obj = Activator.CreateInstance(type);

            foreach (var item in dic)
            {
                var property = type.GetProperty(item.Key);
                if (property == null) continue;

                var value = item.Value;
                if (value is Dictionary<string, object> && !property.PropertyType.FullName.Contains("Generic.IList"))
                {
                    property.SetValue(obj, ConverToClass((Dictionary<string, object>)(item.Value), property.PropertyType));
                    continue;
                }
                if (property.PropertyType.FullName.Contains("Generic.IList"))
                {
                    var SubClassToUse = property.PropertyType.GetGenericArguments()[0];

                    Type genericListType = typeof(List<>);
                    Type concreteListType = genericListType.MakeGenericType(SubClassToUse);
                    var list = (IList)Activator.CreateInstance(concreteListType, new object[] { });

                    var values = (Dictionary<string, object>)dic[item.Key];

                    foreach (var itemClass in values)
                    {
                        list.Add(ConverToClass((Dictionary<string, object>)itemClass.Value, SubClassToUse));
                    }
                    property.SetValue(obj, list);
                    continue;
                }
                property.SetValue(obj, item.Value);
            }

            return obj;
        }

        public static Dictionary<string, object> ToDictionary(object Class)
        {
            return ConverToDictionary(Class);
        }
        private static Dictionary<string, object> ConverToDictionary(object datos)
        {
            var dict = new Dictionary<string, object>();

            foreach (var property in datos.GetType().GetProperties())
            {
                var Value = property.GetValue(datos, null);
                var NameProperty = property.Name;

                if (IsList(Value))
                {
                    var subDict = new Dictionary<string, object>();
                    var values = (IList)Value;
                    var count = 0;
                    foreach (var subItem in values)
                    {
                        var itemToAdd = ConverToDictionary(subItem);
                        if (itemToAdd.Count > 0)
                        {
                            var id = (itemToAdd.ContainsKey("Cedula") ? "Cedula" :
                                     (itemToAdd.ContainsKey("Nombre") ? "Nombre" :
                                     (itemToAdd.ContainsKey("Id") ? "Id" : count.ToString())));

                            subDict.Add((string)itemToAdd[id], itemToAdd);
                            count++;
                        }
                    }
                    dict.Add(NameProperty, subDict);
                    continue;
                }
                // is model class item
                var isClassModel = (Value != null ? Value.GetType().FullName.Contains("Models") : false);
                if (isClassModel)
                {
                    dict.Add(NameProperty, ConverToDictionary(Value));
                    continue;
                }
                dict.Add(NameProperty, Value);
            }
            return dict;
        }

        private static bool IsList(object o)
        {
            try
            {
                return o is IList &&
                   o.GetType().IsGenericType &&
                   o.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>));
            }
            catch
            {
                return false;
            }

        }

        public static int[] intToRGB(int color)
        {
            int[] colors = { 0, 0, 0 };

            colors[0] = ((color >> 16) & 0xff);
            colors[1] = ((color >> 8) & 0xff);
            colors[2] = ((color >> 0) & 0xff);

            return colors;
        }
    }
}