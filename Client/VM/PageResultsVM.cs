using Client.classDTO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Data;
using static Client.VM.UserVM;
using static Client.AutWindow;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography;
using Client.pages;
using System.Runtime.CompilerServices;
using System.Net;
using System.Xml.Linq;

namespace Client.VM
{

    public class PageResultsVM : DependencyObject
    {
        private PageResults myGlobal;
        private string SelectedIdUser = string.Empty;

        public RelayCommand CmdBackGO { get; }
        public RelayCommand CmdViewTestAnswers { get; }



        public class ResultView
        {
            public ResultDto result { get; set; }
            public string prof { get; set; }
            public string level { get; set; }
            public int points { get; set; }
            public string GroupeSubName { get; set; }
            public int IdCompetenc { get; set; }
            public int QuestionsCount { get; set; }
            public int pointerLever { get; set; }

        }

        public PageResultsVM(object myOwner, string IdUser)
        {
            myGlobal = myOwner as PageResults;
            SelectedIdUser = IdUser;
            this.CmdBackGO = new RelayCommand(FuncBackGO);
            this.CmdViewTestAnswers = new RelayCommand(FuncViewTestAnswers);
            LoadData();
        }

        void FuncBackGO(object param)
        {
            myGlobal.NavigationService.GoBack();
        }

        void FuncViewTestAnswers(object param)
        {
            ResultView result = param as ResultView;
            myGlobal.NavigationService.Navigate(new PageViewTestAnswers(result.result));
        }

        public System.Windows.Visibility vis
        {
            get { return (System.Windows.Visibility)GetValue(visProperty); }
            set { SetValue(visProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty visProperty =
            DependencyProperty.Register("vis", typeof(System.Windows.Visibility), typeof(PageResultsVM), new PropertyMetadata(Visibility.Visible));



        public string FilterText
        {
            get { return (string)GetValue(FilterTextProperty); }
            set { SetValue(FilterTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FilterText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FilterTextProperty =
            DependencyProperty.Register("FilterText", typeof(string), typeof(PageResultsVM), new PropertyMetadata("", FilterText_Changed));

        private static void FilterText_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var curr = d as PageResultsVM;
            if (curr != null)
            {
                curr.Items.Filter = null;
                curr.Items.Filter = curr.FilterUsers;
            }
        }

        public ICollectionView Items
        {
            get { return (ICollectionView)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Items.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("Items", typeof(ICollectionView), typeof(PageResultsVM), new PropertyMetadata(null));

        public async void LoadData()
        {
            vis = Visibility.Visible;

            ResultDto[] res;
            AnswersUserTest ans;

            ConnectHost conn = new ConnectHost();

            myGlobal.BTBackGo.IsEnabled = false;
            //JToken jObject = await conn.GetResults();
            string tmp_emp = "{\"Id\"" + ":\"" + SelectedIdUser + "\"}";
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

            myGlobal.LLastName.Content = res[0].Employee.LastName;
            myGlobal.LFirstName.Content = res[0].Employee.FirstName;
            myGlobal.LSecondName.Content = res[0].Employee.SecondName;
            myGlobal.LSub.Content = res[0].Employee.Subdivision.Name;
            myGlobal.LProfile.Content = GlobalRes.GetSubdivision(res[0].Employee.Subdivision.Id).Profile;

            ResultView[] resultViews = new ResultView[res.Count()];



            int i = 0;
            //int points = 0;
            foreach (var item in res)
            {
                resultViews[i] = new ResultView();
                resultViews[i].result = new ResultDto();
                resultViews[i].result = item;
                resultViews[i].pointerLever = (int)((item.NumberPoints * 100.0) / 3);
                if (resultViews[i].pointerLever > 100)
                {
                    resultViews[i].pointerLever = 100;
                    
                }
                resultViews[i].level = GetLevelString(item.NumberPoints);
                
                i++;
            }


            Items = CollectionViewSource.GetDefaultView(resultViews);
            myGlobal.BTBackGo.IsEnabled = true;
            vis = Visibility.Hidden;

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

        private bool FilterUsers(object obj)
        {
            bool result = true;
            ResultView curr = obj as ResultView;
            if (!string.IsNullOrEmpty(FilterText) && (curr != null) && !(curr.result.Employee.FirstName.Contains(FilterText)) && !(curr.result.Employee.LastName.Contains(FilterText)))
            {
                result = false;

            }
            return result;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
