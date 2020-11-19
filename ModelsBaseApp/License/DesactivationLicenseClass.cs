using System;
using System.Collections.Generic;
using System.Text;

namespace ModelsBaseApp.License
{
    public class DesactivationLicenseClass
    {
        public bool success { get; set; }
        public DataDesactivationLicenseClass data { get; set; }
        public bool error { get; set; } = false;
    }
    public class DataDesactivationLicenseClass
    {
        public string id { get; set; }
        public object orderId { get; set; }
        public object productId { get; set; }
        public string licenseKey { get; set; }
        public object expiresAt { get; set; }
        public object validFor { get; set; }
        public string source { get; set; }
        public string status { get; set; }
        public string timesActivated { get; set; }
        public string timesActivatedMax { get; set; }
        public string createdAt { get; set; }
        public string createdBy { get; set; }
        public string updatedAt { get; set; }
        public string updatedBy { get; set; }
    }
}
