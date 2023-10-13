using Client.classDTO;
using Client.pages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Client.forms
{
    /// <summary>
    /// Логика взаимодействия для addSubdivision.xaml
    /// </summary>
    public partial class addSubdivision : Window
    {
        public addSubdivision()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            JSubdivisin jName = new JSubdivisin();
            jName.Name = addSubName.Text.Trim();
            string jout = JsonConvert.SerializeObject(jName);
            sendRequest(jout);
        }
        private async void sendRequest(string jin)
        {
            ConnectHost conn = new ConnectHost();
            JToken jObject = await conn.AddSubdivision(jin);
            if (jObject == null)
            {
                MessageBox.Show("Не удалось добавить подразделение");
            }
            else
            {
                MessageBox.Show("Подразделение успешно добавлено");
                this.Close();
               
               

            }

        }

        public class JSubdivisin
        {
            public string Name { get; set; }
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {   
            this.Close();   
        }
    }
}
