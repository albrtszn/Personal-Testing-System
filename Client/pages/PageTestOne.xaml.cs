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
using static Client.AutWindow;
using Client.classDTO;
using System.IO;
using static Client.pages.PageAddQuestion;
using System.Runtime.CompilerServices;
using Microsoft.Win32;
using System.Windows.Markup;


namespace Client.pages
{

    public partial class PageTestOne : Page
    {
        private string IDTest = string.Empty;
        private Visibility pageLoadOk = Visibility.Visible;
        private string strDel = string.Empty;


        Myid myid = new Myid();
        

        public PageTestOne(string idTest)
        {
            IDTest = idTest;
            InitializeComponent();
            Loaded += PageTestOne_Loaded;
           
        }

        private void PageTestOne_Loaded(object sender, RoutedEventArgs e)
        {
            LoadProgress.Visibility = pageLoadOk;
            
            var jsonSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };
            myid = JsonConvert.DeserializeObject<Myid>(IDTest, jsonSettings);

            LeadTest();
        }

        private async void LeadTest()
        {
            
            OneTest test = new OneTest();  
            ConnectHost conn = new ConnectHost();
            JToken jObject = await conn.GetTest(IDTest);
            var jsonSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            if (jObject == null)
            {
                MessageBox.Show("Не удалось выгрузить тест");
                return;
            }
            test = JsonConvert.DeserializeObject<OneTest>(jObject.ToString(), jsonSettings);
            
            TB_Name.Text = test.Name;
            TB_Discrib.Text = test.Description;

            TB_Comp.Text = GlobalRes.GetCompetence(test.CompetenceId).Name;
            if (test.CompetenceId == 1)
            {
               BT_ADD_Q.Visibility = Visibility.Visible;
               

            }
            else
            {
                BT_ADD_Q.Visibility = Visibility.Hidden;
            }
            
            foreach (var tmpQ in test.Questions)
            {
                if (tmpQ.Base64Image != null)
                { 
                    tmpQ.Img = LoadImage(tmpQ.Base64Image);
                }
                foreach (var tmpI in tmpQ.Answers)
                {
                    if (tmpI.Base64Image != null)
                    {
                        tmpI.Img = LoadImage(tmpI.Base64Image);
                    }
                }
            }
            TestLB.ItemsSource = null;
            TestLB.ItemsSource = test.Questions;
            pageLoadOk = Visibility.Collapsed;
            LoadProgress.Visibility = pageLoadOk;
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

        private class Myid
        {
            public string Id { get; set; }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //string tmp = "{\"Id\"" + ":\"" + IDTest + "\"}";

            this.NavigationService.Navigate(new PageAddQuestion(myid.Id));
        }

        private void Button_Back(object sender, RoutedEventArgs e)
        {
            this.NavigationService?.GoBack();
        }

        private void Button_Edit(object sender, RoutedEventArgs e)
        {
            var tmp = sender as System.Windows.Controls.Button;
            var dat = tmp.DataContext as OneQuestion;
            this.NavigationService.Navigate(new PageOneQuestionEdit(dat, myid.Id));
        }

        private void Button_Delet(object sender, RoutedEventArgs e)
        {
            var tmp = sender as System.Windows.Controls.Button;
            var dat = tmp.DataContext as OneQuestion;

            TBMessage.Text = "Удалить вопрос " + dat.Number.ToString() + "?";
            BRquestion.Visibility = Visibility.Visible;

          
            strDel = "{\"Id\":\"" + dat.Id  + "\"}";
            BRquestion.Visibility = Visibility.Visible;


        }

        private void ButtonClose(object sender, RoutedEventArgs e)
        {
            BRquestion.Visibility=Visibility.Collapsed;
            strDel = "";
        }

        private async void Button_delete(object sender, RoutedEventArgs e)
        {
            ConnectHost conn = new ConnectHost();
            if (strDel != "")
            {
                BRquestion.Visibility = Visibility.Collapsed;
                JToken jObject = await conn.DeleteQuestionInTest(strDel);
                if (jObject != null)
                {
                    pageLoadOk = Visibility.Visible;
                    LoadProgress.Visibility = pageLoadOk;
                    LeadTest();
                }
            }
        }

        private async void Button_export_pdf(object sender, RoutedEventArgs e)
        {
            exportPdf_BT.IsEnabled = false;
            ConnectHost conn = new ConnectHost();
            var filePdf = await conn.GetPdfTest(IDTest);
            if (filePdf != null)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Pdf file (*.pdf)|*.pdf";
                if (saveFileDialog.ShowDialog() == true)
                {
                    File.WriteAllBytes(saveFileDialog.FileName, filePdf);
                }
            }
            exportPdf_BT.IsEnabled = true;
        }

        private async void Button_export_doc(object sender, RoutedEventArgs e)
        {
            exportDoc_BT.IsEnabled = false;
        
            ConnectHost conn = new ConnectHost();
            var filePdf = await conn.GetWordTest(IDTest);
            if (filePdf != null)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Word file (*.docx)|*.docx";
                if (saveFileDialog.ShowDialog() == true)
                {
                    //var stream = File.Create(saveFileDialog.FileName);
                    //stream.Write(filePdf, 0, filePdf.Length);
                    File.WriteAllBytes(saveFileDialog.FileName, filePdf);
                }
            }
            exportDoc_BT.IsEnabled = true;
        }
    }
}
