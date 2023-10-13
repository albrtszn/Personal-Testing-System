using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
using static Client.AutWindow;
using static Client.VM.UserVM;

namespace Client.pages
{
    /// <summary>
    /// Логика взаимодействия для PageAddQuestion.xaml
    /// </summary>
    /// 
   


    public partial class PageAddQuestion : Page
    {
        public string curID { get; set; }

        public class Answersss
	    {
            public string Text { get; set; }
            public float Weight { get; set; }
            public bool Correct { get; set; }
            public string ImagePath { get; set; }
        }

        public class Question
        {
            public string IdTest { get; set; }
            public string Text { get; set; }
            public string ImagePath { get; set; }
            public int IdQuestionType { get; set; }
            public Answersss[] Answers { get; set; }
        }

        private string id_test = string.Empty;
        private BitmapImage image1 = null;
        private BitmapImage image2 = null;
        private BitmapImage image3 = null;
        private BitmapImage image4 = null;

        public PageAddQuestion(string ID_test)
        {
            InitializeComponent();
            curID = ID_test;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            OpenFileDialog ofdPicture = new OpenFileDialog();
            ofdPicture.Filter = "Image files|*.bmp;*.jpg;*.gif;*.png;*.tif|All files|*.*";
            ofdPicture.FilterIndex = 1;

            if (ofdPicture.ShowDialog() == true)
            {
                image1 = new BitmapImage(new Uri(ofdPicture.FileName));
                QFile1.Text = ofdPicture.FileName;

            }

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

            OpenFileDialog ofdPicture = new OpenFileDialog();
            ofdPicture.Filter = "Image files|*.bmp;*.jpg;*.gif;*.png;*.tif|All files|*.*";
            ofdPicture.FilterIndex = 1;

            if (ofdPicture.ShowDialog() == true)
            {
                image2 = new BitmapImage(new Uri(ofdPicture.FileName));
                QFile2.Text = ofdPicture.FileName;

            }

        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {

            OpenFileDialog ofdPicture = new OpenFileDialog();
            ofdPicture.Filter = "Image files|*.bmp;*.jpg;*.gif;*.png;*.tif|All files|*.*";
            ofdPicture.FilterIndex = 1;

            if (ofdPicture.ShowDialog() == true)
            {
                image3 = new BitmapImage(new Uri(ofdPicture.FileName));
                QFile3.Text = ofdPicture.FileName;

            }

        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {

            OpenFileDialog ofdPicture = new OpenFileDialog();
            ofdPicture.Filter = "Image files|*.bmp;*.jpg;*.gif;*.png;*.tif|All files|*.*";
            ofdPicture.FilterIndex = 1;

            if (ofdPicture.ShowDialog() == true)
            {
                image4 = new BitmapImage(new Uri(ofdPicture.FileName));
                QFile4.Text = ofdPicture.FileName;
                

            }

        }

        private async void Button_Click_final(object sender, RoutedEventArgs e)
        {
            Question ans = new Question();
            ans.Text = QName.Text;
            ans.IdTest = curID;
            ans.IdQuestionType = 1;
            ans.ImagePath = null;
            ans.Answers = new Answersss[QKol.SelectedIndex+1];

            var tmp = new Answersss();

            tmp.Text = QAnsweText1.Text;
            tmp.Weight = 1;
            if (QAnswerCor1.SelectedIndex == 0)
            {
                tmp.Correct = true;
            }
            else
            {
                tmp.Correct = false;
            }
            ans.Answers[0] = tmp;

            ans.Answers[1] = new Answersss();
            ans.Answers[1].Text = QAnsweText2.Text;
            ans.Answers[1].Weight = 1;
            if (QAnswerCor2.SelectedIndex == 0)
            {
                ans.Answers[1].Correct = true;
            }
            else
            {
                ans.Answers[1].Correct = false;
            }

            ans.Answers[2] = new Answersss();
            ans.Answers[2].Text = QAnsweText3.Text;
            ans.Answers[2].Weight = 1;
            if (QAnswerCor3.SelectedIndex == 0)
            {
                ans.Answers[2].Correct = true;
            }
            else
            {
                ans.Answers[2].Correct = false;
            }

            ans.Answers[3] = new Answersss();
            ans.Answers[3].Text = QAnsweText4.Text;
            ans.Answers[3].Weight = 1;
            if (QAnswerCor4.SelectedIndex == 0)
            {
                ans.Answers[3].Correct = true;
            }
            else
            {
                ans.Answers[3].Correct = false;
            }

            string jout = JsonConvert.SerializeObject(ans);
            Console.WriteLine(jout);

            ConnectHost conn = new ConnectHost();


            JToken jObject = await conn.AddQuestionInTest(jout);

            if (jObject == null)
            {
                Console.WriteLine(jout);

            }

        }
    }
}
