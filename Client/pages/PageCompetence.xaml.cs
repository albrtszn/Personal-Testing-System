using Client.classDTO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Client.VM;

namespace Client.pages
{
    /// <summary>
    /// Логика взаимодействия для PageCompetence.xaml
    /// </summary>
    public partial class PageCompetence : Page
    {
        public RelayCommand CommandTest { get; }

        public PageCompetence()
        {
            InitializeComponent();
            Loaded += PageCompetence_Loaded;
            this.CommandTest = new RelayCommand(FuncCommandTest);
            DataContext = this;
        }

        void FuncCommandTest(object param)
        {
            string tmp = "{\"Id\": \"" + (param as string) + "\"}";
            this.NavigationService.Navigate(new PageTestOne(tmp));

        }

        private void PageCompetence_Loaded(object sender, RoutedEventArgs e)
        {
            Load();
        }

        private async void Load()
        {
            ConnectHost conn = new ConnectHost();
            JToken jObject = await conn.GetCompetences();


            GlobalRes.itemsCompetence = JsonConvert.DeserializeObject<CompetenceDto[]>(jObject.ToString());
            //listCompetence.ItemsSource = GlobalRes.itemsCompetence;

        }

        private void sel_profile(object sender, SelectionChangedEventArgs e)
        {
            if ((MartixMex != null) && (MartixTex != null))
            {
                if (profileCB.SelectedIndex == 0)
                {
                    MartixMex.Visibility = Visibility.Visible;
                    MartixTex.Visibility = Visibility.Collapsed;
                }
                else
                {
                    MartixMex.Visibility = Visibility.Collapsed;
                    MartixTex.Visibility = Visibility.Visible;
                }
            }
        }

    }
}
