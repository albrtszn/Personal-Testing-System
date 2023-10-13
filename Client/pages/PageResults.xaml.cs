using Client.VM;
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
    /// Логика взаимодействия для PageResults.xaml
    /// </summary>
    public partial class PageResults : Page
    {
        public PageResults()
        {
            InitializeComponent();
            Loaded += PageResults_Loaded;
        }

        private void PageResults_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = new PageResultsVM(this);
        }
    }
}
