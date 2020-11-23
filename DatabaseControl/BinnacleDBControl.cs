using Couchbase.Lite;
using ModelsBaseApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolsBaseApp;

namespace DatabaseControl
{
    public class BinnacleDBControl : IDisposable
    {
        private Database Database { get; set; }
        private Database DatabaseConfig { get { return new Database("config", new DatabaseConfiguration() { Directory = AppDomain.CurrentDomain.BaseDirectory + "Config" }); }} 
        private UserConfig Config { get; set; }
        private MutableDictionaryObject docOnLoad { get; set; }
        private string IdBinnacleCreated { get; set; }
        public string IdBinnacle { get { return IdBinnacleCreated; } }
        private bool IsBinnacleSaved { get; set; } = false;
        public bool IsBinnacleCreated { get { return IsBinnacleSaved; } }
        public bool IsConfigLoad { get => Config != null; }


        public BinnacleDBControl(string action)
        {
            var userInfo = DatabaseConfig.GetDocument("config");
            if (userInfo != null)
            {
                var dic = AppDomain.CurrentDomain.BaseDirectory;
                Database = new Database("binnacle", new DatabaseConfiguration() { Directory = dic + "Config/Bitacora" });

                Config = ConvertTo.ToClass<UserConfig>(userInfo.ToDictionary());

                using (var log = new MutableDocument())
                {
                    log.SetString("timeLoadConfig", DateTime.Now.ToString());
                    log.SetString("action", action);
                    docOnLoad = new MutableDictionaryObject(log.ToDictionary());
                }
            }
        }
        public void Add_Binnacle_Event(MutableDocument doc = null, TypeBinnacle Type = TypeBinnacle.Consult)
        {
            if (IsConfigLoad && doc != null && IdBinnacleCreated == null)
            {
                var newDoc = new MutableDocument();

                newDoc.SetDictionary("docData", new MutableDictionaryObject(doc.ToDictionary()));
                newDoc.SetDate("createAt", DateTime.Now);
                newDoc.SetString("eventType", Enum.GetName(typeof(TypeBinnacle), Type));
                newDoc.SetString("user", Config.Usuario.Cedula);
                newDoc.SetString("DocId", doc.Id);
                newDoc.SetDictionary("Onload", docOnLoad);
                Database.Save(newDoc);
                IsBinnacleSaved = true;
                IdBinnacleCreated = newDoc.Id;
            }
        }

        public void Dispose()
        {
            if (IsConfigLoad)
            {
                Database.Dispose();
            }
            DatabaseConfig.Dispose();
        }
    }

}
