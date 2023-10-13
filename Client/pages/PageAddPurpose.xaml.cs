using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography;
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
    /// Логика взаимодействия для PageAddPurpose.xaml
    /// </summary>
    public partial class PageAddPurpose : Page
    { 
        public static EmployeeDto employee = new EmployeeDto();

        public PageAddPurpose(EmployeeDto user)
        {
            InitializeComponent();
            Loaded += PageAddPurpose_Loaded;
            employee = user;

        }

        private void PageAddPurpose_Loaded(object sender, RoutedEventArgs e)
        {
           this.DataContext = new VM.AddPurposeVM(this);
        }

    }
}
