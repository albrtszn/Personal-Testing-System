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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Client.pages
{
    /// <summary>
    /// Логика взаимодействия для PageLogs.xaml
    /// </summary>
    public partial class PageLogs : Page
    {   
        private int pageIdx;
        private int AllPage;
        public PageLogs()
        {
            pageIdx = 1;
            InitializeComponent();
            
            Loaded += PageLogs_Loaded;
        }

        private void PageLogs_Loaded(object sender, RoutedEventArgs e)
        {
            Load_Data(pageIdx);
        }

        private async void Load_Data(int numPage)
        {
            LogDto[] logDto;
            ConnectHost conn = new ConnectHost();
            JToken jObject = null;
            var jsonSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };
            jObject = await conn.GetLogsPage(numPage);
            DG_Logs.ItemsSource = null;
            AllPage = (int) double.Parse(jObject["Head"]["TotalPages"].ToString());
            BT_NumEnd.Content = AllPage.ToString();
            BT_Curr.Content= numPage.ToString();
            if (numPage == 1)
            {
                BT_NumBegin.Visibility = Visibility.Collapsed;
                BT_NumPrev.Visibility = Visibility.Collapsed;
            }
            else
            {
                BT_NumBegin.Visibility = Visibility.Visible;
                BT_NumPrev.Visibility = Visibility.Visible;
            }

            if (numPage == 2)
            {
                BT_NumBegin.Visibility = Visibility.Visible;
                BT_NumPrev.Visibility = Visibility.Collapsed;
            }



            if (numPage == AllPage)
            {
                BT_NumEnd.Visibility = Visibility.Collapsed;
                BT_NumNext.Visibility = Visibility.Collapsed;
            }
            else
            {
                BT_NumEnd.Visibility = Visibility.Visible;
                BT_NumNext.Visibility = Visibility.Visible;
            }

            if (numPage == AllPage-1)
            {
                BT_NumEnd.Visibility = Visibility.Visible;
                BT_NumNext.Visibility = Visibility.Collapsed;
            }


            var tmp2 = jObject["Data"];

            logDto = JsonConvert.DeserializeObject<LogDto[]>(tmp2.ToString(), jsonSettings);
            DG_Logs.ItemsSource = logDto;
        }

        private void NumBegin(object sender, RoutedEventArgs e)
        {
            pageIdx = 1;
            Load_Data(pageIdx);
        }

        private void NumPrev(object sender, RoutedEventArgs e)
        {
            if (pageIdx > 1)
            {
                pageIdx = pageIdx - 1;
            }
            Load_Data(pageIdx);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void NumNext(object sender, RoutedEventArgs e)
        {
            if (pageIdx < AllPage)
            {
                pageIdx = pageIdx + 1;
            }
            Load_Data(pageIdx);
        }

        private void NumEnd(object sender, RoutedEventArgs e)
        {
            pageIdx = AllPage;
            Load_Data(pageIdx);
        }
    }
}
