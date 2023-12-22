using Client.classDTO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Data;
using System.ComponentModel;
using Client.pages;
using System.Security.Policy;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Runtime.InteropServices.ComTypes;
using Client.windows;
using ScottPlot.Drawing.Colormaps;
using static Client.SettingConnHost;

namespace Client.VM
{
    class PageUserHomeVM : DependencyObject
    {
        private PageUserHome myGlobal; 
        public TestView[] alltest;
        public static EmployeeLogin emp { get; set; }
        public static PurposeDto[] tests;
        public ResultDto[] resTest;

        public RelayCommand CmdRunTest { get; }


        public class TestView
        {
            public TestDto test { get; set; }
            public string DatatimePurpose { get; set; }
            public bool Active { get; set; }
            public string FlagEnd { get; set; }
            public string Timer { get; set; }
        }

        private TestView selectedTest;
        public TestView SelectedTest
        {
            get { return selectedTest; }
            set
            {
                selectedTest = value;
                OnPropertyChanged("SelectedTest");
            }
        }

        public PageUserHomeVM(Object myOwner)
        {
            myGlobal = myOwner as PageUserHome;
            LoadData();
            this.CmdRunTest = new RelayCommand(FuncRunTest);

        }


        // Добавление теста как назваченный выбранному пользователю
        void FuncRunTest(object param)
        {
            if (SelectedTest == null)
            {
                return;
            }
            int i = 0;
            string tmp_payload = string.Empty;

            
            string messageBoxText = "Вы собираетесь пройти тест: " + SelectedTest.test.Name;
            string caption = "Word Processor";
            MessageBoxButton button = MessageBoxButton.YesNoCancel;
            MessageBoxImage icon = MessageBoxImage.Warning;

            // Display message box
            MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);

            // Process message box results
            switch (result)
            {
                case MessageBoxResult.Yes:

                    if (SelectedTest.Active == false)
                    {
                        MessageBox.Show("Выбранный тест уже пройден");
                        break;
                    }

                    //myGlobal.NavigationService.Navigate(new PageRunTest(SelectedTest.test.Id));

                    int tmpTimer = 0;
                    if (!SelectedTest.Timer.Contains("не ограничен"))
                    {
                        tmpTimer = (int)double.Parse(SelectedTest.Timer);
                    }


                    WindowRunTest windowRunTest = new WindowRunTest(SelectedTest.test.Id, tmpTimer*60);
                    windowRunTest.Show();
                    windowRunTest.Closed += WindowRunTest_Closed;
                    break;
                case MessageBoxResult.No:
                    // User pressed No button
                    // ...
                    break;
                case MessageBoxResult.Cancel:
                    // User pressed Cancel button
                    // ...
                    break;
            }


        }

        private void WindowRunTest_Closed(object sender, EventArgs e)
        {

            SelectedTest.FlagEnd = "Завершен (" + System.DateTime.Now.ToShortTimeString() + ")";
            SelectedTest.Active = false;
            
            ItemsTests.Refresh();
        }

        public ICollectionView ItemsTests
        {
            get { return (ICollectionView)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Items.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("ItemsTests", typeof(ICollectionView), typeof(PageUserHomeVM), new PropertyMetadata(null));

        private MySetting[] inSetting;

        public async void LoadData()
        {
            emp = AutWindow.employee;
            string tmp_pay = "{\"Id\"" + ":\"" + emp.Id + "\"}";
            ConnectHost conn = new ConnectHost();

            

            JToken jObject = await conn.GetGlobalConfigures();
            var jsonSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            if (jObject == null)
            {
                MessageBox.Show("Ошибка загрузки глобальной конфигурации!");
                return;
            }
            inSetting = JsonConvert.DeserializeObject<MySetting[]>(jObject.ToString(), jsonSettings);

            jObject = null;
            jObject = await conn.GetPurposesByEmployeeId(tmp_pay);

            if (jObject != null) 
            {

                tests = JsonConvert.DeserializeObject<PurposeDto[]>(jObject.ToString(), jsonSettings);
                alltest = new TestView[tests.Count()];
               
                jObject = await conn.GetTestResultsByEmployee(tmp_pay);
                if (jObject != null)
                {
                    resTest = JsonConvert.DeserializeObject<ResultDto[]>(jObject.ToString(), jsonSettings);
                }


                int i = 0;
                foreach (PurposeDto test in tests)
                {
                    var tmp = new TestView();

                    tmp.test = test.Test;

                    tmp.FlagEnd = "не завершен";
                    tmp.Active = true;

                    if (resTest != null)
                    { 
                        foreach (ResultDto item in resTest)
                        {
                            if (item.Result.Test.Id == tmp.test.Id)
                            {
                                tmp.FlagEnd = "Завершен (" + item.Result.StartDate + " "+ item.Result.EndTime + ")";
                                tmp.Active = false;
                            }


                        }
                    }

                    tmp.DatatimePurpose = test.DatatimePurpose;
                    if ((test.Timer == 0) || (inSetting[0].TestingTimeLimit == false))
                    {
                        tmp.Timer = "не ограничен";
                    }
                    else
                    {
                        tmp.Timer = test.Timer.ToString();
                    }
                    
                    alltest[i] = tmp;
                    i++;
                }

                ItemsTests = CollectionViewSource.GetDefaultView(alltest);
                SelectedTest = alltest[0];
            }



        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
