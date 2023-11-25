using ScottPlot;
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
using System.Windows.Shapes;

namespace Client.windows
{
    /// <summary>
    /// Логика взаимодействия для WindowAbout.xaml
    /// </summary>
    public partial class WindowAbout : Window
    {
        public WindowAbout()
        {
            InitializeComponent();
            VersionTB.Content = "Версия: " + System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString();

        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
