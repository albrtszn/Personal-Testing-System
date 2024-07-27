using Client.classDTO;
using Client.VM;
using Client.windows;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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
using System.Xml.Linq;
using Wordroller;
using Wordroller.Content.Lists;
using Wordroller.Content.Properties.Sections.PageSizes;
using Wordroller.Content.Tables;
using Wordroller.Styles;
using Microsoft.Win32;
using static Client.pages.PageCompetencyAsses;

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
            public double Point { get; set; }
            public int Level { get; set; }
            public double LevelCom1 { get; set; }
            public double LevelCom2 { get; set; }
            public double LevelCom3 { get; set; }
            public double LevelCom4 { get; set; }
            public bool IsSelected { get; set; }
            public string Sub {  get; set; }
            public string Prof {  get; set; }
            public string Groupe { get; set; }
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



            JToken jObject = await conn.GetEmployeesBySubdivisionId(tmpPay);
            if (jObject == null)
            {
                MessageBox.Show("Не удалось выгрузить пользователей");
                return;
            }

            PageLoad.Visibility = Visibility.Visible;
            employees = JsonConvert.DeserializeObject<EmployeeDto[]>(jObject.ToString(), jsonSettings);
            int kol_emp = 0;
            string tmp_emp = string.Empty;
            resualts = new ResualtAnalyse[employees.Length];

            foreach (var item in employees)
            {
                tmp_emp = "{\"Id\"" + ":\"" + item.Id + "\"}";
                jObject = await conn.GetResultsOfPurposesByEmployeeId(tmp_emp);
                if (jObject == null)
                {
                    continue;
                }

                res = JsonConvert.DeserializeObject<ResultDto[]>(jObject.ToString(), jsonSettings);

                resualts[kol_emp] = new ResualtAnalyse();
                resualts[kol_emp].IdEmployee = item.Id;
                resualts[kol_emp].FIO = item.LastName + " " + item.FirstName + " " + item.SecondName;
                resualts[kol_emp].Phone = item.Phone;
                resualts[kol_emp].DateOfBirth = item.DateOfBirth;
                resualts[kol_emp].RegistrationDate = item.RegistrationDate;
                resualts[kol_emp].Prof = subdivision.Profile;
                resualts[kol_emp].Sub = subdivision.Name;
                resualts[kol_emp].Groupe = subdivision.NameGroupPositions2;

                double[] tmp_double = GetPointResualAnalyse(res);
                resualts[kol_emp].Point = tmp_double[0];
                resualts[kol_emp].LevelCom1 =  tmp_double[1];
                resualts[kol_emp].LevelCom2 =  tmp_double[2];
                resualts[kol_emp].LevelCom3 =  tmp_double[3];
                resualts[kol_emp].LevelCom4 =  tmp_double[4];
                resualts[kol_emp].Level = (int)resualts[kol_emp].Point;

               
                kol_emp++;
              

            }
            PageLoad.Visibility = Visibility.Collapsed;

            if (kol_emp > 0)
            {
                ResultDG.ItemsSource = null;
                ResultDG.ItemsSource = resualts;
                ResultDG.Visibility = Visibility.Visible;
                BT_exportWord.IsEnabled = true;
            }
            else
            {
                ResultDG.ItemsSource = null;
                ResultDG.Visibility = Visibility.Hidden;
                BT_exportWord.IsEnabled = false;    
            }

            ResultDG.UpdateLayout();

        }

        private double[] GetPointResualAnalyse(ResultDto[] isRes)
        {
            double points = 0;
            double[] itog = new double[5];

            double sum1 = 0;
            double sum2 = 0;
            double sum3 = 0;
            double sum4 = 0;
            int IdCompetenc = 0;
            int kol1 = 0;
            int kol2 = 0;
            int kol3 = 0;
            int kol4 = 0;
            int IdGroup = isRes[0].Employee.Subdivision.IdGroupPositions;

            foreach (var purpose in isRes)
            {
                IdCompetenc = purpose.Result.Test.CompetenceId;
                if (purpose.Id == 0)
                {
                    points = 0;
                }
                else
                {
                    points = purpose.NumberPoints;
                }

                if (IdCompetenc == 1)
                {
                    sum1 = sum1 + points;
                    kol1++;
                }
                else if (IdCompetenc == 2)
                {
                    sum2 = sum2 + points;
                    kol2++;
                }
                else if (IdCompetenc == 3)
                {
                    sum3 = sum3 + points;
                    kol3++;
                }
                else if (IdCompetenc == 4)
                {
                    sum4 = sum4 + points;
                    kol4++;
                }

            }

            if (kol1 == 0)
            { sum1 = 0; }
            else
            {
                sum1 = sum1 / kol1;
            }

            if (kol2 == 0)
            { sum2 = 0; }
            else
            {
                sum2 = sum2 / kol2;
            }

            if (kol3 == 0)
            { sum3 = 0; }
            else
            {
                sum3 = sum3 / kol3;
            }

            if (kol4 == 0)
            { sum4 = 0; }
            else
            {
                sum4 = sum4 / kol4;
            }

            itog[0] = (sum1 * GlobalRes.matrixCoeff[1, IdGroup]) + (sum2 * GlobalRes.matrixCoeff[2, IdGroup]) + (sum3 * GlobalRes.matrixCoeff[3, IdGroup]) + (sum4 * GlobalRes.matrixCoeff[4, IdGroup]);
            itog[0] = (itog[0] * 100) / 3;
            itog[0] = Math.Round(itog[0], 1);

            itog[1] = Math.Round(sum1 * 100 / 3, 2); 
            itog[2] = Math.Round(sum2 * 100 / 3, 2);
            itog[3] = Math.Round(sum3 * 100 / 3, 2);
            itog[4] = Math.Round(sum4 * 100 / 3, 2);

            return itog;
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
            WordDocument doc = new WordDocument(CultureInfo.GetCultureInfo("ru-ru"));
            doc.Styles.DocumentDefaults.RunProperties.Font.Ascii = "Times New Roman";

            doc.Styles.DocumentDefaults.RunProperties.Font.HighAnsi = "Times New Roman";
            doc.Styles.DocumentDefaults.RunProperties.FontSize = 12;

            var section = doc.Body.Sections.First();
            var w = section.Properties.Size.WidthTw;
            var h = section.Properties.Size.HeightTw;
            section.Properties.Size.Orientation = PageOrientation.Landscape;
            section.Properties.Size.WidthTw = h;
            section.Properties.Size.HeightTw = w;


            var paragraph = section.AppendParagraph();
            paragraph.AppendText("Результаты оценки компетенций");
            paragraph.AppendText("\n");

            paragraph.AppendText("Профиль: "); paragraph.AppendText(resualts.First().Prof);
            paragraph.AppendText("\n");

            paragraph.AppendText("Группа должностей: "); paragraph.AppendText(resualts.First().Groupe);
            paragraph.AppendText("\n");

            paragraph.AppendText("Должность: "); paragraph.AppendText(resualts.First().Sub);
            paragraph.AppendText("\n");
            //paragraph.AppendText("\r\n");

            var table = section.AppendTable(new CreateTableParameters(WidthUnit.Pc, 100, resualts.Length + 1, WidthUnit.Pc, new double[] { 5f, 26.5f, 18.5f, 18.5f, 18.5f, 12.5f }, DefaultTableStyles.TableGrid));
            var rows = table.Rows.ToArray();

            var cells = rows[0].Cells.ToArray();

            cells[0].Paragraphs.First().AppendText("№");
            cells[1].Paragraphs.First().AppendText("ФИО");
            cells[2].Paragraphs.First().AppendText("Дата рождения");
            cells[3].Paragraphs.First().AppendText("Номер телефона");
            cells[4].Paragraphs.First().AppendText("Дата регистрации");
            cells[5].Paragraphs.First().AppendText("Показатель компетенций");

            int i = 0;
            foreach (var item in resualts)
            {

                cells = rows[i + 1].Cells.ToArray();
                cells[0].Paragraphs.First().AppendText((i + 1).ToString());
                cells[1].Paragraphs.First().AppendText(item.FIO);
                cells[2].Paragraphs.First().AppendText(item.DateOfBirth);
                cells[3].Paragraphs.First().AppendText(item.Phone);
                cells[4].Paragraphs.First().AppendText(item.RegistrationDate);
                cells[5].Paragraphs.First().AppendText(item.Point.ToString());
                i++;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Word file (*.docx)|*.docx";
            if (saveFileDialog.ShowDialog() == true)
            {
                using (var fileStream = new FileStream(saveFileDialog.FileName, FileMode.Create, FileAccess.Write, FileShare.Write))
                {
                    doc.Save(fileStream);
                }
            }
        }

        private void MenuItem_Click1(object sender, RoutedEventArgs e)
        { 

            var tmp = ResultDG.SelectedItem as ResualtAnalyse;
            MessageBox.Show(tmp.FIO);
            UserTestResult windowUserTest = new UserTestResult(tmp.IdEmployee);
            windowUserTest.Show();
        }

        private void Button_Click_Comparate(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new PageСomparateResult(resualts));
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            int isEnableButtonComparate = 0;
            foreach (var item in resualts)
            {
                if (item.IsSelected == true)
                {
                    isEnableButtonComparate++;
                    if (isEnableButtonComparate > 1)
                    break;
                }
            }

            BT_Comparate.IsEnabled = isEnableButtonComparate > 1;
        }
    }
}
