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
    }
}
