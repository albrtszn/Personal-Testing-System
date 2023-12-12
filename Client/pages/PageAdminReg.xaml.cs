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
    /// Логика взаимодействия для PageAdminReg.xaml
    /// </summary>
    public partial class PageAdminReg : Page
    {
        public PageAdminReg()
        {
            InitializeComponent();
            addPassword.Text = CreatePassword(6);
        }

        private void Button_Click_ADD(object sender, RoutedEventArgs e)
        {
            AdminReg user = new AdminReg();
            if (addLastName.Text == null || addLastName.Text == "" || addLastName.Text.Length < 2)
            {
                addLastName.Background = Brushes.Pink;
                MessageBox.Show("Значение в поле фамилии заполнено неверно!");
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
                return;
            }
            else
            {

                foreach (var tmpUser in GlobalRes.itemsUserAdmin)
                {
                    if (addLogin.Text == tmpUser.Login)
                    {
                        addLogin.Background = Brushes.Pink;
                        MessageBox.Show("Измените значение в поле логин!");
                        return;
                    }


                }
            }

            addLogin.Background = Brushes.Transparent;



            user.LastName = addLastName.Text.Trim();
            user.FirstName = addFirstName.Text.Trim();
            user.SecondName = addSecondName.Text.Trim();
            user.Login = addLogin.Text.Trim();
            user.Password = addPassword.Text;

            string jout = JsonConvert.SerializeObject(user);
            sendRequest(jout);
        }

        private async void sendRequest(string jin)
        {
            ConnectHost conn = new ConnectHost();
            JToken jObject = await conn.AddAdmin(jin);
            if (jObject == null)
            {
                MessageBox.Show("Не удалось добавить администратора");
            }
            else
            {
                MessageBox.Show("Администратор успешно добавлен");
                GlobalRes.flagUpdateAdmin = true;
                addLastName.Text = string.Empty;
                addFirstName.Text = string.Empty;
                addSecondName.Text = string.Empty;
                addLogin.Text = string.Empty;
                addPassword.Text = string.Empty;
                GlobalRes.getUserEmployee();
            }

        }

        private void Button_Click_Back(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        public string CreatePassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }
    }
}
