using System;
using System.Collections.Generic;
using System.Management;
using System.Text;

namespace ToolsBaseApp
{
    public static class OSInfo
    {
        public static string getSerialHddOS()
        {
            ManagementObjectSearcher objSearcher = new
            ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");

            objSearcher = new
               ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMedia");

            foreach (ManagementObject wmi_HD in objSearcher.Get())
            {
                return wmi_HD["SerialNumber"].ToString();
            }
            return null;
        }

        public static bool CheckSerial(string serial)
        {
            return getSerialHddOS().Equals(serial);
        }
    }
}
