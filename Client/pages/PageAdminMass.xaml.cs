using Client.classDTO;
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
    /// Логика взаимодействия для PageAdminMass.xaml
    /// </summary>
    public partial class PageAdminMass : Page
    {
        private MessageDTO[] mess; 
        
        public PageAdminMass()
        {
            InitializeComponent();
            Loaded += PageAdminMass_Loaded;
        }

        private void PageAdminMass_Loaded(object sender, RoutedEventArgs e)
        {
            Load_mess();
        }

        public async void Load_mess()
        {
            EmployeeDto employee = new EmployeeDto();
            ConnectHost conn = new ConnectHost();
            JToken jObject = await conn.GetMesssages();
            var jsonSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            if (jObject != null)
            { 
                mess = JsonConvert.DeserializeObject<MessageDTO[]>(jObject.ToString());
                int i = 0;
                foreach (var tmp in mess)
                {
                    employee = GlobalRes.GetEmployee(tmp.IdEmployee);
                    mess[i].FIO = employee.LastName + " " + employee.FirstName + " " + employee.SecondName;
                    if (tmp.StatusRead == false)
                    {
                        mess[i].TextStatus = "Новое";
                    }
                    else
                    {
                        mess[i].TextStatus = "Прочитано";
                    }
                    i++;
                }
            }
            DG_Mess.ItemsSource = mess;
        }

        private async void Button_Click_Del(object sender, RoutedEventArgs e)
        {


            string tmp = "{\"Id\": \"" + mess[DG_Mess.SelectedIndex].Id.ToString() + "\"}";

            // Configure the message box to be displayed
            string messageBoxText = "Вы точно хотите удалить сообщение?";
            string caption = "";
            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Warning;

            // Display message box
            MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);

            // Process message box results
            switch (result)
            {
                case MessageBoxResult.Yes:
                    ConnectHost conn = new ConnectHost();
                    JToken jObject = await conn.DeleteMesssage(tmp);
                    DG_Mess.ItemsSource = null;
                    Load_mess();
                    break;
                case MessageBoxResult.No:

                    break;
            }


        }

        private void Button_Click_Edit(object sender, RoutedEventArgs e)
        {


            TBMessage.Text = mess[DG_Mess.SelectedIndex].MessageText;
            BRquestion.Visibility = Visibility.Visible;
        }

        private void CloseQuestion(object sender, RoutedEventArgs e)
        {
            BRquestion.Visibility = Visibility.Collapsed;
        }

        private async void Button_Click_Read(object sender, RoutedEventArgs e)
        {
            string tmp = "{\"Id\": \"" + mess[DG_Mess.SelectedIndex].Id.ToString() + "\"}";
            ConnectHost conn = new ConnectHost();
            JToken jObject = await conn.ChangeMesssageStatus(tmp);
            DG_Mess.ItemsSource = null;
            Load_mess();
            BRquestion.Visibility = Visibility.Collapsed;
        }
    }
}
