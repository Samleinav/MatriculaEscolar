using Couchbase.Lite;
using ModelsBaseApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ToolsBaseApp;

namespace DatabaseControl
{
    public class ConfigDBControl
    {
        public ConfigDBControl()
        {
            ConfigDB = new Database("config", new DatabaseConfiguration() { Directory = AppDomain.CurrentDomain.BaseDirectory + "Config" });
        }

        private Database ConfigDB { get; set; }
        private Document ConfigDoc { get { return ConfigDB.GetDocument("config"); } }
        private Document ConfigGlobal { get { return ConfigDB.GetDocument("global"); } }
        private string LastConfigId { get { return (ConfigExits ? ConfigDoc.GetString("IdLastConfig") : null); } }
        public bool ConfigExits { get { return (ConfigDoc != null); } }

        private MethodResponse<UserConfig> SetConfig(UserConfig config, string Event = "Update_Config")
        {
            var mutableDoc = new MutableDocument("config");
            
            try
            {
                if (ConfigDoc != null)
                {
                    mutableDoc = ConfigDoc.ToMutable();
                }

                mutableDoc.SetDictionary("config",new MutableDictionaryObject(config.ToDictionary()));

                if (Event == "Create_Config")
                {
                    mutableDoc.SetDate("createAt", DateTime.Now);
                }
                else
                {
                    var lastConfig = new MutableDocument();
                    mutableDoc.SetDate("updateAt", DateTime.Now);
                    lastConfig.SetDate("updateAt", DateTime.Now);

                    lastConfig.SetDate("deteleteAt", DateTime.Now);
                    
                    ConfigDB.Save(lastConfig);
                    mutableDoc.SetString("IdLastConfig", lastConfig.Id);
                }

                ConfigDB.Save(mutableDoc);
                return new MethodResponse<UserConfig>()
                {
                    result = true,
                    type = "success",
                    MSG = ResMSG.Config.CONFIGURACION_GUARDADA,
                    Data = config
                };
            }
            catch
            {
                return new MethodResponse<UserConfig>()
                {
                    result = false,
                    type = "Error",
                    MSG = ResError.NO_SE_PUDO_GUARDAR,
                    Data = null
                };
            }
        }
        public MethodResponse<UserConfig> CreateConfig(UserConfig config)
        {
            return SetConfig(config, "Create_Config");
        }
        public MethodResponse<UserConfig> UpdateConfig(UserConfig config)
        {
            if(ConfigExits) return SetConfig(config);
            return new MethodResponse<UserConfig>()
            {
                result = false,
                type = "error",
                MSG = ResError.NO_EXISTE_CONFIGURACION,
                Data = null
            };
        }
        public MethodResponse<UserConfig> RestoreConfig()
        {
            if(ConfigExits)
            {
                using(var doc = ConfigDB.GetDocument(LastConfigId))
                {
                    return SetConfig(ConvertTo.ToClass<UserConfig>((Dictionary<string, object>)doc.ToDictionary()["config"]));
                     
                }
                
            }
            return new MethodResponse<UserConfig>()
            {
                result = false,
                type = "error",
                MSG = ResError.NO_EXISTE_CONFIGURACION,
                Data = null
            };
        }
        public UserConfig GetConfig
        {
            get{
                return (ConfigExits ? ConvertTo.ToClass<UserConfig>(ConfigDoc.ToDictionary()) : new UserConfig());
            }
        } 

        private MethodResponse<ConfigBase> SetGlobalConfig(ConfigBase config)
        {
            using(var globalConfig = new MutableDocument("global"))
            {
                globalConfig.SetData(ConvertTo.ToDictionary(config));
                ConfigDB.Save(globalConfig);
                return new MethodResponse<ConfigBase>()
                {
                    result = true,
                    type = "success",
                    MSG = ResMSG.Config.CONFIGURACION_GUARDADA,
                    Data = config
                };
            }
        }
        public MethodResponse<ConfigBase> SaveGlobalConfig(ConfigBase config)
        {
            return SetGlobalConfig(config);
        }
        public ConfigBase GetGlobalConfig
        {
            get
            {
                return (ConfigGlobal != null ? ConvertTo.ToClass<ConfigBase>(ConfigGlobal.ToDictionary()) : new ConfigBase());
            }
        }
    }
}
