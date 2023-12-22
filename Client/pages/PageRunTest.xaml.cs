using Client.classDTO;
using Client.VM;
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
using static System.Net.Mime.MediaTypeNames;

namespace Client.pages
{

    public partial class PageRunTest : Page
    {
        private string ID_TEST { get; set; }

        public PageRunTest(string idTest)
        {
            InitializeComponent();

            ID_TEST = idTest;
            Loaded += PageRunTest_Loaded;
           
        }

        private void PageRunTest_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = new PageRunTestVM(ID_TEST, this, 0);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

              // this.NavigationService.Navigate(new PageUserHome());

        }


    }
}
