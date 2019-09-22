using CodeGener.Common;
using CodeGener.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GixGenerator
{
    /// <summary>
    /// Setting.xaml 的交互逻辑
    /// </summary>
    public partial class Setting : Window
    {
        //public string iservicePatht;
        //public string servicePatht;
        //public string irepositoryPatht;
        //public string repositoryPatht;
        public Setting()
        {
            this.ResizeMode = System.Windows.ResizeMode.NoResize;
            InitializeComponent();
            //Tbox_IServicePath.Text = ConfigHelper.IServicePath;
            //Tbox_ServicePath.Text = ConfigHelper.ServicePath;
            //Tbox_IRepositoryPath.Text = ConfigHelper.IRepositoryPath;
            //Tbox_RepositoryPath.Text = ConfigHelper.RepositoryPath;
            Tbox_DbConnectStr.Text = ConfigHelper.DBStr;
            Tbox_ModelNameSpace.Text = ConfigHelper.NameSpace;
            Tbox_ModelFullPath.Text = ConfigHelper.ModelFullPath;
            Cbox_DbType.Text = ConfigHelper.DBType;
            Tbox_ProjectPath.Text = ConfigHelper.ProjectPath;
            Tbox_ProjectName.Text = ConfigHelper.ProjectName;
        }

        private void BtnSaveSetting_Click(object sender, RoutedEventArgs e)
        {
            //ConfigHelper.IServicePath = Tbox_IServicePath.Text;
            //ConfigHelper.ServicePath = Tbox_ServicePath.Text;
            //ConfigHelper.IRepositoryPath = Tbox_IRepositoryPath.Text;
            //ConfigHelper.RepositoryPath = Tbox_RepositoryPath.Text;
            ConfigHelper.DBStr = Tbox_DbConnectStr.Text;
            ConfigHelper.NameSpace = Tbox_ModelNameSpace.Text;
            ConfigHelper.ModelFullPath = Tbox_ModelFullPath.Text;
            ConfigHelper.DBType = Cbox_DbType.Text;
            ConfigHelper.ProjectPath = Tbox_ProjectPath.Text;
            ConfigHelper.ProjectName = Tbox_ProjectName.Text;
            MessageBox.Show("保存成功");
            //this.Close();
        }

        private void BtnTestDb_Click(object sender, RoutedEventArgs e)
        {
            var conn = CreateModelService.GetInstance(Tbox_DbConnectStr.Text);
            var ss = conn.Context;
            try
            {
                conn.Open();
                MessageBox.Show("连接成功");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"连接失败:{ex.Message}");
            }

        }

        //private void Btn_IRepo_Click(object sender, RoutedEventArgs e)
        //{
        //    System.Windows.Forms.FolderBrowserDialog openFileDialog = new System.Windows.Forms.FolderBrowserDialog();  //选择文件夹

        //    openFileDialog.Description = "选择文件夹";
        //    openFileDialog.SelectedPath = Tbox_IRepositoryPath.Text;

        //    if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)//注意，此处一定要手动引入System.Window.Forms空间，否则你如果使用默认的DialogResult会发现没有OK属性
        //    {
        //        Tbox_IRepositoryPath.Text = openFileDialog.SelectedPath;
        //        //MessageBox.Show(openFileDialog.SelectedPath);
        //    }
        //}

        private void Btn_Repo_Click(object sender, RoutedEventArgs e)
        {
            //SelectFilePath(Tbox_RepositoryPath);
        }

        private void Btn_ISer_Click(object sender, RoutedEventArgs e)
        {

            ////SelectFilePath(Tbox_IServicePath);
        }

        private void Btn_Ser_Click(object sender, RoutedEventArgs e)
        {
            //SelectFilePath(Tbox_ServicePath);
        }

        private void SelectFilePath(TextBox sender)
        {
            System.Windows.Forms.FolderBrowserDialog openFileDialog = new System.Windows.Forms.FolderBrowserDialog();  //选择文件夹

            openFileDialog.Description = "选择文件夹";
            openFileDialog.SelectedPath = sender.Text;

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                sender.Text = openFileDialog.SelectedPath;
            }
        }
    }
}
