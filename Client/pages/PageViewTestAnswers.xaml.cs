using Client.classDTO;
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
    /// Логика взаимодействия для PageViewTestAnswers.xaml
    /// </summary>
    public partial class PageViewTestAnswers : Page
    {
        private ResultDto demoRes;
        public PageViewTestAnswers(ResultDto res)
        {
            demoRes = res;
            InitializeComponent();
            Loaded += PageViewTestAnswers_Loaded;
        }

        private void PageViewTestAnswers_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = new PageViewTestAnswersVM(demoRes,this);
        }
    }
}
