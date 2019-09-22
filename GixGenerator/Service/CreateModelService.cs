using CodeGener.Common;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGener.Service
{
   public class CreateModelService
    {

        public static void CreateModel()
        {

            var conn = ConfigHelper.DBStr;//config.GetSection("Connections:DefaultConnect").Value;
            string path = string.Empty;
            var relativePath = "";//config.GetSection("Settings:RelativePath").Value;
            //自动找最外层并 找到更外层 方便附加到其他项目中
            if (!string.IsNullOrEmpty(relativePath))
            {
                var basePath = new DirectoryInfo(Directory.GetCurrentDirectory());
                while ((basePath.FullName.Contains(@"\Debug") || basePath.FullName.Contains(@"\bin")) && !string.IsNullOrEmpty(basePath.FullName))
                {
                    basePath = basePath.Parent;
                }
                path = Path.Combine(basePath.Parent.FullName, relativePath);
            }
            var fullPath = ConfigHelper.ModelFullPath;//config.GetSection("Settings:FullPath").Value;
            if (!string.IsNullOrEmpty(fullPath))
                path = fullPath;
            InitModel(conn, ConfigHelper.NameSpace, path, ConfigHelper.GenerateTables);
        }

        public static void InitModel(string conn, string namespaceStr, string path, string genaratetables)
        {
            try
            {
                Console.WriteLine("开始创建");
                var tableNames = genaratetables.Split(',').ToList();
                for (int i = 0; i < tableNames.Count; i++)
                {
                    tableNames[i] = tableNames[i].ToLower();
                }
                var suger = GetInstance(conn).DbFirst.SettingClassTemplate(old =>
                {
                    return old.Replace("{Namespace}", namespaceStr);//.Replace("class {ClassName}", "class {ClassName} :BaseEntity");//改变命名空间
                });

                if (tableNames.Count >= 2)
                {
                    suger.Where(it => tableNames.Contains(it.ToLower())).IsCreateDefaultValue();
                }
                else
                {
                    suger.IsCreateDefaultValue();
                }

                //过滤BaseEntity中存在的字段
                //var pros = typeof(BaseEntity).GetProperties();
                //var list = new List<SqlSugar.IgnoreColumn>();
                var tables = suger.ToClassStringList().Keys;
                //foreach (var item in pros)
                //{
                //    foreach (var table in tables)
                //    {
                //        list.Add(new SqlSugar.IgnoreColumn() { EntityName = table, PropertyName = item.Name });
                //    }
                //}
                //suger.Context.IgnoreColumns.AddRange(list);
                suger.CreateClassFile(path);
                Console.WriteLine("创建完成");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        public static SqlSugarClient GetInstance(string conn)
        {
            SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = conn,
                DbType = DbType.Sqlite,
                IsAutoCloseConnection = true,
                IsShardSameThread = true //设为true相同线程是同一个SqlSugarClient
            });
            db.Ado.IsEnableLogEvent = false;
            //db.Ado.LogEventStarting = (sql, pars) =>{};
            return db;
        }
    }




}
