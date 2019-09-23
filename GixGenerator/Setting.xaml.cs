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
        public Setting()
        {
            this.ResizeMode = System.Windows.ResizeMode.NoResize;
            InitializeComponent();
            Tbox_DbConnectStr.Text = ConfigHelper.DBConnectStr;
            Tbox_ModelNameSpace.Text = ConfigHelper.NameSpace;
            Tbox_ModelFullPath.Text = ConfigHelper.ModelFullPath;
            Cbox_DbType.Text = ConfigHelper.DBType;
            Tbox_ProjectPath.Text = ConfigHelper.ProjectPath;
            Tbox_ProjectName.Text = ConfigHelper.ProjectName;
        }

        private void BtnSaveSetting_Click(object sender, RoutedEventArgs e)
        {
            ConfigHelper.DBConnectStr = Tbox_DbConnectStr.Text;
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

        private void Btn_ProSelecter_Click(object sender, RoutedEventArgs e)
        {
            SelectFilePath(Tbox_ProjectPath);
        }

        private void Btn_ModelSelecter_Click(object sender, RoutedEventArgs e)
        {
            SelectFilePath(Tbox_ModelFullPath);
        }
    }
}
