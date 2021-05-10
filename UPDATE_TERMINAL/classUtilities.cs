using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
namespace UPDATE_TERMINAL
{
    class classUtilities
    {
        public static int GetIntValueFromConfig(string keyName)
        {
            string keyValue = System.Configuration.ConfigurationManager.AppSettings[keyName];
            int value = int.MaxValue;
            try
            {
                value = int.Parse(keyValue);
            }
            catch (Exception ex)
            {
                classDataAccessLogWriter.WriteLog("-------------Error: GetValueFromConfig() with " + keyName);
                classDataAccessLogWriter.WriteLog(ex.Message);
            }
            return value;
        }
        public static string GetStringValueFromConfig(string keyName)
        {
            string keyValue = "";
            try
            {
                keyValue = System.Configuration.ConfigurationManager.AppSettings[keyName];
                return keyValue;
            }
            catch (Exception ex)
            {
                classDataAccessLogWriter.WriteLog("-------------Error: GetValueFromConfig() with " + keyName);
                classDataAccessLogWriter.WriteLog(ex.Message);
            }
            return keyValue;
        }
    }
}
