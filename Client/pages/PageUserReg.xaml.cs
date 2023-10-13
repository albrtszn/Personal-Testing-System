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
    /// <summary>
    /// Логика взаимодействия для PageUserReg.xaml
    /// </summary>
    public partial class PageUserReg : Page
    {
        private List<SubdivisionDto> list = new List<SubdivisionDto>();


        public PageUserReg()
        {
            InitializeComponent();
            LoadSubdivisions();
        }



        private void LoadSubdivisions()
        {

            profileCB.Text = "Механик";
            groupeCB.Text = "Группа 1";

            list.Clear();
            foreach (var tmp in GlobalRes.itemsSubdivision)
            {
                string iProfile = profileCB.Text;
                    
                string iGroupe = groupeCB.Text;

                if ((iProfile == tmp.Profile) && (iGroupe == tmp.NameGroupPositions))
                {
                    list.Add(tmp);
                }
                list.Add(tmp);

            }
            subdivisionComboBox.ItemsSource = list;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            EmployeeDto user = new EmployeeDto();
            if (addLastName.Text == null || addLastName.Text == "")
            {
                MessageBox.Show("Значение в поле фамилии не заполнено!");
                return;
            }

            if (addFirstName.Text == null || addFirstName.Text == "")
            {
                MessageBox.Show("Значение в поле имени не заполнено!");
                return;
            }

            if (addSecondName.Text == null || addSecondName.Text == "")
            {
                MessageBox.Show("Значение в поле отчества не заполнено!");
                return;
            }

            if (addDate.Text == null || addDate.Text == "")
            {
                MessageBox.Show("Значение в поле дата рождения не заполнено!");
                return;
            }

            if (addPhone.Text == null || addPhone.Text == "")
            {
                MessageBox.Show("Значение в поле номер телефона е заполнено!");
                return;
            }
            
            addLogin.Text = addPhone.Text;

            if (addPassword.Text == null || addPassword.Text == "")
            {
                MessageBox.Show("Значение в поле пароль!");
                return;
            }

            if (subdivisionComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Значение в поле подраздение не выбрано!");
                return;
            }

            user.LastName = addLastName.Text.Trim();
            user.FirstName = addFirstName.Text.Trim();
            user.SecondName = addSecondName.Text.Trim();
            user.Login = addLogin.Text.Trim();
            user.Password = addPassword.Text;
            user.DateOfBirth = addDate.Text;
            user.Phone = addPhone.Text.Trim(); ;
            user.IdSubdivision = list[subdivisionComboBox.SelectedIndex].Id;
            user.RegistrationDate = System.DateTime.Now.ToShortDateString();
            
            string jout = JsonConvert.SerializeObject(user);
            sendRequest(jout);
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
            }

        }

        private void groupeCB_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

        }

        private void groupeCB_Selected(object sender, SelectionChangedEventArgs e)
        {
            var ob = sender as ComboBox;

            if (ob != null)
            {
                list.Clear();
                foreach (var tmp in GlobalRes.itemsSubdivision)
                {
                    string iProfile = profileCB.Text;


                    string iGroupe = ob.Text;

                    if ((iProfile == tmp.Profile) && (iGroupe == tmp.NameGroupPositions))
                    {
                        list.Add(tmp);
                    }

                }
                if (list.Count > 0)
                {
                    subdivisionComboBox.ItemsSource = list;
                }
            }
        }
    }
}
