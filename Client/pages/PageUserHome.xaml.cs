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
    public partial class PageUserHome : Page
    {
        public PageUserHome()
        {
            InitializeComponent();
            Loaded += PageUserHome_Loaded;
            DGTests.SelectedIndex = 0;
        }

        private void PageUserHome_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = new VM.PageUserHomeVM(this);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //VM.PageUserHomeVM.TestView tmp1 = this.DGTests.SelectedItem as VM.PageUserHomeVM.TestView;
            //if (tmp1 != null)
            //{
            //    this.NavigationService.Navigate(new PageRunTest(tmp1.test.Id));
            //}
            
        }
    }
}
