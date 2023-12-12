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
using Client.windows;
using System.Windows.Threading;

namespace Client.VM
{
    public class PageRunTestVM: INotifyPropertyChanged
    {
        private WindowRunTest myGlobal;
        public static EmployeeLogin emp { get; set; }
        
        public OneTest test = new OneTest();
        public static OneQuestion Question = new OneQuestion();

        public RelayCommand CmdEndTest { get; }
        public RelayCommand CmdNextQuestion { get; }
        public RelayCommand CmdPrevQuestion { get; }
        public RelayCommand CmdThatExecuteNothing { get; }

        private PushTest pushTest = new PushTest();

        private int _kolAnswer = 0;
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


        private OneQuestion _selectedQuestion;
        public OneQuestion SelectedQuestion
        {
            get { return _selectedQuestion; }
            set
            {
                _selectedQuestion = value;
                OnPropertyChanged(nameof(SelectedQuestion));
            }
        }

        private string _timerContent;
        public string timerContent
        {
            get { return _timerContent; }
            set
            {
                _timerContent = value;
                OnPropertyChanged(nameof(timerContent));
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

        private string _TestDiscrib;
        public string TestDiscrib
        {
            get { return _TestDiscrib; }
            set
            {
                _TestDiscrib = value;
                OnPropertyChanged(nameof(TestDiscrib));
            }
        }

        private string _TestInstruct;
        public string TestInstruct
        {
            get { return _TestInstruct; }
            set
            {
                _TestInstruct = value;
                OnPropertyChanged(nameof(TestInstruct));
            }
        }

        private int _timerTest = 600;
     

        public PageRunTestVM(string idTest, object myOwner)
        {
            myGlobal = myOwner as WindowRunTest;
            LoadData(idTest);
            this.CmdNextQuestion = new RelayCommand(FuncNextQuestion);
            this.CmdPrevQuestion = new RelayCommand(FuncPrevQuestion);
            this.CmdEndTest = new RelayCommand(FuncEndTest);
           
            this.CmdThatExecuteNothing = new RelayCommand(FuncCmdThatExecuteNothing);

            //  установка таймера

            timerContent = TimeSpan.FromSeconds(_timerTest).ToString(@"hh\:mm\:ss");
            
            DispatcherTimer timer = new DispatcherTimer();

            timer.Tick += dtTicker;

            timer.Interval = TimeSpan.FromSeconds(1);

            timer.Start();
        }

        private void dtTicker(object sender, EventArgs e) 
        {
            if (_timerTest > 0) 
            {
                _timerTest--;
                timerContent = TimeSpan.FromSeconds(_timerTest).ToString(@"hh\:mm\:ss");

            }
            

        }


        void FuncCmdThatExecuteNothing(object param)
        {
            Console.WriteLine("Exit");
        }

        public void FuncRadioCheck()
        {
            if (IndexQuestion == _kolQuestion - 1)
            {
                myGlobal.BT_next.IsEnabled = false;
                myGlobal.TestEnd.IsEnabled = true;
                if (SelectedQuestion.PushAnswersID != -1)
                {
                    myGlobal.TestEnd.Visibility = Visibility.Visible;
                }
            }
            else
            {
                if (SelectedQuestion.PushAnswersID != -1)
                {
                    myGlobal.BT_next.IsEnabled = true;
                }
            }
        }

        void FuncNextQuestion(object param)
        {
            if (IndexQuestion < _kolQuestion-1)
            {
                IndexQuestion = IndexQuestion + 1;
                if (_kolAnswer <= IndexQuestion)
                {
                    _kolAnswer = IndexQuestion;
                    myGlobal.BT_next.IsEnabled = false;
                }
                SelectedQuestion = test.Questions[IndexQuestion];
                
                myGlobal.BT_prev.IsEnabled = true;
               
            }

        }

        void FuncPrevQuestion(object param)
        {
            if (IndexQuestion > 0)
            {
                IndexQuestion = IndexQuestion - 1;
                SelectedQuestion = test.Questions[IndexQuestion];
                myGlobal.BT_next.IsEnabled = true;
            }

            if (IndexQuestion == 0)
            {
                myGlobal.BT_prev.IsEnabled = false;
            }

        }

        void FuncEndTest(object param)
        {
            // Configure the message box to be displayed
            string messageBoxText = "Вы точно хотите завершить тест?";
            string caption = "Тест: " + TestName;
            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Warning;

            // Display message box
            MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);

            // Process message box results
            switch (result)
            {
                case MessageBoxResult.Yes:

                    pushTest.EndTime = System.DateTime.Now.ToShortTimeString();
                    pushTest.Questions = new PushQuestion[_kolQuestion];
                    PushQuestion[] tmpPushQ = new PushQuestion[_kolQuestion];

                    
                    for (int i=0; i < _kolQuestion; i++)
                    {   
                        tmpPushQ[i] = new PushQuestion();
                        tmpPushQ[i].Answers = new PushAnswers[1];
                        tmpPushQ[i].Answers[0] = new PushAnswers();
                        int index_point = test.Questions[i].PushAnswersID;
                        if (index_point < 0)
                        {
                            tmpPushQ[i].Answers[0].AnswerId = 0;
                        }
                        else
                        {
                            tmpPushQ[i].Answers[0].AnswerId = test.Questions[i].Answers[index_point].IdAnswer;
                        }
                        tmpPushQ[i].QuestionId = test.Questions[i].Id;
                    }

                    pushTest.Questions = tmpPushQ;
                    string tmp_pay = string.Empty;

                    tmp_pay = JsonConvert.SerializeObject(pushTest);

                    SendData(tmp_pay);

                    myGlobal.Close();
                    MessageBox.Show("Результаты выполнения теста сохранены");

                    break;
                case MessageBoxResult.No:

                    break;
            }
        }

        public async void SendData(string pay)
        {

            ConnectHost conn = new ConnectHost();
            JToken jObject = await conn.PushTest(pay);
            string myTmp = jObject.ToString();
        }

        public async void LoadData(string idTest)
        {
            emp = AutWindow.employee;
            string tmp_pay = "{\"Id\"" + ":\"" + idTest + "\"}";

            ConnectHost conn = new ConnectHost();
            JToken jObject = await conn.GetTest(tmp_pay);
            var jsonSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            test = JsonConvert.DeserializeObject<OneTest>(jObject.ToString(), jsonSettings);

            TestInstruct = test.Instruction;
            TestDiscrib = test.Description;
            TestName = test.Name;

            pushTest.StartDate = System.DateTime.Now.ToShortDateString();
            pushTest.StartTime = System.DateTime.Now.ToShortTimeString();
            pushTest.Description = string.Empty;
            pushTest.TestId = idTest;
            pushTest.EmployeeId = emp.Id;

            int i = 0;
            kolQuestion = test.Questions.Count();
            for (i=0; i < kolQuestion; i++)
            {
                test.Questions[i].Number = i + 1;
                test.Questions[i].PushAnswersID = -1;

                if (test.Questions[i].Base64Image != null)
                {
                    test.Questions[i].Img = LoadImage(test.Questions[i].Base64Image);
                }
            }


            IndexQuestion = 0;
            SelectedQuestion = test.Questions[IndexQuestion];
           
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
