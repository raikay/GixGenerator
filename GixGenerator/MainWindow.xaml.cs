using CodeGener.Common;
using CodeGener.Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Forms;
using MessageBox = System.Windows.Forms.MessageBox;

namespace GixGenerator
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        public string iservicePath;
        public string servicePath;
        public string irepositoryPath;
        public string repositoryPath;
        public MainWindow()
        {
            //禁止缩放
            this.ResizeMode = System.Windows.ResizeMode.NoResize;
            //桌面居中
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.Topmost = true;
            InitializeComponent();
            iservicePath = ConfigHelper.IServicePath;
            servicePath = ConfigHelper.ServicePath;
            irepositoryPath = ConfigHelper.IRepositoryPath;
            repositoryPath = ConfigHelper.RepositoryPath;
        }


        #region 根据表名 生成各层基础文件

        /// <summary>
        /// 根据表名 生成各层基础文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string className = tbox_class_name.Text;

            EditContent("File/IRepository.txt", $"{irepositoryPath}/I{className}Repository.cs", irepositoryPath);

            EditContent("File/Repository.txt", $"{repositoryPath}/{className}Repository.cs", repositoryPath);

            EditContent("File/IService.txt", $"{iservicePath}/I{className}Service.cs", iservicePath);

            EditContent("File/Service.txt", $"{servicePath}/{className}Service.cs", servicePath);

            MessageBox.Show($"生成成功");
        }


        private void EditContent(string fileName, string writeNamePath, string filePath)
        {
            try
            {
                if (!Directory.Exists(filePath))
                    Directory.CreateDirectory(filePath);
                System.IO.StreamReader reader = new System.IO.StreamReader(fileName);
                string contents = reader.ReadToEnd();
                reader.Close();
                string projectName = "Raikay.Managix";
                string className = tbox_class_name.Text;
                string dbcontext = "SqliteContext";
                string projectNameTemp = "#{ProjectName}";
                string classNameTemp = "#{Name}";
                string dbcontextTemp = "#{DBContext}";
                contents = contents.Replace(dbcontextTemp, dbcontext);
                contents = contents.Replace(projectNameTemp, projectName);
                contents = contents.Replace(classNameTemp, className);
                //contents = replaceTemp(contents);
                System.IO.TextWriter textWriter = new System.IO.StreamWriter(writeNamePath, true);
                textWriter.Write(contents);
                textWriter.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }

        #endregion


        #region 根据接口生成 各层函数
        /// <summary>
        /// 根据接口生成 各层函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnGetApiName_Click(object sender, RoutedEventArgs e)
        {
            //C#遍历指定文件夹中的所有文件 
            DirectoryInfo TheFolder = new DirectoryInfo(iservicePath);
            if (!TheFolder.Exists)
                return;

            //遍历文件
            foreach (FileInfo NextFile in TheFolder.GetFiles())
            {
                if (NextFile.Name.EndsWith("Service.cs") && !NextFile.Name.Contains($"BaseService.cs"))
                {
                    #region 匹配IService所有接口
                    var icontent = GetContent(iservicePath + "/" + NextFile.Name);
                    MatchCollection matches = Regex.Matches(icontent, @"\s{0,20}(?<ResultName>[a-zA-Z]{1}[a-zA-Z0-9]{1,20}(<[a-zA-Z0-9\,\<\>\s]{1,30}>){0,1})\s{1,5}(?<FunName>[A-Z]{1}[a-zA-Z0-9]{1,20}(<[a-zA-Z0-9\,\<\>\s]{1,30}>){0,1})\((?<ParamStr>[a-zA-Z0-9\,\<\>\s\[\]]{0,200})\)\s{0,5}\;");
                    var iserverFunList = new List<(string resultName, string funName, string paramStr)>();
                    foreach (Match item in matches)
                    {
                        var resultName = item.Groups["ResultName"].ToString();
                        var funName = item.Groups["FunName"].ToString();
                        var paramStr = item.Groups["ParamStr"].ToString();
                        iserverFunList.Add((resultName, funName, paramStr));
                    }

                    #endregion
                    //匹配File所有函数
                    var serviceFileFullName = $"{servicePath}\\{NextFile.Name.Substring(1, (int)NextFile.Name.Length - 1)}";
                    CreateFile(serviceFileFullName, iserverFunList, GetServiceFunStr);

                    var irepoFileName = $"I{NextFile.Name.Substring(1, (int)NextFile.Name.Length - 1).Replace("Service.cs", "Repository.cs")}";
                    var irepositoryFileFullName = $"{irepositoryPath}\\{irepoFileName}";
                    CreateFile(irepositoryFileFullName, iserverFunList, GetIRepositoryFunStr);

                    var repoFileName = $"{NextFile.Name.Substring(1, (int)NextFile.Name.Length - 1).Replace("Service.cs", "Repository.cs")}";
                    var repositoryFileFullName = $"{repositoryPath}\\{repoFileName}";
                    CreateFile(repositoryFileFullName, iserverFunList, GetRepositoryFunStr);
                }
            }
        }


        public void CreateFile(string fileName, List<(string resultName, string funName, string paramStr)> iserverFunList, Func<string, string, string, string> getFunStr)
        {

            //var fileName = repositoryFileFullName;
            var fileContent = GetContent(fileName);
            MatchCollection serviceMatches = Regex.Matches(fileContent, @"\s{0,20}[a-z]{3,10}\s{1,10}(async\s{1,10}){0,1}(?<ResultName>[a-zA-Z]{1}[a-zA-Z0-9]{1,20}(<[a-zA-Z0-9\,\<\>\s]{1,30}>){0,1})\s{1,5}(?<FunName>[A-Z]{1}[a-zA-Z0-9]{1,20}(<[a-zA-Z0-9\,\<\>\s]{1,30}>){0,1})\((?<ParamStr>[a-zA-Z0-9\,\<\>\s\[\]]{0,200})\)\s{0,5}\n\s{0,20}\{");

            var fileFunList = new List<(string resultName, string funName, string paramStr)>();
            foreach (Match item in serviceMatches)
            {
                var resultName = item.Groups["ResultName"].ToString();
                var funName = item.Groups["FunName"].ToString();
                var paramStr = item.Groups["ParamStr"].ToString();
                fileFunList.Add((resultName, funName, paramStr));
            }

            //获取可生成函数体
            StringBuilder sb = new StringBuilder();
            foreach (var item in iserverFunList)
            {
                if (!fileFunList.Any(c => c.funName == item.funName && c.resultName == item.resultName && c.paramStr == item.paramStr))
                {
                    sb.Append(getFunStr(item.resultName, item.funName, item.paramStr));
                }
            }
            //生成文件
            StringBuilder serviceSb = new StringBuilder();
            if (sb.Length > 0)
            {
                serviceSb.Append(Regex.Replace(fileContent, @"\s{0,20}\}\s{0,20}\n{0,5}\s{0,20}\}\s{0,20}$", ""));
                serviceSb.Append(sb.ToString());
                serviceSb.Append("\r\n");
                serviceSb.Append(Regex.Match(fileContent, @"\s{0,20}\}\s{0,20}\n{0,5}\s{0,20}\}\s{0,20}$").ToString());
                var serviceResultStr = serviceSb.ToString();

                System.IO.TextWriter textWriter = new System.IO.StreamWriter(fileName, false);
                textWriter.Write(serviceResultStr);
                textWriter.Close();
                MessageBox.Show("生成成功");
            }
        }


        #region 生成完整函数体
        /// <summary>
        /// 根据接口匹配值生成完成函数体
        /// </summary>
        /// <param name="resultName"></param>
        /// <param name="funName"></param>
        /// <param name="paramStr"></param>
        /// <returns></returns>
        public string GetServiceFunStr(string resultName, string funName, string paramStr)
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
                sb.Append(codeFirstSpace);
                sb.Append($"return new {resultName}();\r\n");
            }
            sb.Append($"{firstSpace}");
            sb.Append("}");
            sb.Append("\r\n");
            return sb.ToString();
        }
        /// <summary>
        /// 根据接口匹配值生成完成函数体
        /// </summary>
        /// <param name="resultName"></param>
        /// <param name="funName"></param>
        /// <param name="paramStr"></param>
        /// <returns></returns>
        public string GetIRepositoryFunStr(string resultName, string funName, string paramStr)
        {

            StringBuilder sb = new StringBuilder();
            string firstSpace = "        ";
            //函数头
            sb.Append("\r\n");
            sb.Append($"{firstSpace}{resultName} {funName}({paramStr});\r\n");
            return sb.ToString();
        }
        /// <summary>
        /// 根据接口匹配值生成完成函数体
        /// </summary>
        /// <param name="resultName"></param>
        /// <param name="funName"></param>
        /// <param name="paramStr"></param>
        /// <returns></returns>
        public string GetRepositoryFunStr(string resultName, string funName, string paramStr)
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

        #endregion
        /// <summary>
        /// 显示设置窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSetting_Click(object sender, RoutedEventArgs e)
        {
            var Win_Setting = new Setting();
            Win_Setting.ShowDialog();
        }

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


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            CreateModelService.CreateModel();
            MessageBox.Show("生成成功");
        }

        private void linkDmsite_Click(object sender, RoutedEventArgs e)
        {
            Hyperlink link = sender as Hyperlink;
            Process.Start(new ProcessStartInfo(link.NavigateUri.AbsoluteUri));
        }
    }
}
