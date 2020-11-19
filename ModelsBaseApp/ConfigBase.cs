using System;
using System.Collections.Generic;
using System.Text;

namespace ModelsBaseApp
{
    public class ConfigBase
    {
        private static string version = "1.0.0t";
        private static string mainserver = "https://server.com";

        public bool IsFirstLoadProgram { get; set; } = true;
        public string UserID { get; set; }
        public string Version { get { return version; } set { } }
        public string Type { get { return "GlobalConfig"; } set { } }
        public string LocalServer { get; set; }
        public string LocalCredencial { get; set; }
        public string MainServer { get { return mainserver; } set { } }
        public string MainCredencial { get; set; }
    }
}
