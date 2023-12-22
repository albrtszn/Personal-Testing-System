using Client.classDTO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
using Wordroller.Content.Lists;

namespace Client.pages
{
    /// <summary>
    /// Логика взаимодействия для PageAnalyseResult.xaml
    /// </summary>
    public partial class PageAnalyseResult : Page
    {
        public ResultDto[] res;

        public class ResualtAnalyse
        {
            public string IdEmployee { get; set; }
            public string FIO { get; set; }
            public string DateOfBirth { get; set; }
            public string Phone { get; set; }
            public string RegistrationDate { get; set; }
            public int Point { get; set; }
            public int LevelCom1 { get; set; }
            public int LevelCom2 { get; set; }
            public int LevelCom3 { get; set; }
            public int LevelCom4 { get; set; }
            public ResultDto[] Result { get; set; }
        }

        public ResualtAnalyse[] resualts;

        private List<SubdivisionDto> list = new List<SubdivisionDto>();

        public PageAnalyseResult()
        {
            InitializeComponent();
            ResultDG.Visibility = Visibility.Hidden;
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

                }
            }

            subdivisionComboBox.ItemsSource = list;
            subdivisionComboBox.SelectedIndex = -1;

        }

        private void groupeCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateSubdivisions();
        }

        private void selected_item(object sender, SelectionChangedEventArgs e)
        {
            var subd = (SubdivisionDto)subdivisionComboBox.SelectedItem;

            if (subd != null)
            {
                LoadData(subd);
            }


        }


        private async void LoadData(SubdivisionDto subdivision)
        {
            ConnectHost conn = new ConnectHost();
            EmployeeDto[] employees;
            string tmpPay = "{\"Id\":" + subdivision.Id.ToString() + "}";
            var jsonSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };
            JToken jObject = await conn.GetResultsBySubdivision(tmpPay);
            if (jObject == null)
            {
                MessageBox.Show("Не удалось добавить пользователя");
                return;
            }
            res = JsonConvert.DeserializeObject<ResultDto[]>(jObject.ToString(), jsonSettings);

            jObject = await conn.GetEmployees();
            if (jObject == null)
            {
                MessageBox.Show("Не удалось добавить пользователя");
                return;
            }

            if (res.Length > 0)
            {
                employees = JsonConvert.DeserializeObject<EmployeeDto[]>(jObject.ToString(), jsonSettings);
                int kol_emp = 0;
                foreach (var item in employees)
                {
                    if ((item.CountOfResults > 0) && (item.IdSubdivision == subdivision.Id))
                    {
                        kol_emp++;
                    }

                }
                resualts = new ResualtAnalyse[kol_emp];
                kol_emp = 0;
                foreach (var item in employees)
                {
                    if ((item.CountOfResults > 0) && (item.IdSubdivision == subdivision.Id))
                    {
                        resualts[kol_emp] = new ResualtAnalyse();
                        resualts[kol_emp].FIO = item.LastName + " " + item.FirstName + " " + item.SecondName;
                        resualts[kol_emp].Phone = item.Phone;
                        resualts[kol_emp].DateOfBirth = item.DateOfBirth;
                        resualts[kol_emp].RegistrationDate = item.RegistrationDate;
                    }

                }

                ResultDG.ItemsSource = null;
                ResultDG.ItemsSource = resualts;
                ResultDG.UpdateLayout();
                ResultDG.Visibility = Visibility.Visible;
            }
            else
            {
                ResultDG.ItemsSource = null;

                ResultDG.UpdateLayout();
                ResultDG.Visibility = Visibility.Hidden;
            }
     
            
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
                    }
                    else if (selecGroupe == 1)
                    {
                        tmp_str = "Группа 2";
                    }
                    else if (selecGroupe == 2)
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

        private void Button_export_word(object sender, RoutedEventArgs e)
        {

        }
    }
}
