using Client.classDTO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
using static Client.AutWindow;

namespace Client
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class SettingConnHost : Window
    {
        private MainWindow globalOwner;
        private MySetting[] inSetting;
        public MySetting outSetting;

        public class MySetting
        {
            public int Id { get; set; }
            public bool TestingTimeLimit { get; set; }
            public bool SkippingQuestion { get; set; }
            public bool EarlyCompletionTesting { get; set; }
            public bool AdditionalBool {  get; set; }
            public int AdditionalInt {get; set;}
            public string AdditionalString { get; set;}
        }

        public SettingConnHost(object myOwner)
        { 
            globalOwner = myOwner as MainWindow;
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
            Loaded += SettingConnHost_Loaded;
            
        }

        private async void SettingConnHost_Loaded(object sender, RoutedEventArgs e)
        {
            ConnectHost conn = new ConnectHost();
            JToken jObject = await conn.GetGlobalConfigures();
            var jsonSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            if (jObject == null)
            {
                return;
            }
            inSetting = JsonConvert.DeserializeObject<MySetting[]>(jObject.ToString(), jsonSettings);
            outSetting = inSetting[0];
            DataContext = outSetting;
        }

        private void closeSetting(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Close(); // Скрываем нынешнее окно
                globalOwner.Show();

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
            string output = "Ошибка соединения!";

            if (serverName.Text != "")
            {
                Properties.Settings.Default.hostUrl = serverName.Text;
               
                ConnectHost conn = new ConnectHost();
                output = await conn.Ping();
            }        
            
            if (output.Contains("Ошибка соединения!"))
            {
                statusConn.Foreground = Brushes.Red;
            }
            else
            {
                statusConn.Foreground = Brushes.Green;
               
            }
            
            statusConn.Text = output;

        }

        private async void Button_Save(object sender, RoutedEventArgs e)
        {
            BTSaveConfig.IsEnabled = false;
            ConnectHost conn = new ConnectHost();
            string output = "Ошибка соединения!";
            output = await conn.Ping();
            if (output.Contains("Ошибка соединения!"))
            {
                statusConn.Foreground = Brushes.Red;
            }
            else
            {
                statusConn.Foreground = Brushes.Green;

            }
            statusConn.Text = output;
           
            if (!(statusConn.Text.Contains("Соединение не установлено") || statusConn.Text.Contains("Ошибка соединения!")))
            {
                Properties.Settings.Default.Save();
                string tmpPay = string.Empty;
                tmpPay = JsonConvert.SerializeObject(outSetting);
                JToken jObject = await conn.UpdateGlobalConfigure(tmpPay);
                if (jObject == null)
                {
                    MessageBox.Show("Не удалось обновить конфигурацию");
                }
                else
                {
                    MessageBox.Show("Конфигурация обновлена успешно");
                }
            }

            BTSaveConfig.IsEnabled = true;
        }
    }
}
