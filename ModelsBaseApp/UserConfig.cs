using Couchbase.Lite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ModelsBaseApp
{
    public class UserConfig
    {
        public string Id { get; set; }

        public ClassConfigUsuario Usuario { get; set; }
        public string Type { get { return "UserConfig"; } set { } }
        public Dictionary<string, object> ToDictionary()
        {
            var ThisConfig = this.GetType()
                                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                                .ToDictionary(prop => prop.Name, prop => prop.GetValue(this, null));

            MutableDocument datos = new MutableDocument("datos");

            //for (int i = 0; i < FechaMatricula.Count; i++)
            //{
            //    var MutableItem = new MutableDictionaryObject(FechaMatricula[i].GetType()
            //                                                        .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            //                                                        .ToDictionary(prop => prop.Name, prop => prop.GetValue(FechaMatricula[i], null)));

            //    datos.SetDictionary(FechaMatricula[i].Id.ToString(), MutableItem);
            //}
            ThisConfig["FechaMatricula"] = datos.ToDictionary();
            return ThisConfig;

        }
        public MutableDictionaryObject ToMutableDictionary()
        {
            return new MutableDictionaryObject(this.ToDictionary());
        }
    }

    public class ClassConfigUsuario
    {
        public string Nombre { get; set; }
        public string Cedula { get; set; }
        public string Ocupacion { get; set; }
        public string Telefono { get; set; }
        public string CorreoElectronico { get; set; }

    }
}
