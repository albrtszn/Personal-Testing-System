using Client.classDTO;
using Microsoft.Win32;
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
    public partial class PageUserEdit : Page
    {
        private EmployeeDto user = new EmployeeDto();

        public PageUserEdit(EmployeeDto myUser)
        {
            InitializeComponent();
            user = myUser;
            DataContext = user;
            profileCB.Text = GlobalRes.GetSubdivision(user.IdSubdivision).Profile;
            groupeCB.Text = GlobalRes.GetSubdivision(user.IdSubdivision).NameGroupPositions2;
            subdivisionComboBox.Text = GlobalRes.GetSubdivision(user.IdSubdivision).Name;
            DataRegUpdate.Text = user.RegistrationDate.ToString();
        }

        private void Button_Click_Back(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }


        private async void Button_Click_update(object sender, RoutedEventArgs e)
        {
            BT_update.IsEnabled = false;
           
            if (user.LastName == null || user.LastName == "" || user.LastName.Length < 2)
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

            if (user.FirstName == null || user.FirstName == "" || user.FirstName.Length < 2)
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

            if (user.SecondName == null || user.SecondName == "" || user.SecondName.Length < 2)
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


            if (falidDateBirthday(addDate.Text))
            {
                
                addDate.Background = Brushes.Pink;
                MessageBox.Show("Значение поля даты рождения заполнено неверно!");
                BT_update.IsEnabled = true;
                return;
            }
            else
            {
                DateTime dateTime = DateTime.Parse(addDate.Text);
                int tmpTime = System.DateTime.Now.Year - dateTime.Year;
                if (tmpTime <= 17)
                {
                    addDate.Background = Brushes.Pink;
                    MessageBox.Show("Значение поля даты рождения заполнено неверно!");
                    BT_update.IsEnabled = true;
                    return;
                }

               addDate.Background = Brushes.Transparent;
            }

            if (user.Phone == null || user.Phone == "" || user.Phone.Length != 9)
            {
                addPhone.Background = Brushes.Pink;
                MessageBox.Show("Значение в поле номер телефона заполнено неверно!");
                BT_update.IsEnabled = true;
                return;
            }
            else
            {

                double number;
                if (double.TryParse(user.Phone, out number))
                {
                    if (number > 0)
                    {
                        addPhone.Background = Brushes.Transparent;
                    }
                    else
                    {
                        addPhone.Background = Brushes.Pink;
                        MessageBox.Show("Значение в поле номер телефона заполнено неверно!");
                        BT_update.IsEnabled = true;
                        return;
                    }
                }
                else
                {
                    addPhone.Background = Brushes.Pink;
                    MessageBox.Show("Значение в поле номер телефона заполнено неверно!");
                    BT_update.IsEnabled = true;
                    return;
                }


            }

            if (user.Password == null || user.Password == "")
            {
                addPassword.Background = Brushes.Pink;
                MessageBox.Show("Значение в поле пароль заполнено неверно!");
                BT_update.IsEnabled = true;
                return;
            }
            else
            {

                foreach (var tmpUser in GlobalRes.itemsUserEmployee)
                {
                    if (tmpUser.employee.Phone != null)
                    {
                        if (tmpUser.employee.Phone.Contains(user.Phone))
                        {
                            if (tmpUser.employee.LastName.Contains(user.LastName))
                            {
                                if (tmpUser.employee.DateOfBirth.Contains(user.DateOfBirth))
                                {
                                    if (user.Password == tmpUser.employee.Password)
                                    {
                                        if (user.Login == tmpUser.employee.Login)
                                        {
                                            if (user.Id != tmpUser.employee.Id)
                                            {
                                                addPassword.Background = Brushes.Pink;
                                                MessageBox.Show("Измените значение в поле пароль!");
                                                BT_update.IsEnabled = true;
                                                return;
                                            }
                                        }

                                    }

                                }
                            }

                        }
                    }
                }

                addPassword.Background = Brushes.Transparent;

            }



            string tmp = JsonConvert.SerializeObject(user);
            ConnectHost conn = new ConnectHost();
            JToken jObject = await conn.UpdateEmployee(tmp);
            if (jObject != null)
            {
                MessageBox.Show("Данные пользователя успешно обновлены");
                
            }
            else
            {
                MessageBox.Show("Данные не обновлены!");
            }
            BT_update.IsEnabled = true;
        }

        private bool falidDateBirthday(string v)
        {
            try
            {
                DateTime.Parse(v);
            }
            catch (Exception ex)
            {
                return true;
            }
            return false;
        }
    }
}
