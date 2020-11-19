using System;
using System.Collections.Generic;
using System.Text;

namespace ModelsBaseApp.License
{
    public class LicenseClass
    {

        public LicenseClass()
        {

        }
        public LicenseClass(object license)
        {
            if (license != null)
            {
                foreach (var property in this.GetType().GetProperties())
                {
                    if (license.GetType().GetProperty(property.Name).GetValue(license) == null) continue;

                    property.SetValue(this, license.GetType().GetProperty(property.Name).GetValue(license));
                }
            }

        }
        public string id { get; set; }
        public bool isActive { get; set; } = false;
        public bool isValid { get; set; } = false;
        public bool isInactive { get; set; } = false;
        public object orderId { get; set; }
        public object productId { get; set; } = "123456";
        public string licenseKey { get; set; }
        public string expiresAt { get; set; } = null;
        public object validFor { get; set; }
        public string source { get; set; }
        public string status { get; set; }
        public string timesActivated { get; set; }
        public string timesActivatedMax { get; set; }
        public string createdAt { get; set; }
        public string createdBy { get; set; }
        public string updatedAt { get; set; }
        public string updatedBy { get; set; }
        public string Type { get { return "License"; } set { } }
        public void LoadValues(object values)
        {
            foreach (var property in this.GetType().GetProperties())
            {
                property.SetValue(this, values.GetType().GetProperty(property.Name).GetValue(values, null));
            }
        }
    }
}
