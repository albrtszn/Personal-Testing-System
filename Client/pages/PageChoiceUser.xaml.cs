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
    /// Логика взаимодействия для PageChoiceUser.xaml
    /// </summary>
    public partial class PageChoiceUser : Page
    {
        public PageChoiceUser()
        {
            InitializeComponent();
            Loaded += PageChoiceUser_Loaded;
        }

        private void PageChoiceUser_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = new VM.PageChoiceUserVM(this);
        }


    }
}
