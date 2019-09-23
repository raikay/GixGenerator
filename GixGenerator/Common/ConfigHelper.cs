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
        private static string projectFilePath=$@"{GetValue("ProjectPath")}\{GetValue("ProjectName")}";
        public static string IServicePath
        {
            get { return $@"{projectFilePath}IServices"; }
            //set { SetValue("IServicePath", value); }
        }

        public static string ServicePath
        {
            get { return $@"{projectFilePath}Services"; }
            //get { return GetValue("ServicePath"); }
            //set { SetValue("ServicePath", value); }
        }

        public static string IRepositoryPath
        {
            get { return $@"{projectFilePath}IRepository"; }
            //get { return GetValue("IRepositoryPath"); }
            //set { SetValue("IRepositoryPath", value); }
        }
        
        public static string RepositoryPath
        {
            get { return $@"{projectFilePath}Repository"; }
            //get { return GetValue("RepositoryPath"); }
            //set { SetValue("RepositoryPath", value); }
        }
        /// <summary>
        /// 数据库连接字符串
        /// Sqlite demo: Data Source=managix.db3;
        /// </summary>
        public static string DBConnectStr
        {
            get { return GetValue("DBConnectString"); }
            set { SetValue("DBConnectString", value); }
        }


        /// <summary>
        /// model 命名空间
        /// </summary>
        public static string NameSpace
        {
            get { return GetValue("NameSpace"); }
            set { SetValue("NameSpace", value); }
        }

        /// <summary>
        /// model路径
        /// </summary>
        public static string ModelFullPath
        {
            get { return GetValue("ModelFullPath"); }
            set { SetValue("ModelFullPath", value); }
        }

        /// <summary>
        /// 需要生成的表名 不填默认生成全部的表
        /// </summary>
        public static string GenerateTables
        {
            get { return GetValue("GenerateTables"); }
            set { SetValue("GenerateTables", value); }
        }

        /// <summary>
        /// 数据库类型
        /// </summary>
        public static string DBType
        {
            get { return GetValue("DBType"); }
            set { SetValue("DBType", value); }
        }

        /// <summary>
        /// 项目地址
        /// </summary>
        public static string ProjectPath
        {
            get { return GetValue("ProjectPath"); }
            set { SetValue("ProjectPath", value); }
        }
        /// <summary>
        /// 项目地址
        /// </summary>
        public static string ProjectName
        {
            get { return GetValue("ProjectName"); }
            set { SetValue("ProjectName", value); }
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
