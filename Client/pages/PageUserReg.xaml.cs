using Client.classDTO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using static Client.forms.addSubdivision;

namespace Client.forms
{
    public partial class PageUserReg : Page
    {
        private List<SubdivisionDto> list = new List<SubdivisionDto>();
        private CollectionViewSource _viewSource = new CollectionViewSource();

        public PageUserReg()
        {
            InitializeComponent();
            Loaded += PageUserReg_Loaded;
            
        }

        private void PageUserReg_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = _viewSource;

            LoadSubdivisions();
        }

        private void LoadSubdivisions()
        {

            list.Clear();
            foreach (var tmp in GlobalRes.itemsSubdivision)
            {
                string iProfile = profileCB.Text;

                string iGroupe = groupeCB.Text;
                int selecGroupe = groupeCB.SelectedIndex;

                if ((iProfile == tmp.Profile) && (selecGroupe + 1 == tmp.IdGroupPositions))
                {
                    list.Add(tmp);
                    Console.WriteLine(tmp.Name);
                }

                //list.Add(tmp);

            }

            addPassword.Text = CreatePassword(6);
            subdivisionComboBox.ItemsSource = list;
            subdivisionComboBox.SelectedIndex = 0;

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

        private void UpdateSubdivisions()
        {

            if (subdivisionComboBox != null)
            {

                subdivisionComboBox.ItemsSource = null;

                list.Clear();
                foreach (var tmp in GlobalRes.itemsSubdivision)
                {
                    string iProfile = profileCB.Text;

                    string iGroupe = groupeCB.Text;
                    int selecGroupe = groupeCB.SelectedIndex;
                    string tmp_str = string.Empty;
                    if (selecGroupe == 0)
                    {
                        tmp_str = "Группа 1";
                    } else if (selecGroupe == 1)
                    {
                        tmp_str = "Группа 2";
                    } else if (selecGroupe == 2)
                    {
                        tmp_str = "Группа 3";
                    }
                    else
                    {
                        tmp_str = "Группа 4";
                    }


                    if ((iProfile == tmp.Profile) && (tmp_str == tmp.NameGroupPositions))
                    {
                        list.Add(tmp);
                        Console.WriteLine(tmp.Name);
                    }

                }

                subdivisionComboBox.ItemsSource = list;
                subdivisionComboBox.SelectedIndex = -1;
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            EmployeeDto user = new EmployeeDto();
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


            if (addDate3.Text == null || addDate3.Text == "")
            {
                addDate3.Background = Brushes.Pink;
                MessageBox.Show("Значение в поле дата рождения (год) заполнено неверно!");
                return;
            }
            
            if ( falidDateBirthday(addDate1.Text + "." + addDate2.Text + "." + addDate3.Text))
            {
                addDate3.Background = Brushes.Pink;
                addDate1.Background = Brushes.Pink;
                addDate2.Background = Brushes.Pink;
                MessageBox.Show("Значение полей даты рождения заполнено неверно!");
                return;
            }
            else
            {
                DateTime dateTime = DateTime.Parse(addDate1.Text + "." + addDate2.Text + "." + addDate3.Text);
                int tmpTime = System.DateTime.Now.Year - dateTime.Year;
                if (tmpTime <= 17)
                {
                    addDate3.Background = Brushes.Pink;
                    addDate1.Background = Brushes.Pink;
                    addDate2.Background = Brushes.Pink;
                    MessageBox.Show("Значение в поле год, даты рождения заполнено неверно!");
                    return;
                }

                addDate1.Background = Brushes.Transparent;
                addDate2.Background = Brushes.Transparent;
                addDate3.Background = Brushes.Transparent;
            }

            if (addPhone.Text == null || addPhone.Text == "" || addPhone.Text.Length != 9 ) 
            {
                addPhone.Background = Brushes.Pink;
                MessageBox.Show("Значение в поле номер телефона заполнено неверно!");
                return;
            }
            else
            {

                double number;
                if (double.TryParse(addPhone.Text, out number))
                {
                    if (number > 0)
                    {
                        addPhone.Background = Brushes.Transparent;
                    }
                    else
                    {
                        addPhone.Background = Brushes.Pink;
                        MessageBox.Show("Значение в поле номер телефона заполнено неверно!");
                        return;
                    }
                }
                else
                {
                    addPhone.Background = Brushes.Pink;
                    MessageBox.Show("Значение в поле номер телефона заполнено неверно!");
                    return;
                }

               
            }

            if (addPassword.Text == null || addPassword.Text == "")
            {
                addPassword.Background = Brushes.Pink;
                MessageBox.Show("Значение в поле пароль заполнено неверно!");
                return;
            }
            else
            {

                foreach (var tmpUser in GlobalRes.itemsUserEmployee)
                {
                    if (tmpUser.employee.Phone != null)
                    {
                        if (tmpUser.employee.Phone.Contains(addPhone.Text))
                        {
                            if (tmpUser.employee.LastName.Contains(addLastName.Text))
                            {
                                if (tmpUser.employee.DateOfBirth.Contains(addDate3.Text))
                                {
                                    if (addPassword.Text == tmpUser.employee.Password)
                                    {
                                        addPassword.Background = Brushes.Pink;
                                        MessageBox.Show("Измените значение в поле пароль!");
                                        return;
                                    }

                                }
                            }

                        }
                    }
                }

                addPassword.Background = Brushes.Transparent;

            }

            if (subdivisionComboBox.SelectedIndex == -1)
            {
                subdivisionComboBox.Background = Brushes.Pink;
                MessageBox.Show("Значение в поле должность не выбрано!");
                return;
            }
            else
            {
                subdivisionComboBox.Background = Brushes.Transparent;
            }

            user.LastName = addLastName.Text.Trim();
            user.FirstName = addFirstName.Text.Trim();
            user.SecondName = addSecondName.Text.Trim();
            user.Login = addPhone.Text.Trim();
            user.Password = addPassword.Text;
            user.DateOfBirth = DateTime.Parse(addDate1.Text + "." + addDate2.Text + "." + addDate3.Text).ToShortDateString();
            user.Phone = addPhone.Text.Trim();
            user.IdSubdivision = list[subdivisionComboBox.SelectedIndex].Id;
            user.RegistrationDate = System.DateTime.Now.ToShortDateString();
            
            string jout = JsonConvert.SerializeObject(user);
            sendRequest(jout);
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

        private async void sendRequest(string jin)
        {
            ConnectHost conn = new ConnectHost();
            JToken jObject = await conn.AddEmployee(jin);
            if (jObject == null)
            {
                MessageBox.Show("Не удалось добавить пользователя");
            }
            else
            {
                MessageBox.Show("Пользователь успешно добавлен");
                GlobalRes.flagUpdateEmployee = true;
                addLastName.Text = string.Empty;
                addFirstName.Text = string.Empty;
                addSecondName.Text = string.Empty;
                addPhone.Text = string.Empty;
                addPassword.Text = string.Empty;
                addDate3.Text = string.Empty;
                addPhone.Text = string.Empty;

                GlobalRes.getUserEmployee();
            }

        }

        private void groupeCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateSubdivisions();
        }
    }
}
