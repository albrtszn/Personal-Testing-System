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

namespace Client
{
    /// <summary>
    /// Логика взаимодействия для UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        public UserWindow()
        {
            InitializeComponent();


            textBlock_UserName.Text = AutWindow.employee.LastName + " " + AutWindow.employee.FirstName + " " + AutWindow.employee.SecondName;


        }

        private void CloseApp(object sender, RoutedEventArgs e)
        {
            try
            {
                Application.Current.Shutdown();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool isMaximazed = false;

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (isMaximazed)
                {
                    this.WindowState = WindowState.Normal;
                    this.Width = 1280;
                    this.Height = 768;
                    isMaximazed = false;
                }
                else
                {
                    this.WindowState = WindowState.Maximized;

                    isMaximazed = true;
                }
            }

        }

    }
}
