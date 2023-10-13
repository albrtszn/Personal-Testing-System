using Client.classDTO;
using Client.forms;
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
    /// Логика взаимодействия для PageTests.xaml
    /// </summary>
    public partial class PageTests : Page
    {
        public PageTests()
        {
            InitializeComponent();
            Loaded += PageTests_Loaded;
            DGTests.SelectedIndex = 0;
        }

        private void PageTests_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = new VM.TestsVM();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string tmp = "{\"Id\"" + ":\"" + VM.TestsVM.tests[DGTests.SelectedIndex].Id + "\"}"; 
           
            this.NavigationService.Navigate(new PageTestOne(tmp));


        }
    }
}
