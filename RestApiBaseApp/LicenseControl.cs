using Newtonsoft.Json;
using System.Net;
using System;
using ModelsBaseApp.License;

namespace RestApiBaseApp
{
    public sealed class LicenseControl
    {
        private static readonly LicenseControl instanse = new LicenseControl();
        static LicenseControl() {
            
        }
        private LicenseControl() {
            license = new LicenseClass();
        }
        public static LicenseControl Instanse { get { return instanse; } }
        private string Host { get; set; } = "https//localhost";
        private string Pretender { get; set; } = "THE-PRETENDER";
        private static LicenseClass license { get; set; } 
        public LicenseClass LicenseInstanse { set { license = value; } }
        private bool IsActiveLicense { get { return license.isActive; } } 
        private bool IsValidateLicense { get { return license.isValid; } }
        private bool IsDesactivatedLicense { get { return license.isInactive; } }
        public bool IsActivate { get { return IsActiveLicense; } }
        public bool IsValid { get { return IsValidateLicense; } }
        public bool IsInactive { get { return IsDesactivatedLicense; } }

        public string ExpiresAt { get { return license.expiresAt; } }

        private T GetLicense<T>(string key, string Link) where T: class
        {
            using (var client = new WebClient())
            {
                try
                {
                    var responseString = client.DownloadString($"{Host}{Link}{Pretender}?consumer_key={key}");
                    var jsonToClass = JsonConvert.DeserializeObject<T>(responseString);
                    return jsonToClass;

                }
                catch
                {
                    var err = "{'success':false , 'error': true}";
                    return JsonConvert.DeserializeObject<T>(err);
                }
               
            }
        }
        public ActivationLicenseClass Active(string consumerKey = null)
        {
            if (consumerKey == null) consumerKey = license.licenseKey;

            var Link = "/wp-json/lmfwc/v2/licenses/activate/";
            return GetLicense<ActivationLicenseClass>(consumerKey, Link);
          
        }
        public DesactivationLicenseClass Desactive()
        {
            if (IsActiveLicense)
            {
                var Link = "/wp-json/lmfwc/v2/licenses/deactivate/";
                return GetLicense<DesactivationLicenseClass>(license.licenseKey, Link);
            }
            return null;
        }
        public ValidateLicenseClass Validate(string consumerKey = null)
        {
            if (consumerKey == null) consumerKey = license.licenseKey;

            var Link = "/wp-json/lmfwc/v2/licenses/validate/";
            return GetLicense<ValidateLicenseClass>(consumerKey, Link);
        }

        public void LoadValues(object values)
        {
            license.LoadValues(values);
        }
    }
}
  
