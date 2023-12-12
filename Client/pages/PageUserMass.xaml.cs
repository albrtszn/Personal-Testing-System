using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Client.pages
{
    /// <summary>
    /// Логика взаимодействия для PageUserMass.xaml
    /// </summary>
    public partial class PageUserMass : Page
    {
        public PageUserMass()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            BT_sendMass.IsEnabled = false;
            if (addTextMass.Text.Length > 0)
            {
                string tmp = "{ \"MessageText\":\"" + addTextMass.Text + "\"}";

                ConnectHost conn = new ConnectHost();
                JToken jObject = await conn.AddMessage(tmp);
                if (jObject != null)
                {
                    MessageBox.Show("Сообщение отправлено");
                    addTextMass.Clear();
                }
                else
                {
                    MessageBox.Show("Сообщение не удалось отправить");
                }
              
            }
            BT_sendMass.IsEnabled = true;
        }
    }
}
