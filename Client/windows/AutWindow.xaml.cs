using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
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

    public partial class AutWindow : Window
    {
        public static EmployeeDto employee = new EmployeeDto();
        public static AdminDto admin = new AdminDto();

        public AutWindow()
        {
            InitializeComponent();
            ConnectHost.urlHost = Properties.Settings.Default.hostUrl;
            ConnectHost.proсHost = Properties.Settings.Default.protocol;

        }

        private void CloseApp(object sender, RoutedEventArgs e)
        {
            try
            {
                Application.Current.Shutdown();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Setting(object sender, RoutedEventArgs e)
        {
            this.Hide(); // Скрываем нынешнее окно

            // Создаем объект на основе определенного окан
            SettingConnHost windowSetting = new SettingConnHost();
            // Показываем новое окно
            windowSetting.Show();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            BtnAut.IsEnabled = false;
            Jlogin jlogin = new Jlogin();
            jlogin.login = autLogin.Text.Trim();
            jlogin.password = autPass.Password;
            string jout = JsonConvert.SerializeObject(jlogin);

            ConnectHost.userRole = 2;
            ConnectHost conn = new ConnectHost();
            JToken jObject = await conn.Login(jout);

            if (jObject == null)
            {
                ConnectHost.userRole = 1;
                jObject = await conn.Login(jout);
            }

            if (jObject == null)
            {
                MessageBox.Show("Пользователь не найден");
                
            }
            else
            {
                var jsonSettings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                };

                if (ConnectHost.userRole == 1)
                {
                    employee = JsonConvert.DeserializeObject<EmployeeDto>(jObject["Employee"].ToString(), jsonSettings);
                    ConnectHost.token = jObject["TokenEmployee"].ToString();
                    this.Hide();
                    UserWindow userWindow = new UserWindow();
                    userWindow.Show();
                    this.Close();
                }
                else
                {
                    admin = JsonConvert.DeserializeObject<AdminDto>(jObject["Admin"].ToString(), jsonSettings);
                    ConnectHost.token = jObject["TokenAdmin"].ToString();
                    this.Hide();
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    this.Close();

                }


            }
            BtnAut.IsEnabled = true;
        }
              
        public class Jlogin
        {
            public string login { get; set; }
            public string password { get; set; }
        }
  
  }
}