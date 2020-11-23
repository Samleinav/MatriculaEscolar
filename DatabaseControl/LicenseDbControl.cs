using Couchbase.Lite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;
using System.Globalization;
using ModelsBaseApp.License;
using RestApiBaseApp;
using ToolsBaseApp;
using ModelsBaseApp;

namespace DatabaseControl
{
    public  class LicenseDbControl
    {
        
        public bool IsActive { get { return LicenseInfo.isActive; } }
        public bool IsInactive { get { return LicenseInfo.isInactive; } }
        public bool IsValid { get { return LicenseInfo.isValid; } }
        public string ExpiresAt { get { return LicenseInfo.expiresAt; } }
        public LicenseClass LicenseData { get { return LicenseInfo; } }
        public string SetLicenseKey { get; set; } = null;
        private  Database LicenseDb { get; set; } = new Database("license", new DatabaseConfiguration() { Directory = AppDomain.CurrentDomain.BaseDirectory + "Config/License" });
        private  Document LicenseDoc { get { return LicenseDb.GetDocument("License"); } }
        private LicenseClass LicenseInfo { get { return (LicenseDoc != null? ConvertTo.ToClass<LicenseClass>(LicenseDoc.ToDictionary()): new LicenseClass()); } }
        private  LicenseControl License { get { return LicenseControl.Instanse; } }
        
        private  void SetLicense(LicenseClass licenseClass)
        {
            using (var mu = new MutableDocument("License"))
            {
                var dict = ConvertTo.ToDictionary(licenseClass);
                mu.SetData(dict);
                LicenseDb.Save(mu);
            }
                
        }
        private  void SaveActiveLicense(ActivationLicenseClass license)
        {
            var activeclass = new LicenseClass(license);
            activeclass.isActive = license.success;
            activeclass.isInactive = false;
            activeclass.isValid = true;
            activeclass.createdAt = DateTime.Now.ToString();
            activeclass.validFor = "365";
            activeclass.status = "active";
            activeclass.expiresAt = DateTime.Now.AddYears(1).ToString(); 
            SetLicense(activeclass);
        }
        private  void SaveDesactivateLicense(DesactivationLicenseClass license = null)
        {
            if (license == null) license = new DesactivationLicenseClass();
            license.success = false;

            var activeclass = new LicenseClass(license);
            activeclass.isActive = false;
            activeclass.isInactive = false;
            activeclass.isValid = false;

            SetLicense(activeclass);
        }
        private void DesactiveExpiresLicense()
        {
            var license = LicenseInfo;
            license.isActive = false;
            license.isValid = false;
            license.isInactive = true;

            SetLicense(license);
        }

        private void InvalidLicense()
        {
            var license = LicenseInfo;
            license.isActive = false;
            license.isValid = false;
            license.isInactive = true;

            SetLicense(license);
        }
        //public void ValidateLicense(ValidateLicenseClass license)
        //{
        //    var activeclass = new LicenseClass(license);
        //    activeclass.isActive = false;
        //    activeclass.isInactive = false;
        //    activeclass.isValid = license.success;

        //    SetLicense(activeclass);
        //}

        public MethodResponse<string> Activate()
        {
            var response = Validate();
            if (response.result )
            {
                if (response.Data == "0")
                {
                    var result = License.Active(SetLicenseKey);
                    if (result != null && result.success)
                    {
                        SaveActiveLicense(result);
                        response.MSG = ResMSG.License.LICENCIA_ACTIVADA;
                        response.Data = null;
                    }
                }
                else
                {
                    response.result = false;
                    response.type = "error";
                    response.Data = null;
                    response.MSG = ResMSG.License.LICENCIA_EN_USO;
                }
                
            }
          
            return response;
        }
        public MethodResponse<string> Validate()
        {
            var licenseResult = License.Validate(SetLicenseKey);
            if (licenseResult.success){
                return new MethodResponse<string>()
                {
                    result = true,
                    type = "success",
                    MSG = ResMSG.License.LICENCIA_VALIDA,
                    Data = licenseResult.Data.timesActivated.ToString()
                };
            }
            if(licenseResult.error)
            {
                return new MethodResponse<string>()
                {
                    result = false,
                    type = "error",
                    MSG = ResMSG.System.ERROR_SERVIDOR,
                    Data = "Error"

                };
            }
            else
            {
                InvalidLicense();
                return new MethodResponse<string>()
                {
                    result = false,
                    type = "warning",
                    MSG = ResMSG.License.LICENCIA_INVALIDA

                };
            }
            
        }
        public MethodResponse Desactivate()
        {
            if (IsActive)
            {
                if (Convert.ToDateTime(ExpiresAt) < DateTime.Now)
                {
                    DesactiveExpiresLicense();
                    return new MethodResponse()
                    {
                        result = true,
                        type = "error",
                        MSG = ResMSG.License.LICENCIA_VENCIDA

                    };
                }
               
                var result = License.Desactive();
                if (result.success)
                {
                    SaveDesactivateLicense(result);
                    return new MethodResponse()
                    {
                        result = false,
                        type = "success",
                        MSG = ResMSG.License.LICENCIA_DESACTIVADA

                    };
                }
                if (result.error)
                {
                    return new MethodResponse()
                    {
                        result = true,
                        type = "error",
                        MSG = ResMSG.System.ERROR_SERVIDOR

                    };
                } 
            }
            return new MethodResponse()
            {
                result = false,
                type = "error",
                MSG = ResMSG.License.LICENCIA_ERROR

            };
        }
        public MethodResponse DesactiveExpires()
        {
            DesactiveExpiresLicense();
            return new MethodResponse() {
                MSG = ResMSG.License.LICENCIA_VENCIDA,
                result = true,
                type = "error"
            };

        }
    }
}
