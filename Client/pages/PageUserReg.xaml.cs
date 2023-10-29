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
        
            subdivisionComboBox.ItemsSource = list;
            subdivisionComboBox.SelectedIndex = 0;

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
            user.Login = addPhone.Text.Trim();
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

        private void groupeCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateSubdivisions();
        }
    }
}
