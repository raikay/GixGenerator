using CodeGener.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GixGenerator
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        public string iservicePatht;
        public string servicePatht;
        public string irepositoryPatht;
        public string repositoryPatht;
        public MainWindow()
        {

            InitializeComponent();
            iservicePatht = ConfigHelper.IServicePath;
            servicePatht = ConfigHelper.ServicePath;
            irepositoryPatht = ConfigHelper.IRepositoryPath;
            repositoryPatht = ConfigHelper.RepositoryPath;
        }


        /// <summary>
        /// 根据表名 生成各层基础文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string className = tbox_class_name.Text;

            if (!Directory.Exists(irepositoryPatht))
            {
                Directory.CreateDirectory(irepositoryPatht);
            }
            EditContent("File/IRepository.txt", $"{irepositoryPatht}/I{className}Repository.cs");



            if (!Directory.Exists(repositoryPatht))
            {
                Directory.CreateDirectory(repositoryPatht);
            }
            EditContent("File/Repository.txt", $"{repositoryPatht}/{className}Repository.cs");


            if (!Directory.Exists(iservicePatht))
            {
                Directory.CreateDirectory(iservicePatht);
            }
            EditContent("File/IService.txt", $"{iservicePatht}/{className}IService.cs");

            if (!Directory.Exists(servicePatht))
            {
                Directory.CreateDirectory(servicePatht);
            }
            EditContent("File/Service.txt", $"{servicePatht}/{className}Service.cs");


            //var te = addr1.Text;
            MessageBox.Show($"生成成功");
        }



        /// <summary>
        /// 遍历文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDoFile_Click(object sender, RoutedEventArgs e)
        {
            //C#遍历指定文件夹中的所有文件 
            DirectoryInfo TheFolder = new DirectoryInfo(repositoryPatht);
            if (!TheFolder.Exists)
                return;

            //遍历文件
            foreach (FileInfo NextFile in TheFolder.GetFiles())
            {
                if (NextFile.Name.EndsWith("Repository.cs"))
                {
                    var content = GetContent(repositoryPatht + "/" + NextFile.Name);
                    //匹配一个类中所有函数
                    MatchCollection matches=Regex.Matches(content, "public\\s{1,50}(?!class).{1,300}\\s{1,50}(\\w{1,50})\\(");
                    //MatchCollection matches = Regex.Matches(content, "(?s)(?<all>(\\s{0,200}}\\s\n|}\n)\\s{0,100}(}\\s{1,200}|}))$");
                    //var ss = matches["all"].ToString();
                    var temp = (matches[0] as Match).Groups["all"].ToString();
                    //content = content.Replace(temp, "");
                    foreach (Match item in matches)
                    {
                        var ss = item.Groups["all"].ToString();
                        MessageBox.Show(item.ToString());
                    }



                }
                //  continue;
                // 获取文件完整路径
                //string heatmappath = NextFile.FullName;

            }
        }



        /// <summary>
        /// 根据接口生成 各层函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnGetApiName_Click(object sender, RoutedEventArgs e)
        {
            //C#遍历指定文件夹中的所有文件 
            DirectoryInfo TheFolder = new DirectoryInfo(iservicePatht);
            if (!TheFolder.Exists)
                return;

            //遍历文件
            foreach (FileInfo NextFile in TheFolder.GetFiles())
            {
                if (NextFile.Name.EndsWith("Service.cs") && !NextFile.Name.Contains("BaseService.cs"))
                {


                    #region 匹配IService所有接口
                    var icontent = GetContent(iservicePatht + "/" + NextFile.Name);
                    MatchCollection matches = Regex.Matches(icontent, @"\s{0,20}(?<ResultName>[a-zA-Z]{1}[a-zA-Z0-9]{1,20}(<[a-zA-Z0-9\,\<\>\s]{1,30}>){0,1})\s{1,5}(?<FunName>[A-Z]{1}[a-zA-Z0-9]{1,20}(<[a-zA-Z0-9\,\<\>\s]{1,30}>){0,1})\((?<ParamStr>[a-zA-Z0-9\,\<\>\s\[\]]{0,200})\)\s{0,5}\;");

                    var iserverFunList = new List<(string resultName, string funName, string paramStr)>();
                    foreach (Match item in matches)
                    {
                        var resultName = item.Groups["ResultName"].ToString();
                        var funName = item.Groups["FunName"].ToString();
                        var paramStr = item.Groups["ParamStr"].ToString();

                        iserverFunList.Add((resultName, funName, paramStr));
                    }
                    //循环生成函数体
                    StringBuilder sb = new StringBuilder();
                    foreach (var item in iserverFunList)
                    {
                        //if (!ifunList.Any(c => c.funName == item.funName && c.resultName == item.resultName && c.paramStr == item.paramStr))
                        //{
                        //    sb.Append(GetFunStr(item.resultName, item.funName, item.paramStr));
                        //}

                    }
                    var resultstr = sb.ToString();

                    #endregion

                    //生成Service
                    //获取service
                    var serviceFileName = $"{servicePatht}\\{NextFile.Name.Substring(1, (int)NextFile.Name.Length - 1)}";

                    var serviceFileContent = GetContent(serviceFileName);
                    MatchCollection serviceMatches = Regex.Matches(serviceFileContent, @"\s{0,20}[a-z]{3,10}\s{1,10}(async\s{1,10}){0,1}(?<ResultName>[a-zA-Z]{1}[a-zA-Z0-9]{1,20}(<[a-zA-Z0-9\,\<\>\s]{1,30}>){0,1})\s{1,5}(?<FunName>[A-Z]{1}[a-zA-Z0-9]{1,20}(<[a-zA-Z0-9\,\<\>\s]{1,30}>){0,1})\((?<ParamStr>[a-zA-Z0-9\,\<\>\s\[\]]{0,200})\)\s{0,5}\n\s{0,20}\{");

                    var serviceFunList = new List<(string resultName, string funName, string paramStr)>();
                    foreach (Match item in serviceMatches)
                    {


                        var resultName = item.Groups["ResultName"].ToString();
                        var funName = item.Groups["FunName"].ToString();
                        var paramStr = item.Groups["ParamStr"].ToString();


                        //var ss = sb.ToString();
                        serviceFunList.Add((resultName, funName, paramStr));
                    }

                    foreach (var item in iserverFunList)
                    {
                        if (!serviceFunList.Any(c => c.funName == item.funName && c.resultName == item.resultName && c.paramStr == item.paramStr))
                        {
                            sb.Append(GetFunStr(item.resultName, item.funName, item.paramStr));
                        }
                    }

                    StringBuilder serviceSb = new StringBuilder();
                    //var tempStr = Regex.Match(serviceFileContent,);
                    if (sb.Length > 0)
                    {
                        serviceSb.Append(Regex.Replace(serviceFileContent, @"\s{0,20}\}\s{0,20}\n{0,5}\s{0,20}\}\s{0,20}$", ""));
                        serviceSb.Append(sb.ToString());
                        serviceSb.Append("\r\n");
                        serviceSb.Append(Regex.Match(serviceFileContent, @"\s{0,20}\}\s{0,20}\n{0,5}\s{0,20}\}\s{0,20}$").ToString());
                        var serviceResultStr = serviceSb.ToString();

                        System.IO.TextWriter textWriter = new System.IO.StreamWriter(serviceFileName, false);
                        textWriter.Write(serviceResultStr);
                        textWriter.Close();
                        MessageBox.Show("生成成功");
                    }


                    //生成Repo
                    //生成IRepo
                }

            }
        }



        /// <summary>
        /// 显示设置窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSetting_Click(object sender, RoutedEventArgs e)
        {
            new Setting().Show();
        }




        #region 根据模板生成四个文件
        private void EditContent(string filePath, string writePath)
        {

            System.IO.StreamReader reader = new System.IO.StreamReader(filePath);
            string contents = reader.ReadToEnd();
            reader.Close();
            Console.Write(contents);

            contents = replaceTemp(contents);

            System.IO.TextWriter textWriter = new System.IO.StreamWriter(writePath, true);
            textWriter.Write(contents);
            textWriter.Close();
        }

        private string replaceTemp(string contents)
        {
            string dbcontext = "SqliteContext";
            string projectName = "Raikay.Managix";
            string className = "User";

            string projectNameTemp = "#{ProjectName}";
            string classNameTemp = "#{Name}";
            string dbcontextTemp = "#{DBContext}";

            contents = contents.Replace(dbcontextTemp, dbcontext);
            contents = contents.Replace(projectNameTemp, projectName);
            contents = contents.Replace(classNameTemp, className);

            return contents;

        }
        #endregion

        #region 生成完整函数体
        /// <summary>
        /// 根据接口匹配值生成完成函数体
        /// </summary>
        /// <param name="resultName"></param>
        /// <param name="funName"></param>
        /// <param name="paramStr"></param>
        /// <returns></returns>
        public string GetFunStr(string resultName, string funName, string paramStr)
        {
            StringBuilder sb = new StringBuilder();
            string firstSpace = "        ";
            string codeFirstSpace = "            ";
            //函数头
            sb.Append($"\r\n{firstSpace}public {resultName} {funName}({paramStr})\r\n");
            sb.Append(firstSpace);
            sb.Append("{\r\n");
            //函数体

            if (resultName != "Task" && resultName != "void")
            {
                //sb.Append(codeFirstSpace);
                //sb.Append($"{resultName} result = new {resultName}();\r\n");
                sb.Append(codeFirstSpace);
                sb.Append($"return new {resultName}();\r\n");
            }

            //
            sb.Append($"{firstSpace}");
            sb.Append("}");
            sb.Append("\r\n");
            return sb.ToString();
        }
        #endregion

        #region 根据路径获取文本内容
        /// <summary>
        /// 根据路径获取文本内容
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>

        private string GetContent(string filePath)
        {
            System.IO.StreamReader reader = new System.IO.StreamReader(filePath);
            string contents = reader.ReadToEnd();
            reader.Close();
            return contents;
        }


        #endregion

        private void Btn_TeConfig_Click(object sender, RoutedEventArgs e)
        {
            ConfigHelper.SetValue("IRepositoryPath", @"D:\Resource\www\Managix\Raikay.Managix.IRepository");
        }
    }
}
