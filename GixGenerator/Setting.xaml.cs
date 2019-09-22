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
            InitializeComponent();
            tbox_iservice.Text = ConfigHelper.IServicePath;
            tbox_service.Text = ConfigHelper.ServicePath;
            tbox_irepository.Text = ConfigHelper.IRepositoryPath;
            tbox_repository.Text = ConfigHelper.RepositoryPath;
            tbox_dbstr.Text = ConfigHelper.DBStr;
            tbox_modelnamespace.Text = ConfigHelper.NameSpace;
            tbox_modelfullpath.Text = ConfigHelper.ModelFullPath;
            cbox_dbtype.Text = ConfigHelper.DBType;
        }

        private void BtnSaveSetting_Click(object sender, RoutedEventArgs e)
        {
            ConfigHelper.IServicePath = tbox_iservice.Text;
            ConfigHelper.ServicePath = tbox_service.Text;
            ConfigHelper.IRepositoryPath = tbox_irepository.Text;
            ConfigHelper.RepositoryPath = tbox_repository.Text;
            ConfigHelper.DBStr = tbox_dbstr.Text;
            ConfigHelper.NameSpace = tbox_modelnamespace.Text;
            ConfigHelper.ModelFullPath = tbox_modelfullpath.Text;
            ConfigHelper.DBType = cbox_dbtype.Text;
            MessageBox.Show("保存成功");
            //this.Close();
        }

        private void BtnTestDb_Click(object sender, RoutedEventArgs e)
        {
            var conn = CreateModelService.GetInstance(tbox_dbstr.Text);
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

        private void Btn_Repo_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog openFileDialog = new System.Windows.Forms.FolderBrowserDialog();  //选择文件夹

            openFileDialog.Description = "选择文件夹";
            openFileDialog.SelectedPath = tbox_irepository.Text;

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)//注意，此处一定要手动引入System.Window.Forms空间，否则你如果使用默认的DialogResult会发现没有OK属性
            {
                tbox_irepository.Text = openFileDialog.SelectedPath;
                //MessageBox.Show(openFileDialog.SelectedPath);
            }
        }
    }
}
