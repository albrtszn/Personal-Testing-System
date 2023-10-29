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
using static ScottPlot.Plottable.PopulationPlot;

namespace Client.pages
{
    /// <summary>
    /// Логика взаимодействия для PageCompetencyAsses.xaml
    /// </summary>
    public partial class PageCompetencyAsses : Page
    {
        public class UserCompetency
        {
            public string LastName { get; set; }
            public string FirstName { get; set; }
            public string SecondName { get; set; }
            public string DateOfBirth { get; set; }
            public string Phone { get; set; }
            public string Competence1 { get; set; }
            public string Competence2 { get; set; }
            public string Competence3 { get; set; }
            public string Competence4 { get; set; }
        }

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
            string[] Xlabels = { "Профессиональные", "Корпоративные", "Управленческие", "Профессионально-личностные" };
            string[] Ylabels = { "не определено", "низкий", "средний", "высокий" };
            double[] positions = { 0, 1, 2, 3 };
            WpfPlot1.Plot.XTicks(positions, Xlabels);
            WpfPlot1.Plot.XAxis.TickLabelStyle(rotation: 45);
            WpfPlot1.Plot.YTicks(positions, Ylabels);
            WpfPlot1.Plot.SetAxisLimitsY(0, 3.5);
            WpfPlot1.Plot.SetAxisLimitsX(-1, 4);
            WpfPlot1.Configuration.RightClickDragZoom = false;
            WpfPlot1.Configuration.ScrollWheelZoom = false;
            WpfPlot1.Configuration.DoubleClickBenchmark = false;
            WpfPlot1.Configuration.LockVerticalAxis = true;
            WpfPlot1.Configuration.LockHorizontalAxis = true;
            //WpfPlot1.Plot.SetAx(0, 4);
            LoadData();
        }

        public async void LoadData()
        {
            ResultDto[] res;
            AnswersUserTest ans;

            ConnectHost conn = new ConnectHost();

            BT_BackGo.IsEnabled = false;



            string tmp_emp = "{\"Id\"" + ":\"" + IdUser + "\"}";
            JToken jObject = await conn.GetResultsByEmployee(tmp_emp);

            var jsonSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            if (jObject == null)
            {
                return;
            }
            res = JsonConvert.DeserializeObject<ResultDto[]>(jObject.ToString(), jsonSettings);

            userCom.FirstName = res[0].Employee.FirstName;
            userCom.LastName = res[0].Employee.LastName;
            userCom.SecondName = res[0].Employee.SecondName;
            userCom.DateOfBirth = res[0].Employee.DateOfBirth;
            userCom.Phone = res[0].Employee.Phone;
            userCom.Competence1 = "не определен";
            userCom.Competence2 = "не определен";
            userCom.Competence3 = "не определен";
            userCom.Competence4 = "не определен";


            ResultView[] resultViews = new ResultView[res.Count()];


            int i = 0;
            int points = 0;
            int sum1 = 0;
            int sum2 = 0;
            int sum3 = 0;
            int sum4 = 0;
            int sumQ = 0;

            foreach (var item in res)
            {
                if (item.ScoreFrom == 0)
                {
                    points = 0;
                    string tmp_pay = "{\"Id\"" + ":\"" + item.Result.Id + "\"}";
                    jObject = await conn.GetEmployeeResultAnswers(tmp_pay);
                    if (jObject != null)
                    {
                        ans = JsonConvert.DeserializeObject<AnswersUserTest>(jObject.ToString(), jsonSettings);
                        points = Rating.GetPointTest(ans, item.Result.Test.Id);
                    }

                    resultViews[i] = new ResultView();
                    resultViews[i].result = new ResultDto();
                    resultViews[i].result = item;
                    resultViews[i].prof = GlobalRes.GetSubdivision(item.Employee.Subdivision.Id).Profile;
                    resultViews[i].points = points;
                    resultViews[i].level = Rating.GetLevelTestPoit(item.Result.Test.Id, resultViews[i].points);
                    resultViews[i].GroupeSubName = GlobalRes.GetSubdivision(item.Employee.Subdivision.Id).NameGroupPositions;
                    resultViews[i].IdCompetenc = item.Result.Test.CompetenceId;
                    resultViews[i].QuestionsCount = item.Result.Test.QuestionsCount;
                }
                else
                {

                    resultViews[i] = new ResultView();
                    resultViews[i].result = new ResultDto();
                    resultViews[i].result = item;
                    resultViews[i].prof = GlobalRes.GetSubdivision(item.Employee.Subdivision.Id).Profile;
                    resultViews[i].points = item.ScoreFrom;
                    resultViews[i].level = Rating.GetLevelTestPoit(item.Result.Test.Id, resultViews[i].points);
                    resultViews[i].GroupeSubName = GlobalRes.GetSubdivision(item.Employee.Subdivision.Id).NameGroupPositions;
                    resultViews[i].IdCompetenc = item.Result.Test.CompetenceId;
                    resultViews[i].QuestionsCount = item.Result.Test.QuestionsCount;
                }

                if (resultViews[i].IdCompetenc == 1)
                {
                    sumQ = sumQ + resultViews[i].QuestionsCount;
                    sum1 = sum1 + resultViews[i].points;
                }
                else if (resultViews[i].IdCompetenc == 2)
                {
                    sum2 = sum2 + resultViews[i].points;
                }
                else if (resultViews[i].IdCompetenc == 3)
                {
                    sum3 = sum3 + resultViews[i].points;
                }
                else
                {
                    sum4 = sum4 + resultViews[i].points;
                }

                i++;
            }

            string res1 = "не определен";
            string res2 = "не определен";
            string res3 = "не определен";
            string res4 = "не определен";
            int final1 = 0;

            if (resultViews[0].GroupeSubName == "Группа 1")
            {
                if (sum2 >= 59)
                {
                    res2 = "высокий";
                }
                else if ((sum2 >= -16) && (sum2 <= 58))
                {
                    res2 = "средний";
                }
                else
                {
                    res2 = "низкий";
                }

                final1 = ((sum1 * 100) / sumQ);

                if (final1 > 75)
                {
                    res1 = "высокий";
                }
                else if ((final1 >= 55) && (sum2 <= 74))
                {
                    res1 = "средний";
                }
                else
                {
                    res1 = "низкий";
                }
            }

            if (resultViews[0].GroupeSubName == "Группа 2")
            {
                if (sum2 >= 248)
                {
                    res2 = "высокий";
                }
                else if ((sum2 >= 103) && (sum2 <= 247))
                {
                    res2 = "средний";
                }
                else
                {
                    res2 = "низкий";
                }

                final1 = ((sum1 * 100) / sumQ);

                if (final1 > 75)
                {
                    res1 = "высокий";
                }
                else if ((final1 >= 55) && (sum2 <= 74))
                {
                    res1 = "средний";
                }
                else
                {
                    res1 = "низкий";
                }
            }

            if (resultViews[0].GroupeSubName == "Группа 3")
            {
                if (sum2 >= 478)
                {
                    res2 = "высокий";
                }
                else if ((sum2 >= 218) && (sum2 <= 477))
                {
                    res2 = "средний";
                }
                else
                {
                    res2 = "низкий";
                }

                final1 = ((sum1 * 100) / sumQ);

                if (final1 > 75)
                {
                    res1 = "высокий";
                }
                else if ((final1 >= 55) && (sum2 <= 74))
                {
                    res1 = "средний";
                }
                else
                {
                    res1 = "низкий";
                }

            }

            if (resultViews[0].GroupeSubName == "Группа 4")
            {
                if (sum2 >= 731)
                {
                    res2 = "высокий";
                }
                else if ((sum2 >= 345) && (sum2 <= 730))
                {
                    res2 = "средний";
                }
                else
                {
                    res2 = "низкий";
                }

                final1 = ((sum1 * 100) / sumQ);

                if (final1 > 75)
                {
                    res1 = "высокий";
                }
                else if ((final1 >= 55) && (sum2 <= 74))
                {
                    res1 = "средний";
                }
                else
                {
                    res1 = "низкий";
                }
            }

            userCom.Competence1 = res1;
            userCom.Competence2 = res2;
            userCom.Competence3 = res3;
            userCom.Competence4 = res4;

            this.DataContext = userCom;
            BT_BackGo.IsEnabled = true;



     
            List<ScottPlot.Plottable.Bar> bars = new List<ScottPlot.Plottable.Bar>();
            
            int value = 0;
            if (userCom.Competence1 == "не определен")
            {
                value = 0;
            } else if (userCom.Competence1 == "низкий")
            {
                value = 1;
            } else if (userCom.Competence1 == "средний")
            {
                value = 2;
            } else
            {
                value = 3;
            }

            ScottPlot.Plottable.Bar bar0 = new ScottPlot.Plottable.Bar()
            {
                    Value = value,
                    Position = 0,
                    FillColor = ScottPlot.Palette.Category10.GetColor(0),
                    Label = userCom.Competence1,
                    LineWidth = 2,
            };
            bars.Add(bar0);

            if (userCom.Competence2 == "не определен")
            {
                value = 0;
            }
            else if (userCom.Competence2 == "низкий")
            {
                value = 1;
            }
            else if (userCom.Competence2 == "средний")
            {
                value = 2;
            }
            else
            {
                value = 3;
            }

            ScottPlot.Plottable.Bar bar1 = new ScottPlot.Plottable.Bar()
            {
                Value = value,
                Position = 1,
                FillColor = ScottPlot.Palette.Category10.GetColor(1),
                Label = userCom.Competence2,
                LineWidth = 2,
            };
            bars.Add(bar1);

            if (userCom.Competence3 == "не определен")
            {
                value = 0;
            }
            else if (userCom.Competence3 == "низкий")
            {
                value = 1;
            }
            else if (userCom.Competence3 == "средний")
            {
                value = 2;
            }
            else
            {
                value = 3;
            }

            ScottPlot.Plottable.Bar bar2 = new ScottPlot.Plottable.Bar()
            {
                Value = value,
                Position = 2,
                FillColor = ScottPlot.Palette.Category10.GetColor(2),
                Label = userCom.Competence3,
                LineWidth = 2,
            };
            bars.Add(bar2);

            if (userCom.Competence4 == "не определен")
            {
                value = 0;
            }
            else if (userCom.Competence4 == "низкий")
            {
                value = 1;
            }
            else if (userCom.Competence4 == "средний")
            {
                value = 2;
            }
            else
            {
                value = 3;
            }

            ScottPlot.Plottable.Bar bar3 = new ScottPlot.Plottable.Bar()
            {
                Value = value,
                Position = 3,
                FillColor = ScottPlot.Palette.Category10.GetColor(3),
                Label = userCom.Competence4,
                LineWidth = 2,
            };
            bars.Add(bar3);
            // Add the BarSeries to the plot
            //WpfPlot1.Plot.SetAxisLimitsY(0,3);
            //WpfPlot1.Plot.Frameless();

            WpfPlot1.Plot.AddBarSeries(bars);
            WpfPlot1.Refresh();



        }



        private void Button_Click_Back(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}
