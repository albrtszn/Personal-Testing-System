using Client.classDTO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
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
using static Client.VM.PageResultsVM;
using ScottPlot;

using Client.VM;
using System.Xml.Linq;
using System.Globalization;
using System.IO;
using Wordroller.Content.Properties.Sections.PageSizes;
using Wordroller.Content.Tables;
using Wordroller.Styles;
using Wordroller;
using Microsoft.Win32;
using MaterialDesignThemes.Wpf;
using System.Security.Cryptography;
using SkiaSharp;
using Wordroller.Content.Images;
using Wordroller.Content.Drawings;
using System.Reflection;

namespace Client.pages
{
    /// <summary>
    /// Логика взаимодействия для PageCompetencyAsses.xaml
    /// </summary>
    public partial class PageCompetencyAsses : Page
    {
        public class ListResultCompetence
        {
            public string Name { get; set; }
            public string Point { get; set; }
            public int Level { get; set; }
            public string Description { get; set; }
            public string Label { get; set; }

        }

        public class UserCompetency
        {
            public string LastName { get; set; }
            public string FirstName { get; set; }
            public string SecondName { get; set; }
            public string DateOfBirth { get; set; }
            public string Phone { get; set; }
            public string Profile { get; set; }
            public string Subdevision { get; set; }
            public string Competence1 { get; set; }
            public string Competence2 { get; set; }
            public string Competence3 { get; set; }
            public string Competence4 { get; set; }
            public int Point1 { get; set; }
            public int Point2 { get; set; }
            public int Point3 { get; set; }
            public int Point4 { get; set; }
            public string Description1 { get; set; }
            public string Description2 { get; set; }
            public string Description3 { get; set; }
            public string Description4 { get; set; }
            public int ItogLevel { get; set; }
            public double ItogPoint { get; set; }
        }

        CompetenceScoreDto[] competenceScore;
        public static PurposeDto[] purposes;
        public List<ListResultCompetence> listComp1 = new List<ListResultCompetence>();
        public List<ListResultCompetence> listComp2 = new List<ListResultCompetence>();
        public List<ListResultCompetence> listComp3 = new List<ListResultCompetence>();
        public List<ListResultCompetence> listComp4 = new List<ListResultCompetence>();

        private SavedImageInfo tmp_path;
        public UserCompetency userCom = new UserCompetency();
        public string IdUser = string.Empty;
        
        public PageCompetencyAsses(string Id)
        {
            InitializeComponent();
            IdUser = Id;
            Loaded += PageCompetencyAsses_Loaded;
        }

        private void PageCompetencyAsses_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        public async void LoadData()
        {
            ResultDto[] res;
           
            ConnectHost conn = new ConnectHost();
            BT_BackGo.IsEnabled = false;
            BT_exportWord.IsEnabled = false;

            string tmp_emp = "{\"Id\"" + ":\"" + IdUser + "\"}";

            var jsonSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            JToken jObject = await conn.GetResultsOfPurposesByEmployeeId(tmp_emp);

           
            if (jObject == null)
            {
                MessageBox.Show("Не удалось выгрузить результаты тестов");
                return;
            }

            res = JsonConvert.DeserializeObject<ResultDto[]>(jObject.ToString(), jsonSettings);


            int IdGroup = res[0].Employee.Subdivision.IdGroupPositions;

            float points = 0;
            float sum1 = 0;
            float sum2 = 0;
            float sum3 = 0;
            float sum4 = 0;
            
            int IdCompetenc = 0;
            int kol1 = 0;
            int kol2 = 0;
            int kol3 = 0;
            int kol4 = 0;

            //RB_table.IsChecked = true;
           // RB_pie.IsChecked = false;

            foreach (var purpose in res) 
            {
                ListResultCompetence bufList = new ListResultCompetence();

                bufList.Name = purpose.Result.Test.Name;
                if (purpose.Id == 0)
                {
                    points = 0;
                    bufList.Point = "";
                    bufList.Level = 0;
                    bufList.Description = "Тест не пройден";
                    bufList.Label = "Тест не пройден";
                }
                else
                {
                    points = purpose.NumberPoints;
                    bufList.Point = purpose.ScoreFrom.ToString();
                    bufList.Level = (int)purpose.NumberPoints * 100 / 3;
                    bufList.Description = purpose.ResultLevel;
                    bufList.Label = GetLevelString(purpose.NumberPoints);
                }

                IdCompetenc = purpose.Result.Test.CompetenceId;

                if (IdCompetenc == 1)
                {
                    listComp1.Add(bufList);
                    sum1 = sum1 + points;
                    kol1++;
                } 
                else if (IdCompetenc  == 2)
                {
                    listComp2.Add(bufList);
                    sum2 = sum2 + points;
                    kol2++;
                }
                else if (IdCompetenc == 3)
                {
                    listComp3.Add(bufList);
                    sum3 = sum3 + points;
                    kol3++;
                }
                else if (IdCompetenc == 4)
                {
                    listComp4.Add(bufList);
                    sum4 = sum4 + points;
                    kol4++;
                }


            }

            LV_ListComp1.ItemsSource = listComp1;
            LV_ListComp2.ItemsSource = listComp2;
            LV_ListComp3.ItemsSource = listComp3;
            LV_ListComp4.ItemsSource = listComp4;

            userCom.FirstName = res[0].Employee.FirstName;
            userCom.LastName = res[0].Employee.LastName;
            userCom.SecondName = res[0].Employee.SecondName;
            userCom.DateOfBirth = res[0].Employee.DateOfBirth;
            userCom.Phone = res[0].Employee.Phone;
            userCom.Subdevision = res[0].Employee.Subdivision.Name;
            userCom.Profile = GlobalRes.GetSubdivision(res[0].Employee.Subdivision.Id).Profile;
            userCom.Competence1 = "не определен";
            userCom.Competence2 = "не определен";
            userCom.Competence3 = "не определен";
            userCom.Competence4 = "не определен";

            tmp_emp = "{\"Id\"" + ":\"" + IdGroup + "\"}";
            jObject = await conn.GetCompetenceScoresByGroupId(tmp_emp);
            competenceScore = JsonConvert.DeserializeObject<CompetenceScoreDto[]>(jObject.ToString(), jsonSettings);

            //ResultView[] resultViews = new ResultView[res.Count()];

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
         

            userCom.Competence1 = GetLevelString(sum1);
            userCom.Competence2 = GetLevelString(sum2); 
            userCom.Competence3 = GetLevelString(sum3); 
            userCom.Competence4 = GetLevelString(sum4);

            userCom.Point1 = (int) (sum1 * 100 / 3);
            userCom.Point2 = (int) (sum2 * 100 / 3);
            userCom.Point3 = (int) (sum3 * 100 / 3);
            userCom.Point4 = (int) (sum4 * 100 / 3);

            userCom.ItogPoint = (sum1 * GlobalRes.matrixCoeff[1, IdGroup]) + (sum2 * GlobalRes.matrixCoeff[2, IdGroup]) + (sum3 * GlobalRes.matrixCoeff[3, IdGroup]) + (sum4 * GlobalRes.matrixCoeff[4, IdGroup]);
            userCom.ItogPoint = userCom.ItogPoint * 100 / 3;
            userCom.ItogPoint = Math.Round(userCom.ItogPoint,1);
            userCom.ItogLevel = (int)userCom.ItogPoint;

            userCom.Description1 = GetDescription(1, IdGroup, sum1);
            userCom.Description2 = GetDescription(2, IdGroup, sum2);
            userCom.Description3 = GetDescription(3, IdGroup, sum3);
            userCom.Description4 = GetDescription(4, IdGroup, sum4);

            this.DataContext = userCom;

            List<PieSlice> slices = new List<PieSlice>
            {
                new PieSlice() { Value = listComp1.Count, FillColor = ScottPlot.Colors.Pink, Label = "Профессиональные" , LabelFontSize = 14, LabelAlignment = Alignment.LowerLeft},
                new PieSlice() { Value = listComp2.Count, FillColor = ScottPlot.Colors.LightBlue, Label = "Корпоративные", LabelFontSize = 14, LabelAlignment = Alignment.UpperCenter},
                new PieSlice() { Value = listComp3.Count, FillColor = ScottPlot.Colors.LightGreen, Label = "Управленческие", LabelFontSize = 14, LabelAlignment = Alignment.UpperRight},
                new PieSlice() { Value = listComp4.Count, FillColor = ScottPlot.Color.FromHex("#FAFAD2"), Label = "Профессионально-психологические", LabelFontSize = 14, LabelAlignment = Alignment.LowerCenter },
            };

            myPlot.Plot.HideGrid();
            myPlot.Plot.Axes.SquareUnits();

            var pie = myPlot.Plot.Add.Pie(slices);
            pie.DonutFraction = .28;
            myPlot.Plot.Legend.IsVisible = false;
            pie.ShowSliceLabels = true;
            pie.SliceLabelDistance = 1.1;
            
            var line1 = myPlot.Plot.Add.Circle(0, 0, 0.52);
            line1.LineColor = ScottPlot.Colors.Black;
            line1.LinePattern = LinePattern.DenselyDashed;
            var line2 = myPlot.Plot.Add.Circle(0, 0, 0.76);
            line2.LineColor = ScottPlot.Colors.Black;
            line2.LinePattern = LinePattern.DenselyDashed;
            double sum_cnt = listComp1.Count + listComp2.Count + listComp3.Count + listComp4.Count;
            var result = listComp1.Concat(listComp2);
            result = result.Concat(listComp3);
            result = result.Concat(listComp4);

            double alfa = -2 * Math.PI / sum_cnt;
            double beta = Math.PI / sum_cnt;
            double alfa_g = 360.0 / sum_cnt;
            double beta_g = 180.0 / sum_cnt;

            double yy = 0, y0 = 0, xP = 0;
            double xx = 0, x0 = 0, yP = 0;
            List<ListResultCompetence> items = result.ToList<ListResultCompetence>();
            Coordinates[] pixels = new Coordinates[items.Count];
            double[] y_arr = new double[items.Count];
            double[] x_arr = new double[items.Count];
            for (int i = 0; i < sum_cnt; i++)
            {
                yy = Math.Sin(alfa*(i+1) + beta);
                xx = Math.Cos(alfa*(i+1) + beta);
                y0 = Math.Sin(alfa * (i + 1) + beta)*0.28;
                x0 = Math.Cos(alfa * (i + 1) + beta)*0.28;
                yP = Math.Sin(alfa * (i + 1) + beta) * (0.28 + (0.72 * items[i].Level / 100.0));
                xP = Math.Cos(alfa * (i + 1) + beta) * (0.28 + (0.72 * items[i].Level / 100.0));
                pixels[i].X = xP;
                pixels[i].Y = yP;
                x_arr[i]= x0;
                y_arr[i]= y0;   
                var line3 = myPlot.Plot.Add.Line(x0, y0, xx, yy);
                line3.LineColor = ScottPlot.Colors.Black;
                line3.LinePattern = LinePattern.DenselyDashed;

            }

    

            var poly = myPlot.Plot.Add.Polygon(pixels);
            poly.FillColor = ScottPlot.Color.FromHex("6970B5").WithAlpha(0.5);
            poly.LineColor = ScottPlot.Color.FromHex("6970B5");
            //var point3 = myPlot.Plot.Add.Markers(x_arr, y_arr, MarkerShape.FilledCircle, 10, ScottPlot.Color.FromHex("6970B5"));

            var text1 = myPlot.Plot.Add.Text(userCom.ItogPoint.ToString(), 0, 0);
            text1.LabelFontColor = ScottPlot.Colors.Black;
            text1.Alignment = Alignment.MiddleCenter;
            text1.LabelFontSize = 32;

            for (int i = 0; i < sum_cnt; i++)
            {
                var point3 = myPlot.Plot.Add.Marker(pixels[i].X, pixels[i].Y, MarkerShape.FilledCircle, 10);
                point3.LegendText = items[i].Name + " (" + items[i].Label + ", " + items[i].Level.ToString() + "%)";
                string strleg = String.Empty;
                if (items[i].Name.Length > 20)
                {
                    strleg = items[i].Name.Substring(0, 20) + "...";
                }
                else
                {
                    strleg = items[i].Name;
                }
                var text5 = myPlot.Plot.Add.Text(strleg, x_arr[i], y_arr[i]);
                
                text5.LabelFontColor = point3.MarkerFillColor;
                text5.LabelRotation = (float)(alfa_g * (i) + beta_g);
                text5.LabelFontSize = 12;
            }

            var text2 = myPlot.Plot.Add.Text("Низкий", 0.52, 0);
            text2.LabelFontColor = ScottPlot.Colors.Black;
            text2.Alignment = Alignment.UpperLeft;
            text2.OffsetY = 5;
            text2.OffsetX = 3;
            text2.LabelFontSize = 12;

            var text3 = myPlot.Plot.Add.Text("Средний", 0.76, 0);
            text3.LabelFontColor = ScottPlot.Colors.Black;
            text3.OffsetY = -5;
            text3.OffsetX = 3;
            text3.Alignment = Alignment.LowerLeft;
            text3.LabelFontSize = 12;

            var text4 = myPlot.Plot.Add.Text("Высокий", 1, 0);
            text4.LabelFontColor = ScottPlot.Colors.Black;
            text4.Alignment = Alignment.UpperLeft;
            text4.OffsetY = 5;
            text4.OffsetX = 3;
            text4.LabelFontSize = 12;

            myPlot.Plot.Add.Marker(0.52, 0, MarkerShape.FilledCircle, 7, ScottPlot.Colors.Black.WithAlpha(0.9));
            myPlot.Plot.Add.Marker(0.76, 0, MarkerShape.FilledCircle, 7, ScottPlot.Colors.Black.WithAlpha(0.9));
            myPlot.Plot.Add.Marker(1, 0, MarkerShape.FilledCircle, 7, ScottPlot.Colors.Black.WithAlpha(0.9));

            myPlot.Plot.Axes.SetLimitsY(-1.2, 1.2);
            myPlot.Plot.Axes.SetLimitsX(0, 2);
            myPlot.Plot.Layout.Frameless();
            myPlot.Plot.ShowLegend();


            myPlot.Plot.Legend.Alignment = Alignment.MiddleRight;
            myPlot.Plot.Legend.OutlineWidth = 0;
            myPlot.Plot.Legend.OutlineColor = ScottPlot.Colors.White;
            myPlot.Plot.Legend.ShadowColor = ScottPlot.Colors.White;
            myPlot.Interaction.Disable();
            myPlot.Refresh();
            tmp_path = myPlot.Plot.SaveJpeg("tmp_" + AutWindow.admin.Id + ".jpg", 1024, 400);

            BT_BackGo.IsEnabled = true;
            BT_exportWord.IsEnabled = true;
            PageLoad.Visibility = Visibility.Collapsed;
            View_pie.Visibility = Visibility.Collapsed;
            View_table.Visibility = Visibility.Visible;

        }

        string GetDescription(int IdCompetence, int IdGroupe, float level)
        {
            string strOut = string.Empty;
            int LEV = 0;

            if ((level > 0) && (level <= 1.3))
            { LEV = 1; }

            if ((level > 1.3) && (level <= 1.8))
            { LEV = 2; }

            if ((level > 1.8) && (level <= 2.2))
            { LEV = 2; }

            if ((level > 2.2) && (level <= 2.8))
            { LEV = 2; }

            if ((level > 2.8))
            { LEV = 3;  }

            foreach (var tmp in competenceScore)
            {
                if ((IdCompetence == tmp.IdCompetence) && (IdGroupe == tmp.IdGroup) && (LEV == tmp.NumberPoints))
                {
                    strOut = tmp.Description;
                    break;

                }
            }
            return strOut;
        }

        string GetLevelString(float  level)
        {
            string strOut = "не определен";

            if ((level > 0) && (level <= 1.3))
            { strOut = "низкий"; }

            if ((level > 1.3) && (level <= 1.8))
            { strOut = "ниже среднего"; }

            if ((level > 1.8) && (level <= 2.2))
            { strOut = "средний"; }

            if ((level > 2.2) && (level <= 2.8))
            { strOut = "выше среднего"; }

            if ((level > 2.8))
            { strOut = "высокий"; }
            return strOut;
        }

        private void Button_Click_Back(object sender, RoutedEventArgs e)
        {
            BT_exportWord.IsEnabled = false;
            this.NavigationService.GoBack();
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
            paragraph.AppendText("Результаты оценки групповых компетенций");
            paragraph.AppendText("\n");

            paragraph.AppendText(userCom.LastName + " "); paragraph.AppendText(userCom.FirstName + " "); paragraph.AppendText(userCom.SecondName);
            paragraph.AppendText("\n");

            paragraph.AppendText(userCom.Profile + " : " + userCom.Subdevision);
            paragraph.AppendText("\n");

            paragraph.AppendText("Дата рождения: "); paragraph.AppendText(userCom.DateOfBirth);
            paragraph.AppendText("\n");

            paragraph.AppendText("Номер телефона: "); paragraph.AppendText(userCom.Phone);
            paragraph.AppendText("\n");
            paragraph.AppendText("\n");


            var imageStream1 = File.OpenRead(tmp_path.Path);
           
            if (imageStream1 == null)
            {
                Console.WriteLine("Нет такого файла");
            }
           
            var image1 = doc.AddImage(imageStream1, KnownImageContentTypes.Jpg);
            var picture1 = section.WrapImageIntoInlinePicture(image1, "pie", "", 465, 182);
            paragraph.AppendPicture(picture1);
            paragraph.AppendText("\n");

            //paragraph.AppendText("\r\n");

            var table = section.AppendTable(new CreateTableParameters(WidthUnit.Pc, 100, 4 + 1, WidthUnit.Pc, new double[] { 5f, 20f, 20f, 15f, 40f }, DefaultTableStyles.TableGrid));
            var rows = table.Rows.ToArray();

            var cells = rows[0].Cells.ToArray();

            cells[0].Paragraphs.First().AppendText("№");
            cells[1].Paragraphs.First().AppendText("Групповая компетенция");
            cells[2].Paragraphs.First().AppendText("Компетенция");
            cells[3].Paragraphs.First().AppendText("Показатель компетенции");
            cells[4].Paragraphs.First().AppendText("Описание");

            int i = 0;
            cells = rows[i + 1].Cells.ToArray();
            cells[0].Paragraphs.First().AppendText((i + 1).ToString());
            cells[1].Paragraphs.First().AppendText("Профессиональные");
            cells[2].Paragraphs.First().AppendText("");
            cells[3].Paragraphs.First().AppendText(userCom.Point1.ToString());
            cells[4].Paragraphs.First().AppendText(userCom.Description1);

            i++;
            cells = rows[i + 1].Cells.ToArray();
            cells[0].Paragraphs.First().AppendText((i + 1).ToString());
            cells[1].Paragraphs.First().AppendText("Корпоративные");
            cells[2].Paragraphs.First().AppendText("");
            cells[3].Paragraphs.First().AppendText(userCom.Point2.ToString());
            cells[4].Paragraphs.First().AppendText(userCom.Description2);

            i++;

            cells = rows[i + 1].Cells.ToArray();
            cells[0].Paragraphs.First().AppendText((i + 1).ToString());
            cells[1].Paragraphs.First().AppendText("Управленческие");
            cells[2].Paragraphs.First().AppendText("");
            cells[3].Paragraphs.First().AppendText(userCom.Point3.ToString());
            cells[4].Paragraphs.First().AppendText(userCom.Description3);

            i++;

            cells = rows[i + 1].Cells.ToArray();
            cells[0].Paragraphs.First().AppendText((i + 1).ToString());
            cells[1].Paragraphs.First().AppendText("Профессионально-психологические");
            cells[2].Paragraphs.First().AppendText("");
            cells[3].Paragraphs.First().AppendText(userCom.Point4.ToString());
            cells[4].Paragraphs.First().AppendText(userCom.Description4);

            i++;
            //foreach (var item in result)
            //{

            //    cells = rows[i + 1].Cells.ToArray();
            //    cells[0].Paragraphs.First().AppendText((i + 1).ToString());
            //    cells[1].Paragraphs.First().AppendText(item.FIO);
            //    cells[2].Paragraphs.First().AppendText(item.DateOfBirth);
            //    cells[3].Paragraphs.First().AppendText(item.LevelCom1.ToString());
            //    cells[4].Paragraphs.First().AppendText(item.LevelCom1.ToString());

            //    i++;
            //}

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

        private void BT_GoPie_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new PageCompetencyAssesPie());
        }

        private void RB_table_Checked(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Table");
            if (View_table != null)
            {
                View_table.Visibility = Visibility.Visible;
                View_pie.Visibility = Visibility.Collapsed;
            }
        }

        private void RB_pie_Checked(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Pie");
            if (View_pie != null)
            {
                View_table.Visibility = Visibility.Collapsed;
                View_pie.Visibility = Visibility.Visible;
            }
        }
    }
}
