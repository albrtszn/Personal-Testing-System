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

namespace Client
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class SettingConnHost : Window
    {
        public SettingConnHost()
        { 
            InitializeComponent();
            serverName.Text = ConnectHost.urlHost;
            if (ConnectHost.proсHost == "http")
            {
                CB_http.SelectedIndex = 0;
            }
            else
            {
                CB_http.SelectedIndex = 1;
            }
            

        }

        private void closeSetting(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Close(); // Скрываем нынешнее окно
                AutWindow autWindow = new AutWindow();
                autWindow.Show();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {


            ConnectHost.urlHost = serverName.Text;

            if (CB_http.SelectedIndex == 0)
            {
                Properties.Settings.Default.protocol = "http";
                ConnectHost.proсHost = "http";
            }
            else
            {
                Properties.Settings.Default.protocol = "https";
                ConnectHost.proсHost = "https";
            }
            Properties.Settings.Default.hostUrl = serverName.Text;
            Properties.Settings.Default.Save();


            ConnectHost conn = new ConnectHost();

            string output = await conn.Ping();
            Console.WriteLine(output);
            statusConn.Text = output;

        }
    }
}
