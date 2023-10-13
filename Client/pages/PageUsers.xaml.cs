using Client.pages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

namespace Client.forms
{
    /// <summary>
    /// Логика взаимодействия для PageUsers.xaml
    /// </summary>
    public partial class PageUsers : Page
    {
        

        public PageUsers()
        {
            InitializeComponent();
           
            Loaded += PageUsers_Loaded;
        }
              



        private void PageUsers_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = new VM.UserVM(this);

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            //VM.UserVM.UserEmployee emp = new VM.UserVM.UserEmployee();
            //emp = DGuser.SelectedItem as VM.UserVM.UserEmployee;
            //this.NavigationService.Navigate(new PageAddPurpose(emp.employee));



        }
    }
}
