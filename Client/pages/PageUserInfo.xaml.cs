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
    /// Логика взаимодействия для PageUserInfo.xaml
    /// </summary>
    public partial class PageUserInfo : Page
    {
        public PageUserInfo()
        {
            InitializeComponent();
            DataContext = AutWindow.employee;
            subdivisionComboBox.Text = AutWindow.employee.subdivision.Name;
            DataRegUpdate.Text = AutWindow.employee.RegistrationDate.ToString();
        }
    }
}
