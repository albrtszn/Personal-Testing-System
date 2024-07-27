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
            public int Weight { get; set; }
            public Answersss[] Answers { get; set; }
        }

        private string id_test = string.Empty;
        private BitmapImage imageQ = null;
        private BitmapImage image1 = null;
        private BitmapImage image2 = null;
        private BitmapImage image3 = null;
        private BitmapImage image4 = null;

        private List<Client.ConnectHost.Payload_files> payload_Files = new List<ConnectHost.Payload_files>();


        public PageAddQuestion(string ID_test)
        {
            InitializeComponent();
            curID = ID_test;
            payload_Files.Clear();
        }

        private async void Button_Click_final(object sender, RoutedEventArgs e)
        {
            Question ans = new Question();
            addQFinal.IsEnabled = false;
            if (addTextQ.Text == null || addTextQ.Text == "" || addTextQ.Text.Length < 5)
            {
                addTextQ.Background = Brushes.Pink;
                MessageBox.Show("Поле текста вопроса заполнено неверно!");
                addQFinal.IsEnabled = true;
                return;
            }
            else
            {
                addTextQ.Background = Brushes.Transparent;
                ans.Text = addTextQ.Text;
            }
            
            ans.IdTest = curID;
            ans.IdQuestionType = 1;
            ans.ImagePath = addImgNameQ.Text;
            ans.Weight = 1;
            ans.Answers = new Answersss[addKolAnswer.SelectedIndex+1];



            // Answer 1
            ans.Answers[0] = new Answersss();

            if (addTextA1.Text == null || addTextA1.Text == "" || addTextA1.Text.Length < 5)
            {
                addTextA1.Background = Brushes.Pink;
                MessageBox.Show("Поле текста ответа заполнено неверно!");
                addQFinal.IsEnabled = true;
                return;
            }
            else
            {
                addTextA1.Background = Brushes.Transparent;
                ans.Answers[0].Text = addTextA1.Text;
            }
           
            ans.Answers[0].ImagePath = addImgNameA1.Text;
            
            float tmpFloat = 0;
            if (float.TryParse(addBallA1.Text, out tmpFloat))
            {
                ans.Answers[0].Weight = tmpFloat;
                addBallA1.Background = Brushes.Transparent; 
            }
            else
            {
                addBallA1.Background = Brushes.Pink;
                MessageBox.Show("Поле весовой коэффициент заволнено неверно");
                addQFinal.IsEnabled = true;
                return ;
            }
            
            

            if (addTrueA1.SelectedIndex == 0)
            {
                ans.Answers[0].Correct = false;
            }
            else
            {
                ans.Answers[0].Correct = true;
            }


            // Answer 2
            ans.Answers[1] = new Answersss();
            if (addTextA2.Text == null || addTextA2.Text == "" || addTextA2.Text.Length < 5)
            {
                addTextA2.Background = Brushes.Pink;
                MessageBox.Show("Поле текста вопроса заполнено неверно!");
                addQFinal.IsEnabled = true;
                return;
            }
            else
            {
                addTextA2.Background = Brushes.Transparent;
                ans.Answers[1].Text = addTextA2.Text;
            }

            ans.Answers[1].ImagePath = addImgNameA2.Text;

            tmpFloat = 0;
            if (float.TryParse(addBallA2.Text, out tmpFloat))
            {
                ans.Answers[1].Weight = tmpFloat;
                addBallA2.Background = Brushes.Transparent;
            }
            else
            {
                addBallA2.Background = Brushes.Pink;
                MessageBox.Show("Поле весовой коэффициент заволнено неверно");
                addQFinal.IsEnabled = true;
                return;
            }

            
            if (addTrueA2.SelectedIndex == 0)
            {
                ans.Answers[1].Correct = false;
            }
            else
            {
                ans.Answers[1].Correct = true;
            }


            // Answer 3
            ans.Answers[2] = new Answersss();
            if (addTextA3.Text == null || addTextA3.Text == "" || addTextA3.Text.Length < 5)
            {
                addTextA3.Background = Brushes.Pink;
                MessageBox.Show("Поле текста вопроса заполнено неверно!");
                addQFinal.IsEnabled = true;
                return;
            }
            else
            {
                addTextA3.Background = Brushes.Transparent;
                ans.Answers[2].Text = addTextA3.Text;
            }
            
            ans.Answers[2].ImagePath = addImgNameA3.Text;
            tmpFloat = 0;
            if (float.TryParse(addBallA3.Text, out tmpFloat))
            {
                ans.Answers[2].Weight = tmpFloat;
                addBallA3.Background = Brushes.Transparent;
            }
            else
            {
                addBallA3.Background = Brushes.Pink;
                MessageBox.Show("Поле весовой коэффициент заволнено неверно");
                addQFinal.IsEnabled = true;
                return;
            }
          
            if (addTrueA3.SelectedIndex == 0)
            {
                ans.Answers[2].Correct = false;
            }
            else
            {
                ans.Answers[2].Correct = true;
            }

            // Answer 4
            ans.Answers[3] = new Answersss();
            if (addTextA4.Text == null || addTextA4.Text == "" || addTextA4.Text.Length < 5)
            {
                addTextA4.Background = Brushes.Pink;
                MessageBox.Show("Поле текста вопроса заполнено неверно!");
                addQFinal.IsEnabled = true;
                return;
            }
            else
            {
                addTextA4.Background = Brushes.Transparent;
                ans.Answers[3].Text = addTextA4.Text;
            }
            
            ans.Answers[3].ImagePath = addImgNameA4.Text;
            tmpFloat = 0;
            if (float.TryParse(addBallA4.Text, out tmpFloat))
            {
                ans.Answers[3].Weight = tmpFloat;
                addBallA4.Background = Brushes.Transparent;
            }
            else
            {
                addBallA4.Background = Brushes.Pink;
                MessageBox.Show("Поле весовой коэффициент заволнено неверно");
                addQFinal.IsEnabled = true;
                return;
            }

            if (addTrueA4.SelectedIndex == 0)
            {
                ans.Answers[3].Correct = false;
            }
            else
            {
                ans.Answers[3].Correct = true;
            }
          

            string jout = JsonConvert.SerializeObject(ans);
            ConnectHost conn = new ConnectHost();
            JToken jObject = null;
            if (payload_Files.Count > 0)
            {
                jObject = await conn.AddQuestionInTest(jout, payload_Files);
            }
            else
            {
                jObject = await conn.AddQuestionInTest(jout, null);
            }

            if (jObject == null)
            {

                MessageBox.Show("Не удалось добавить вопрос!");

            }
            else
            {
                MessageBox.Show("Вопрос успешно добавлен!");
                addBallA1.Text = "";
                addBallA2.Text = "";
                addBallA3.Text = "";
                addBallA4.Text = "";
                addTextQ.Text = "";
                addTextA1.Text = "";
                addTextA2.Text = "";
                addTextA3.Text = "";
                addTextA4.Text = "";
               
                addImgNameQ.Text = "";
                addImgNameQ.Visibility = Visibility.Collapsed;
                addImg.Visibility = Visibility.Visible;
                addImgQ.Source = null;
                addImgQ.Visibility = Visibility.Collapsed;

                addImgA11.Source = null;
                addImgA11.Visibility = Visibility.Collapsed;
                addImgA1.Visibility = Visibility.Visible;
                addImgNameA1.Text = null;
                addImgNameA1.Visibility = Visibility.Collapsed;

                addImgA12.Source = null;
                addImgA12.Visibility = Visibility.Collapsed;
                addImgA2.Visibility = Visibility.Visible;
                addImgNameA2.Text = null;
                addImgNameA2.Visibility = Visibility.Collapsed;

                addImgA13.Source = null;
                addImgA13.Visibility = Visibility.Collapsed;
                addImgA3.Visibility = Visibility.Visible;
                addImgNameA3.Text = null;
                addImgNameA3.Visibility = Visibility.Collapsed;

                addImgA14.Source = null;
                addImgA14.Visibility = Visibility.Collapsed;
                addImgA4.Visibility = Visibility.Visible;
                addImgNameA4.Text = null;
                addImgNameA4.Visibility = Visibility.Collapsed;
            }

        }

        private string NameFileUuid(string name)
        {
            string str_buf = string.Empty;
            Guid myuuid = Guid.NewGuid();
            str_buf = myuuid.ToString() + '.' + name.Split('.')[1];
            return str_buf;
        }

        private void ButtonImgADD(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofdPicture = new OpenFileDialog();
            ofdPicture.Filter = "Image files|*.bmp;*.jpg;*.gif;*.png;*.tif|All files|*.*";
            ofdPicture.FilterIndex = 1;

            if (ofdPicture.ShowDialog() == true)
            {
                imageQ = new BitmapImage(new Uri(ofdPicture.FileName));


                addImgNameQ.Text = NameFileUuid(ofdPicture.SafeFileName);
              

                addImgNameQ.Visibility = Visibility.Visible;
                addImg.Visibility = Visibility.Collapsed;
                addImgQ.Source = imageQ;
                addImgQ.Visibility = Visibility.Visible;
                Client.ConnectHost.Payload_files file_buf = new Client.ConnectHost.Payload_files();
                file_buf.filePath = ofdPicture.FileName;
                file_buf.name = addImgNameQ.Text;

                payload_Files.Add(file_buf);
            }

        }

        private void ButtonImgADD_A1(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofdPicture = new OpenFileDialog();
            ofdPicture.Filter = "Image files|*.bmp;*.jpg;*.gif;*.png;*.tif|All files|*.*";
            ofdPicture.FilterIndex = 1;

            if (ofdPicture.ShowDialog() == true)
            {
                image1 = new BitmapImage(new Uri(ofdPicture.FileName));
                addImgA11.Source = image1;
                addImgA11.Visibility = Visibility.Visible;
                addImgA1.Visibility = Visibility.Collapsed;
                addImgNameA1.Text = NameFileUuid(ofdPicture.SafeFileName);
                addImgNameA1.Visibility = Visibility.Visible;

                Client.ConnectHost.Payload_files file_buf = new Client.ConnectHost.Payload_files();
                file_buf.filePath = ofdPicture.FileName;
                file_buf.name = addImgNameA1.Text;

                payload_Files.Add(file_buf);
            }
        }

        private void ButtonImgADD_A2(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofdPicture = new OpenFileDialog();
            ofdPicture.Filter = "Image files|*.bmp;*.jpg;*.gif;*.png;*.tif|All files|*.*";
            ofdPicture.FilterIndex = 1;

            if (ofdPicture.ShowDialog() == true)
            {
                image2 = new BitmapImage(new Uri(ofdPicture.FileName));
                addImgA12.Source = image2;
                addImgA12.Visibility = Visibility.Visible;
                addImgA2.Visibility = Visibility.Collapsed;
                addImgNameA2.Text = NameFileUuid(ofdPicture.SafeFileName);
                addImgNameA2.Visibility = Visibility.Visible;

                Client.ConnectHost.Payload_files file_buf = new Client.ConnectHost.Payload_files();
                file_buf.filePath = ofdPicture.FileName;
                file_buf.name = addImgNameA2.Text;

                payload_Files.Add(file_buf);
            }
        }

        private void ButtonImgADD_A3(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofdPicture = new OpenFileDialog();
            ofdPicture.Filter = "Image files|*.bmp;*.jpg;*.gif;*.png;*.tif|All files|*.*";
            ofdPicture.FilterIndex = 1;

            if (ofdPicture.ShowDialog() == true)
            {
                image3 = new BitmapImage(new Uri(ofdPicture.FileName));
                addImgA13.Source = image3;
                addImgA13.Visibility = Visibility.Visible;
                addImgA3.Visibility = Visibility.Collapsed;
                addImgNameA3.Text = NameFileUuid(ofdPicture.SafeFileName);
                addImgNameA3.Visibility = Visibility.Visible;
                
                Client.ConnectHost.Payload_files file_buf = new Client.ConnectHost.Payload_files();
                file_buf.filePath = ofdPicture.FileName;
                file_buf.name = addImgNameA3.Text;

                payload_Files.Add(file_buf);
            }
        }

        private void ButtonImgADD_A4(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofdPicture = new OpenFileDialog();
            ofdPicture.Filter = "Image files|*.bmp;*.jpg;*.gif;*.png;*.tif|All files|*.*";
            ofdPicture.FilterIndex = 1;

            if (ofdPicture.ShowDialog() == true)
            {
                image4 = new BitmapImage(new Uri(ofdPicture.FileName));
                addImgA14.Source = image4;
                addImgA14.Visibility = Visibility.Visible;
                addImgA4.Visibility = Visibility.Collapsed;
                addImgNameA4.Text = NameFileUuid(ofdPicture.SafeFileName);
                addImgNameA4.Visibility = Visibility.Visible;

                Client.ConnectHost.Payload_files file_buf = new Client.ConnectHost.Payload_files();
                file_buf.filePath = ofdPicture.FileName;
                file_buf.name = addImgNameA4.Text;

                payload_Files.Add(file_buf);

            }
        }

        private void Button_Click_Back(object sender, RoutedEventArgs e)
        {
            this.NavigationService?.GoBack();
        }
    }
}
