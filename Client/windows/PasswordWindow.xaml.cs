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

namespace Client.windows
{
    /// <summary>
    /// Логика взаимодействия для PasswordWindow.xaml
    /// </summary>
    public partial class PasswordWindow : Window
    {
        private string idUser = string.Empty;
        public PasswordWindow(string id)
        {
            idUser = id;
            InitializeComponent();
            this.idUser = idUser;
        }

        private async void Accept_Click(object sender, RoutedEventArgs e)
        {
            string pass = passBox.Password;

            if (pass == "1985")
            {
                string tmp_str = "{\"Id\"" + ":\"" + idUser + "\"}";
                ConnectHost conn = new ConnectHost();
                JToken jObject = await conn.DeleteEmployee(tmp_str);
                if (jObject != null)
                {
                    MessageBox.Show("Пользователь успешно удален");
                }
            }


            this.Close(); 
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
