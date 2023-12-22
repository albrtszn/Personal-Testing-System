using Client.VM;
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
using Wordroller.Content.Properties.Sections.PageSizes;
using Wordroller.Content.Tables;
using Wordroller.Styles;
using Wordroller;
using Microsoft.Win32;
using Client.classDTO;

namespace Client.pages
{
    /// <summary>
    /// Логика взаимодействия для PageResults.xaml
    /// </summary>
    public partial class PageResults : Page
    {
        private string IdUser = string.Empty;
        public PageResults(string id)
        {
            IdUser = id;
            InitializeComponent();
            Loaded += PageResults_Loaded;
        }

        private void PageResults_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = new PageResultsVM(this, IdUser);
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
            paragraph.AppendText("Список пройденных  тестов");
            paragraph.AppendText("\n");


            paragraph.AppendText("\n");
            //paragraph.AppendText("\r\n");

            PageResultsVM data = DataContext as PageResultsVM;
            PageResultsVM.ResultView[] res = data.Items.SourceCollection as PageResultsVM.ResultView[];

            paragraph.AppendText("Фамилия: " + res[0].result.Employee.LastName); paragraph.AppendTextBreak();
            paragraph.AppendText("Имя: " + res[0].result.Employee.FirstName); paragraph.AppendTextBreak();
            paragraph.AppendText("Отчество: " + res[0].result.Employee.SecondName); paragraph.AppendTextBreak();
            paragraph.AppendText("Дата рождения: " + res[0].result.Employee.DateOfBirth); paragraph.AppendTextBreak();
            paragraph.AppendText("Номер телефона: " + res[0].result.Employee.Phone); paragraph.AppendTextBreak();
            paragraph.AppendText("Дата регистрации: " + res[0].result.Employee.RegistrationDate); paragraph.AppendTextBreak();
            paragraph.AppendText("Профиль: " + GlobalRes.GetSubdivision(res[0].result.Employee.Subdivision.Id).Profile); paragraph.AppendTextBreak();
            paragraph.AppendText("Должность: " + res[0].result.Employee.Subdivision.Name); paragraph.AppendTextBreak();
            var table = section.AppendTable(new CreateTableParameters(WidthUnit.Pc, 100, res.Length + 1, WidthUnit.Pc, new double[] { 5f, 20f, 10f, 10f, 15f , 40f },
                                            DefaultTableStyles.TableNormal));
            var rows = table.Rows.ToArray();

            var cells = rows[0].Cells.ToArray();

            cells[0].Paragraphs.First().AppendText("№");
            cells[1].Paragraphs.First().AppendText("Тест (компетенция)");
            cells[2].Paragraphs.First().AppendText("Время тестирования, мин.");
            cells[3].Paragraphs.First().AppendText("Баллы");
            cells[4].Paragraphs.First().AppendText("Уровень компетенции");
            cells[5].Paragraphs.First().AppendText("Описание");


            for (int i = 0; i < res.Length; i++)
            {

                cells = rows[i + 1].Cells.ToArray();
                cells[0].Paragraphs.First().AppendText((i + 1).ToString());
                cells[1].Paragraphs.First().AppendText(res[i].result.Result.Test.Name);
                
                cells[2].Paragraphs.First().AppendText((res[i].result.Result.Duration + 1).ToString());
                cells[3].Paragraphs.First().AppendText(res[i].result.ScoreFrom.ToString());
                cells[4].Paragraphs.First().AppendText(res[i].level);
                cells[5].Paragraphs.First().AppendText(res[i].result.ResultLevel);

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

    }
}
