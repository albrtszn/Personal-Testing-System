using Client.classDTO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static Client.VM.TestsVM;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Data;
using System.ComponentModel;
using System.Security.Policy;
using Client.pages;

namespace Client.VM
{
    class AddPurposeVM : DependencyObject
    {
        private PageAddPurpose myGlobal;
        public TestView[] alltest;

        public RelayCommand CmdAddPurpose { get; }
        public RelayCommand CmdBackGo { get; }

        public class TestView
        {
            public TestDto test { get; set; }
            public string nameCompetense { get; set; }
            public int checkBox { get; set; }
            public bool active { get; set; }
            public string DatatimePurpose { get; set; }
        }

        public static EmployeeDto emp { get; set; }
        public static TestDto[] tests;
        public static PurposeDto[] purposes;

        public AddPurposeVM(object myOwner)
        {
            myGlobal = myOwner as PageAddPurpose;
            LoadData();
            this.CmdAddPurpose = new RelayCommand(FuncAddPurpose);
            this.CmdBackGo = new RelayCommand(FuncBackGo);
        }

        // Добавление теста как назваченный выбранному пользователю

        void FuncBackGo(object param)
        {
            
            myGlobal.NavigationService.GoBack();
        }

        void FuncAddPurpose(object param)
        {
            int i = 0;
           

            string tmp_payload = string.Empty;
 
            foreach (var tmp in this.alltest)
            {
                Console.WriteLine(tmp.checkBox.ToString());

                if ((tmp.checkBox == 1) && (tmp.active == false))
                {

                    tmp_payload = "{\"IdEmployee\"" + ":\"" + pages.PageAddPurpose.employee.Id + "\", \"IdTest\"" + ":\"" + tmp.test.Id + "\"}";
                    PushPurpose(tmp_payload);
                }


                i++; 
               
            }

            GlobalRes.flagUpdateEmployee = true;
            MessageBox.Show("Выбранные тесты назначены");
            myGlobal.NavigationService.GoBack();


        }

        public async void PushPurpose(string payload)
        {
            ConnectHost conn = new ConnectHost();
            JToken jObject = await conn.AddPurpose(payload);
            
        }

        public ICollectionView User
        {
            get { return (ICollectionView)GetValue(UserProperty); }
            set { SetValue(UserProperty, value); }
        }

        // Using a DependencyProperty as the backing store for User.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UserProperty =
            DependencyProperty.Register("User", typeof(ICollectionView), typeof(AddPurposeVM), new PropertyMetadata(null));



        public ICollectionView ItemsTests
        {
            get { return (ICollectionView)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Items.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("ItemsTests", typeof(ICollectionView), typeof(AddPurposeVM), new PropertyMetadata(null));

        public async void LoadData()
        {
            emp = pages.PageAddPurpose.employee;
            string tmp_pay = "{\"Id\"" + ":\"" + pages.PageAddPurpose.employee.Id + "\"}";

            ConnectHost conn = new ConnectHost();

            JToken jObject = await conn.GetTestsByEmployeeId(tmp_pay);
            tests = JsonConvert.DeserializeObject<TestDto[]>(jObject.ToString());

            jObject = await conn.GetPurposesByEmployeeId(tmp_pay);
            var jsonSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            alltest = new TestView[tests.Count()];

            if (jObject != null)
            {
                purposes = JsonConvert.DeserializeObject<PurposeDto[]>(jObject.ToString(), jsonSettings);
            }

            int i = 0;
            foreach (TestDto test in tests)
            {
                var tmp = new TestView();

                tmp.test = test;
                tmp.checkBox = 0;
                tmp.active = false;
                tmp.nameCompetense = GlobalRes.GetCompetence(test.CompetenceId).Name;
                tmp.DatatimePurpose = "Не назвачен";

                if (jObject != null)
                {
                    foreach (var tmp_j in purposes)
                    {
                        if (tmp_j.Test.Id == tmp.test.Id)
                        {
                            tmp.checkBox = 1;
                            tmp.active = true;
                            tmp.DatatimePurpose = tmp_j.DatatimePurpose;
                        }
                    }
                }

                alltest[i] = tmp;
                i++;
            }

            ItemsTests = CollectionViewSource.GetDefaultView(alltest);

        }

    }
}
