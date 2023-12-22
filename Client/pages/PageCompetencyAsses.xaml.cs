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
using Client.VM;

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
            userCom.Subdevision = res[0].Employee.Subdivision.Name;
            userCom.Profile = GlobalRes.GetSubdivision(res[0].Employee.Subdivision.Id).Profile;
            userCom.Competence1 = "не определен";
            userCom.Competence2 = "не определен";
            userCom.Competence3 = "не определен";
            userCom.Competence4 = "не определен";


            ResultView[] resultViews = new ResultView[res.Count()];


            int i = 0;
            float points = 0;
            float sum1 = 0;
            float sum2 = 0;
            float sum3 = 0;
            float sum4 = 0;
            float sumQ = 0;
            int IdCompetenc = 0;
            int kol1 = 0;
            int kol2 = 0;
            int kol3 = 0;
            int kol4 = 0;

            foreach (var item in res)
            {

                if (item.NumberPoints == null)
                {
                    points = 0;
                }
                else
                {
                    points = item.NumberPoints;
                }
                
                IdCompetenc = item.Result.Test.CompetenceId;
               
             

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
                else
                {
                    sum4 = sum4 + points;
                    kol4++;
                }

                i++;
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
         

            userCom.Competence1 = GetLevelString(sum1);
            userCom.Competence2 = GetLevelString(sum2); 
            userCom.Competence3 = GetLevelString(sum3); 
            userCom.Competence4 = GetLevelString(sum4);

            userCom.Point1 = (int) sum1 * 100 / 3;
            userCom.Point2 = (int) sum2 * 100 / 3;
            userCom.Point3 = (int) sum3 * 100 / 3;
            userCom.Point4 = (int) sum4 * 100 / 3;

            this.DataContext = userCom;
            BT_BackGo.IsEnabled = true;

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
            this.NavigationService.GoBack();
        }

        private void Button_export_word(object sender, RoutedEventArgs e)
        {

        }
    }
}
