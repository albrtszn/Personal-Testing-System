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


namespace Client.pages
{
    /// <summary>
    /// Логика взаимодействия для PageTestOne.xaml
    /// </summary>
    public partial class PageTestOne : Page
    {
        private string IDTest = string.Empty;

        public PageTestOne(string idTest)
        {
            InitializeComponent();
            LeadTest(idTest);
        }

        private async void LeadTest(string idTest)
        {
            IDTest = idTest;
            OneTest test = new OneTest();  
            ConnectHost conn = new ConnectHost();
            JToken jObject = await conn.GetTest(idTest);
            var jsonSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };
            test = JsonConvert.DeserializeObject<OneTest>(jObject.ToString(), jsonSettings);
            TB_Name.Text = test.Name;
            TB_Discrib.Text = test.Description;
            TB_Instruct.Text = test.Instruction;
            TB_Comp.Text = test.CompetenceId.ToString();
            
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
                
            TestLB.ItemsSource = test.Questions;
            
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string tmp = "{\"Id\"" + ":\"" + IDTest + "\"}";
            
            this.NavigationService.Navigate(new PageAddQuestion(tmp));
        }


    }
}
