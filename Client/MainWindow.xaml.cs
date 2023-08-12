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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Client
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public JEmployee employee = new JEmployee();
        public string TokenEmployee = "";
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            string login = autLogin.Text.Trim();
            string pass = autPass.Password;
            var result = await ProcessRequest(login, pass);
        }

        private async Task <string> ProcessRequest(string login, string password)
        {
            var uri = "https://fitpsu.online/user-api/Login";
            var request = new HttpRequestMessage();
            string xjson = "";
            Jlogin jlogin = new Jlogin();
            jlogin.login = login;
            jlogin.password = password;

            string jout = JsonConvert.SerializeObject(jlogin);

            HttpContent c = new StringContent(jout, Encoding.UTF8, "application/json");

            request.RequestUri = new Uri(uri);
            request.Method = HttpMethod.Post;
            request.Content = c;
            var client = new HttpClient();
            // client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tok_ty, acc_tok);

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = await client.SendAsync(request);

            switch (response.StatusCode)
            {
                case (System.Net.HttpStatusCode.OK):
                    HttpContent content = response.Content;
                    xjson = await content.ReadAsStringAsync();
                    JToken jObject = JToken.Parse(xjson);
                    employee = JsonConvert.DeserializeObject<JEmployee>(jObject["Employee"].ToString());
                    TokenEmployee = jObject["TokenEmployee"].ToString();
                    break;

                default:
                    xjson = "";
                    break; 
            }

            
            Console.WriteLine(xjson);
            return xjson;

        }

        private void closeApp(object sender, RoutedEventArgs e)
        {
            try
            {
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void setting(object sender, RoutedEventArgs e)
        {
            this.Hide(); // Скрываем нынешнее окно

            // Создаем объект на основе определенного окан
            Window1 windowSetting = new Window1();
            // Показываем новое окно
            windowSetting.Show();
        }
    }

    public class Jlogin
    {
        public string login { get; set; }
        public string password { get; set; }
    }

    public class JEmployee
    {
        public string id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string LastName { get; set; }
        public string Login { get; set; }
        public string DateOfBirth { get; set; }
        public int IdSubdivision { get; set; }
    }


}
