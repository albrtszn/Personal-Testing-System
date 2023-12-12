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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Client.pages
{
    /// <summary>
    /// Логика взаимодействия для PageAdminEdit.xaml
    /// </summary>
    public partial class PageAdminEdit : Page
    {
        private AdminDto userAdmin = new AdminDto();

        public PageAdminEdit(AdminDto myAdmin)
        {
            InitializeComponent();
            userAdmin = myAdmin;
            DataContext = userAdmin;
        }

        private void Button_Click_update(object sender, RoutedEventArgs e)
        {
         
            BT_update.IsEnabled = false;

            if (addLastName.Text == null || addLastName.Text == "" || addLastName.Text.Length < 2)
            {
                addLastName.Background = Brushes.Pink;
                MessageBox.Show("Значение в поле фамилии заполнено неверно!");
                BT_update.IsEnabled = true;
                return;
            }
            else
            {
                addLastName.Background = Brushes.Transparent;
            }

            if (addFirstName.Text == null || addFirstName.Text == "" || addFirstName.Text.Length < 2)
            {
                addFirstName.Background = Brushes.Pink;
                MessageBox.Show("Значение в поле имени заполнено неверно!");
                BT_update.IsEnabled = true;
                return;
            }
            else
            {
                addFirstName.Background = Brushes.Transparent;
            }

            if (addSecondName.Text == null || addSecondName.Text == "" || addSecondName.Text.Length < 2)
            {
                addSecondName.Background = Brushes.Pink;
                MessageBox.Show("Значение в поле отчества заполнено неверно!");
                BT_update.IsEnabled = true;
                return;
            }
            else
            {
                addSecondName.Background = Brushes.Transparent;
            }

            if (addPassword.Text == null || addPassword.Text == "" || addPassword.Text.Length < 2)
            {
                addPassword.Background = Brushes.Pink;
                MessageBox.Show("Значение в поле пароль заполнено неверно!");
                BT_update.IsEnabled = true;
                return;
            }
            else
            {
                addPassword.Background = Brushes.Transparent;
            }


            if (addLogin.Text == null || addLogin.Text == "" || addLogin.Text.Length < 2)
            {
                addLogin.Background = Brushes.Pink;
                MessageBox.Show("Значение в поле логин заполнено неверно!");
                BT_update.IsEnabled = true;
                return;
            }
            else
            {

                foreach (var tmpUser in GlobalRes.itemsUserAdmin)
                {
                    if (addLogin.Text == tmpUser.Login)
                    {
                        if (userAdmin.Id != tmpUser.Id)
                        {
                            addLogin.Background = Brushes.Pink;
                            MessageBox.Show("Измените значение в поле логин!");
                            BT_update.IsEnabled = true;
                            return;
                        }
                    }


                }
            }

            addLogin.Background = Brushes.Transparent;
           
            string jout = JsonConvert.SerializeObject(userAdmin);
            BT_update.IsEnabled = true;
            sendRequest(jout);
        }

        private async void sendRequest(string jin)
        {
            ConnectHost conn = new ConnectHost();
            JToken jObject = await conn.UpdateAdmin(jin);
            if (jObject == null)
            {
                MessageBox.Show("Не удалось обновить данные администратора");
            }
            else
            {
                MessageBox.Show("Данные администратора успешно обнавлены");
            }

        }

        private void Button_Click_Back(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}
