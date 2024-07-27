using Client.pages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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
using System.Xml.Linq;
using Wordroller;
using Wordroller.Content.Images;
using Microsoft.Win32;
using Wordroller.Content.Tables;
using Wordroller.Styles;
using Wordroller.Content.Properties.Sections.PageSizes;
using Wordroller.Content.Properties.Sections;
using Wordroller.Content.Text;
using Client.classDTO;

namespace Client.forms
{
    /// <summary>
    /// Логика взаимодействия для PageUsers.xaml
    /// </summary>
    public partial class PageUsers : Page
    {
        

        public PageUsers()
        {
            InitializeComponent();
           
            Loaded += PageUsers_Loaded;
        }
              



        private void PageUsers_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = new VM.UserVM(this);
        }
        private bool isButtonClick = false;

        private void ShowFilter(object sender, RoutedEventArgs e)
        {
            if (isButtonClick)
            {
                BorderFilter.Visibility = Visibility.Collapsed;
                isButtonClick = false;
            }
            else
            {
                BorderFilter.Visibility = Visibility.Visible;
                isButtonClick = true;
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
            paragraph.AppendText("Список всех пользователей");
            paragraph.AppendText("\n");

            //paragraph.AppendText("\r\n");

            VM.UserVM user = DataContext as VM.UserVM;
            classDTO.UserEmployee[] users = user.Items.SourceCollection as classDTO.UserEmployee[];

            var table = section.AppendTable(new CreateTableParameters(WidthUnit.Pc, 100, users.Length + 1, WidthUnit.Pc, new double[] {5f, 12.5f, 12.5f, 12.5f, 10f, 10f, 11f, 10f, 15f },
    DefaultTableStyles.TableNormal));
            var rows = table.Rows.ToArray();
            
            var cells = rows[0].Cells.ToArray();

            cells[0].Paragraphs.First().AppendText("№");
            cells[1].Paragraphs.First().AppendText("Фамилия");
            cells[2].Paragraphs.First().AppendText("Имя");
            cells[3].Paragraphs.First().AppendText("Отчество");
            cells[4].Paragraphs.First().AppendText("Дата рождения");
            cells[5].Paragraphs.First().AppendText("Номер телефона");
            cells[6].Paragraphs.First().AppendText("Дата регистрации");
            cells[7].Paragraphs.First().AppendText("Профиль");
            cells[8].Paragraphs.First().AppendText("Должность");

            for (int i = 0; i < users.Length; i++)
            {

                cells = rows[i+1].Cells.ToArray();
                cells[0].Paragraphs.First().AppendText((i+1).ToString());
                cells[1].Paragraphs.First().AppendText(users[i].employee.LastName);
                cells[2].Paragraphs.First().AppendText(users[i].employee.FirstName);
                cells[3].Paragraphs.First().AppendText(users[i].employee.SecondName);
                cells[4].Paragraphs.First().AppendText(users[i].employee.DateOfBirth);
                cells[5].Paragraphs.First().AppendText(users[i].employee.Phone);
                cells[6].Paragraphs.First().AppendText(users[i].employee.RegistrationDate);
                cells[7].Paragraphs.First().AppendText(users[i].prof);
                cells[8].Paragraphs.First().AppendText(users[i].sub);
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

        private void DataGridD_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            UserEmployee tmp = (UserEmployee)e.Row.DataContext;
            if (tmp.employee.CountOfResults == tmp.employee.CountOfPurposes)

            {
                e.Row.Background = new SolidColorBrush(Color.FromRgb(0xF0, 0xFF, 0xFF));
            }
           
        }
    }
}
