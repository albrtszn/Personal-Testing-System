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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static Client.ConnectHost;
using static Client.pages.PageAddQuestion;
using ScottPlot.Palettes;
using System.Windows.Media.Media3D;
using static SkiaSharp.SKImageFilter;

namespace Client.pages
{
    /// <summary>
    /// Логика взаимодействия для PageOneQuestionEdit.xaml
    /// </summary>
    public partial class PageOneQuestionEdit : Page
    {
        public string curID { get; set; }
        private OneQuestion curQuestion;
        private OneQuestionEdit curQuestionEdit = new OneQuestionEdit();
        private List<OneAnswer> answerList;
        private List<AnswerDto> answerList2;
        private string IdTest = string.Empty;
        private BitmapImage imageQ = null;
        private List<Payload_files> pay_Files = new List<Payload_files>();
      

        public PageOneQuestionEdit(OneQuestion buf, string str)
        {
            InitializeComponent();
            IdTest = str;
            curQuestion = buf;
            addTextQ.Text = curQuestion.Text;
            addImgQ.Source = curQuestion.Img;
            addImgNameQ.Text = curQuestion.ImagePath;
            
            answerList = curQuestion.Answers.Select(p => p).ToList();
            addKolAnswer.SelectedIndex = answerList.Count - 1;

            AnswerLB.ItemsSource = answerList;
           
        }

        private void Button_Click_Back(object sender, RoutedEventArgs e)
        {
            this.NavigationService?.GoBack();
        }

        private async void Button_Click_final(object sender, RoutedEventArgs e)
        {
            int kol_true = 0;
            addQFinal.IsEnabled = false;
            if (addTextQ.Text == null || addTextQ.Text == "" || addTextQ.Text.Length < 5)
            {
                addTextQ.Background = Brushes.Pink;
                System.Windows.MessageBox.Show("Поле текста вопроса заполнено неверно!");
                
                return;
            }

            addTextQ.Background = Brushes.Transparent;
            curQuestion.Text = addTextQ.Text;
            curQuestion.ImagePath = addImgNameQ.Text;

            foreach (var item in answerList)
            {
                if (item.Text == "" || item.Text == null)
                {
                    System.Windows.MessageBox.Show("Поле текста ответа заполнено неверно!");
                    return;
                }

                if (item.Correct) kol_true++;
            }


            curQuestion.Answers = answerList.ToArray();
            var demo = answerList.Select<OneAnswer, AnswerDto>(p => new AnswerDto {
                    IdAnswer = p.IdAnswer,
                    Number = p.Number,
                    Text = p.Text,
                    Weight = p.Weight,
                    IdQuestion = p.IdQuestion,
                    Correct = p.Correct,
                    ImagePath = p.ImagePath
            }) ;

            curQuestionEdit.Answers = demo.ToArray();
            curQuestionEdit.IdQuestion = curQuestion.Id;
            curQuestionEdit.IdTest = IdTest;
            curQuestionEdit.Text = curQuestion.Text;
            curQuestionEdit.IdQuestionType = curQuestion.IdQuestionType;
            curQuestionEdit.ImagePath = addImgNameQ.Text;
            curQuestionEdit.Weight = 1;
            curQuestionEdit.Number = curQuestion.Number;

            string jout = JsonConvert.SerializeObject(curQuestionEdit);
            ConnectHost conn = new ConnectHost();
            JToken jObject = null;
            if (pay_Files.Count > 0)
            {
                jObject = await conn.UpdateQuestionInTest(jout, pay_Files);
            }
            else
            {
                jObject = await conn.UpdateQuestionInTest(jout, null);
            }
             
            if (jObject != null)
            {
                System.Windows.MessageBox.Show("Обновление вопроса прошло успешно!");
            }
            addQFinal.IsEnabled = true;
        }


        private void ButtonImgADD_A1(object sender, RoutedEventArgs e)
        {
            var tmp = sender as System.Windows.Controls.Button;
            var dat = tmp.DataContext as OneAnswer;
            
            OpenFileDialog ofdPicture = new OpenFileDialog();
            ofdPicture.Filter = "Image files|*.bmp;*.jpg;*.gif;*.png;*.tif|All files|*.*";
            ofdPicture.FilterIndex = 1;

            if (ofdPicture.ShowDialog() == DialogResult.OK)
            {
                var image_buf = new BitmapImage(new Uri(ofdPicture.FileName));
                var newNameFile = NameFileUuid(ofdPicture.SafeFileName);

                Client.ConnectHost.Payload_files file_buf = new Client.ConnectHost.Payload_files();
                file_buf.filePath = ofdPicture.FileName;
                file_buf.name = newNameFile;
                pay_Files.Add(file_buf);
                answerList.FindAll(s => s.IdAnswer == dat.IdAnswer)
                          .ForEach(x => { x.Img = image_buf; x.ImagePath = newNameFile; });
                AnswerLB.ItemsSource = null;
                AnswerLB.ItemsSource = answerList;
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

            if (ofdPicture.ShowDialog() == DialogResult.OK)
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

                pay_Files.Add(file_buf);
            }
        }
    }
}
