using Client.classDTO;
using Client.VM;
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
using System.Windows.Shapes;
using static Client.pages.PageCompetencyAsses;

namespace Client.windows
{
    public partial class UserTestResult : Window
    {
        public class ListResultCompetence
        {
            public string Name { get; set; }
            public string Point { get; set; }
            public int Level { get; set; }
            public string Description { get; set; }

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

        public UserCompetency userCom = new UserCompetency();
        public string IdUser = string.Empty;

        public UserTestResult(string Id)
        {
            InitializeComponent();
            IdUser = Id;
            Loaded += UserTestResult_Loaded;
        }

        private void UserTestResult_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        public async void LoadData()
        {
            ResultDto[] res;

            ConnectHost conn = new ConnectHost();
            

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
                }
                else
                {
                    points = purpose.NumberPoints;
                    bufList.Point = purpose.ScoreFrom.ToString();
                    bufList.Level = (int)purpose.NumberPoints * 100 / 3;
                    bufList.Description = purpose.ResultLevel;
                }

                IdCompetenc = purpose.Result.Test.CompetenceId;

                if (IdCompetenc == 1)
                {
                    listComp1.Add(bufList);
                    sum1 = sum1 + points;
                    kol1++;
                }
                else if (IdCompetenc == 2)
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

            userCom.Point1 = (int)(sum1 * 100 / 3);
            userCom.Point2 = (int)(sum2 * 100 / 3);
            userCom.Point3 = (int)(sum3 * 100 / 3);
            userCom.Point4 = (int)(sum4 * 100 / 3);

            userCom.ItogPoint = (sum1 * GlobalRes.matrixCoeff[1, IdGroup]) + (sum2 * GlobalRes.matrixCoeff[2, IdGroup]) + (sum3 * GlobalRes.matrixCoeff[3, IdGroup]) + (sum4 * GlobalRes.matrixCoeff[4, IdGroup]);
            userCom.ItogPoint = userCom.ItogPoint * 100 / 3;
            userCom.ItogPoint = Math.Round(userCom.ItogPoint, 1);
            userCom.ItogLevel = (int)userCom.ItogPoint;

            userCom.Description1 = GetDescription(1, IdGroup, sum1);
            userCom.Description2 = GetDescription(2, IdGroup, sum2);
            userCom.Description3 = GetDescription(3, IdGroup, sum3);
            userCom.Description4 = GetDescription(4, IdGroup, sum4);

            this.DataContext = userCom;
           

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
            { LEV = 3; }

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

        string GetLevelString(float level)
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

        private void Button_export_word(object sender, RoutedEventArgs e)
        {

        }
    }
}
