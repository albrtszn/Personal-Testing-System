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
    /// Логика взаимодействия для PageAdminMass.xaml
    /// </summary>
    public partial class PageAdminMass : Page
    {
        private MessageDTO[] mess; 
        
        public PageAdminMass()
        {
            InitializeComponent();
            Loaded += PageAdminMass_Loaded;
        }

        private void PageAdminMass_Loaded(object sender, RoutedEventArgs e)
        {
            Load_mess();
        }

        public async void Load_mess()
        {
            ConnectHost conn = new ConnectHost();
            JToken jObject = await conn.GetMesssages();


            mess = JsonConvert.DeserializeObject<MessageDTO[]>(jObject.ToString());
            DG_Mess.ItemsSource = mess;
        }
    }
}
