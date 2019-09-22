using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGener.Common
{
    public class ConfigHelper
    {

        public static string IServicePath
        {
            get { return GetValue("IServicePath"); }
            set { SetValue("IServicePath", value); }
        }

        public static string ServicePath
        {
            get { return GetValue("ServicePath"); }
            set { SetValue("ServicePath", value); }
        }

        public static string IRepositoryPath
        {
            get { return GetValue("IRepositoryPath"); }
            set { SetValue("IRepositoryPath", value); }
        }

        public static string RepositoryPath
        {
            get { return GetValue("RepositoryPath"); }
            set { SetValue("RepositoryPath", value); }
        }

        /// <summary>
        /// 设置 config
        /// </summary>
        /// <param name="AppKey"></param>
        /// <param name="AppValue"></param>
        public static void SetValue(string AppKey, string AppValue)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            if (config.AppSettings.Settings[AppKey] != null)
                config.AppSettings.Settings[AppKey].Value = AppValue;
            else
                config.AppSettings.Settings.Add(AppKey, AppValue);

            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");

        }

        /// <summary>
        /// 读取config
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetValue(string key)
        {
            return ConfigurationManager.AppSettings[key].ToString();
            //return ConfigurationSettings.AppSettings[key].ToString();//过时
        }
    }
}
