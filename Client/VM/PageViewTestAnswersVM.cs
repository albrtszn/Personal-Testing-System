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
using System.IO;
using System.Windows.Media.Imaging;
using static Client.AutWindow;

namespace Client.VM
{
    public class PageViewTestAnswersVM : INotifyPropertyChanged
    {
        private PageViewTestAnswers myGlobal;
        private AnswersUserTest ans;

        public static EmployeeDto emp { get; set; }

        public OneTest test = new OneTest();
        public static OneQuestion Question = new OneQuestion();

        public RelayCommand CmdEndTest { get; }
        public RelayCommand CmdNextQuestion { get; }
        public RelayCommand CmdPrevQuestion { get; }

        private int _kolQuestion;
        public int kolQuestion
        {
            get { return _kolQuestion; }
            set
            {
                _kolQuestion = value;
                OnPropertyChanged(nameof(kolQuestion));
            }
        }

        private int _indexQuestion;
        public int IndexQuestion
        {
            get { return _indexQuestion; }
            set
            {
                _indexQuestion = value;
                OnPropertyChanged(nameof(IndexQuestion));
            }
        }


        private AnswersQuestion _selectedQuestion;
        public AnswersQuestion SelectedQuestion
        {
            get { return _selectedQuestion; }
            set
            {
                _selectedQuestion = value;
                OnPropertyChanged(nameof(SelectedQuestion));
            }
        }

        private string _TestName;
        public string TestName
        {
            get { return _TestName; }
            set
            {
                _TestName = value;
                OnPropertyChanged(nameof(TestName));
            }
        }

        private string _TestBeginTime;
        public string TestBeginTime
        {
            get { return _TestBeginTime; }
            set
            {
                _TestBeginTime = value;
                OnPropertyChanged(nameof(TestBeginTime));
            }
        }

        private string _TestEndTime;
        public string TestEndTime
        {
            get { return _TestEndTime; }
            set
            {
                _TestEndTime = value;
                OnPropertyChanged(nameof(TestEndTime));
            }
        }

        private string _TestDate;
        public string TestDate
        {
            get { return _TestDate; }
            set
            {
                _TestDate = value;
                OnPropertyChanged(nameof(TestDate));
            }
        }

        private string _FIO;
        public string FIO
        {
            get { return _FIO; }
            set
            {
                _FIO = value;
                OnPropertyChanged(nameof(FIO));
            }
        }

        public PageViewTestAnswersVM(ResultDto res, object myOwner)
        {
            myGlobal = myOwner as PageViewTestAnswers;
            
            LoadData(res);
            this.CmdNextQuestion = new RelayCommand(FuncNextQuestion);
            this.CmdPrevQuestion = new RelayCommand(FuncPrevQuestion);
            this.CmdEndTest = new RelayCommand(FuncEndTest);
        }

        void FuncNextQuestion(object param)
        {
            if (IndexQuestion < _kolQuestion - 1)
            {
                IndexQuestion = IndexQuestion + 1;
                SelectedQuestion = ans.Questions[IndexQuestion];
                myGlobal.BT_prev.IsEnabled = true;
            }

            if (IndexQuestion == _kolQuestion - 1)
            {
                myGlobal.BT_next.IsEnabled = false;
            }

        }

        void FuncPrevQuestion(object param)
        {
            if (IndexQuestion > 0)
            {
                IndexQuestion = IndexQuestion - 1;
                SelectedQuestion = ans.Questions[IndexQuestion];
                myGlobal.BT_next.IsEnabled = true;
            }

            if (IndexQuestion == 0)
            {
                myGlobal.BT_prev.IsEnabled = false;
            }

        }

        void FuncEndTest(object param)
        {
            myGlobal.NavigationService.GoBack();
        }

        public async void SendData(string pay)
        {

            ConnectHost conn = new ConnectHost();
            JToken jObject = await conn.PushTest(pay);
            string myTmp = jObject.ToString();
        }

        public async void LoadData(ResultDto res)
        {


            ConnectHost conn = new ConnectHost();

            string tmp_pay = "{\"Id\"" + ":\"" + res.Result.Id + "\"}";
            JToken jObject = await conn.GetEmployeeResultAnswers(tmp_pay);

            var jsonSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            if (jObject != null)
            {
                ans = JsonConvert.DeserializeObject<AnswersUserTest>(jObject.ToString(), jsonSettings);



                TestDate = res.Result.StartDate;
                TestBeginTime = res.Result.StartTime;
                TestEndTime = res.Result.EndTime;
                TestName = res.Result.Test.Name;
                FIO = res.Employee.LastName + " " + res.Employee.FirstName + " " + res.Employee.SecondName;

                int i = 0;
                kolQuestion = ans.Questions.Count();
                for (i = 0; i < kolQuestion; i++)
                {
                    if (ans.Questions[i].Base64Image != null)
                    {
                        ans.Questions[i].Img = LoadImage(ans.Questions[i].Base64Image);
                    }
                }

                IndexQuestion = 0;
                SelectedQuestion = ans.Questions[IndexQuestion];
            }
        }

        public BitmapImage LoadImage(string instr)
        {
            byte[] bytes = Convert.FromBase64String(instr);

            var image = new BitmapImage();
            using (var mem = new MemoryStream(bytes))
            {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze();


            return image;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
