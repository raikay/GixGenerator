using CodeGener.Common;
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
            iservice.Text = ConfigHelper.IServicePath;
            service.Text = ConfigHelper.ServicePath;
            irepository.Text = ConfigHelper.IRepositoryPath;
            repository.Text = ConfigHelper.RepositoryPath;
        }

        private void BtnSaveSetting_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("保存成功");
        }
    }
}
