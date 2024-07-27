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

namespace Client.pages
{
    /// <summary>
    /// Логика взаимодействия для PageСomparateResult.xaml
    /// </summary>
    public partial class PageСomparateResult : Page
    {
        private List <PageAnalyseResult.ResualtAnalyse> result = new List<PageAnalyseResult.ResualtAnalyse>();
        public PageСomparateResult(PageAnalyseResult.ResualtAnalyse[] RES)
        {
            foreach (var item in RES) 
            {
                if (item.IsSelected == true)
                {
                    result.Add(item);
                }
            }
            InitializeComponent();
            Loaded += PageСomparateResult_Loaded;
        }

        private void PageСomparateResult_Loaded(object sender, RoutedEventArgs e)
        {
            ResultDG.ItemsSource = result;
            ResultDG.UpdateLayout();
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
            paragraph.AppendText("Сравнение результатов оценки групповых компетенций");
            paragraph.AppendText("\n");

            paragraph.AppendText("Профиль: "); paragraph.AppendText(result.First().Prof);
            paragraph.AppendText("\n");

            paragraph.AppendText("Группа должностей: "); paragraph.AppendText(result.First().Groupe);
            paragraph.AppendText("\n");

            paragraph.AppendText("Должность: "); paragraph.AppendText(result.First().Sub);
            paragraph.AppendText("\n");
            //paragraph.AppendText("\r\n");

            var table = section.AppendTable(new CreateTableParameters(WidthUnit.Pc, 100, result.Count + 1, WidthUnit.Pc, new double[] { 2.5f, 22.5f, 12.5f, 12.5f, 12.5f, 12.5f, 12.5f, 12.5f },    DefaultTableStyles.TableGrid));
            var rows = table.Rows.ToArray();

            var cells = rows[0].Cells.ToArray();

            cells[0].Paragraphs.First().AppendText("№");
            cells[1].Paragraphs.First().AppendText("ФИО");
            cells[2].Paragraphs.First().AppendText("Дата рождения");
            cells[3].Paragraphs.First().AppendText("Профессиональные");
            cells[4].Paragraphs.First().AppendText("Корпоративные");
            cells[5].Paragraphs.First().AppendText("Управленческие");
            cells[6].Paragraphs.First().AppendText("Профессионально-психологические");
            cells[7].Paragraphs.First().AppendText("Балл");

            int i = 0;
            foreach ( var item in result)
            {

                cells = rows[i + 1].Cells.ToArray();
                cells[0].Paragraphs.First().AppendText((i + 1).ToString());
                cells[1].Paragraphs.First().AppendText(item.FIO);
                cells[2].Paragraphs.First().AppendText(item.DateOfBirth);
                cells[3].Paragraphs.First().AppendText(item.LevelCom1.ToString());
                cells[4].Paragraphs.First().AppendText(item.LevelCom1.ToString());
                cells[5].Paragraphs.First().AppendText(item.LevelCom1.ToString());
                cells[6].Paragraphs.First().AppendText(item.LevelCom1.ToString());
                cells[7].Paragraphs.First().AppendText(item.Point.ToString());
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

        private void Button_Click_Back(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();    
        }
    }
}
