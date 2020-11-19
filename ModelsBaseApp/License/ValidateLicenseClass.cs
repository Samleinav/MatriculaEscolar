using System;
using System.Collections.Generic;
using System.Text;

namespace ModelsBaseApp.License
{
    public class ValidateLicenseClass
    {
        public bool success { get; set; }
        public DataValidateLicenseClass Data { get; set; }
        public bool error { get; set; } = false;
        public class DataValidateLicenseClass
        {
            public int timesActivated { get; set; }
            public int timesActivatedMax { get; set; }
            public int remainingActivations { get; set; }
        }
    }
      
}
