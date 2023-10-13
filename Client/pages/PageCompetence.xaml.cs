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

namespace Client.pages
{
    /// <summary>
    /// Логика взаимодействия для PageCompetence.xaml
    /// </summary>
    public partial class PageCompetence : Page
    {
        public PageCompetence()
        {
            InitializeComponent();
            Load();
        }

        private async void Load()
        {
            ConnectHost conn = new ConnectHost();
            JToken jObject = await conn.GetCompetences();


            GlobalRes.itemsCompetence = JsonConvert.DeserializeObject<CompetenceDto[]>(jObject.ToString());
            listCompetence.ItemsSource = GlobalRes.itemsCompetence;

        }
    }
}
