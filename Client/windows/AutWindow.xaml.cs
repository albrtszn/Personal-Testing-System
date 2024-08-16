using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Windows;
using System.Windows.Input;
using Client.classDTO;
using Client.windows;
using System.Windows.Media.TextFormatting;
using OpenTK.Graphics.ES10;



namespace Client
{

    public partial class AutWindow : Window
    {
        public static EmployeeLogin employee = new EmployeeLogin();
        public static AdminDto admin = new AdminDto();

        public AutWindow()
        {
            InitializeComponent();

            TestConnect();



            VerL.Content = "Версия: " + System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString();

        }

        private async void TestConnect()
        {
            WindowURL windowURL = new WindowURL();

            if (Client.Properties.Settings.Default.hostUrl == "")
            {
                MessageBox.Show("Ошибка соединения с сервером. Требуется настройка!");
                if (windowURL.ShowDialog() == true)
                {
                    Client.Properties.Settings.Default.hostUrl = windowURL.TextUrl;
                    Client.Properties.Settings.Default.Save();
                }

            }

            ConnectHost conn = new ConnectHost();
            string output = "Ошибка соединения!";

            output = await conn.Ping(Client.Properties.Settings.Default.hostUrl);
           
            if (output.Contains("Ошибка соединения!"))
            {
                MessageBox.Show("Ошибка соединения с сервером. Требуется настройка!");
                if (windowURL.ShowDialog() == true)
                {
                    Client.Properties.Settings.Default.hostUrl = windowURL.TextUrl;
                    Client.Properties.Settings.Default.Save();

                    ConnectHost.proсHost = Client.Properties.Settings.Default.protocol;
                    ConnectHost.urlHost = Client.Properties.Settings.Default.hostUrl;
                }
            }
            else
            {
                ConnectHost.proсHost = Client.Properties.Settings.Default.protocol;
                ConnectHost.urlHost = Client.Properties.Settings.Default.hostUrl;
            }

        }

        private void CloseApp(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Windows.Application.Current.Shutdown();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
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
                System.Windows.MessageBox.Show("Пользователь не найден");
                
            }
            else
            {
                var jsonSettings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                };

                if (ConnectHost.userRole == 1)
                {
                    employee = JsonConvert.DeserializeObject<EmployeeLogin>(jObject["Employee"].ToString(), jsonSettings);
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

        private void autPass_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                Button_Click(null, null);
        }
    }
}