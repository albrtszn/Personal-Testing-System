using Client.classDTO;
using Client.forms;
using Client.pages;
using Client.windows;
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


            textBlock_UserName1.Text = AutWindow.employee.LastName;
            textBlock_UserName2.Text = AutWindow.employee.FirstName + " " + AutWindow.employee.SecondName;
            textBlock_UserName3.Text = AutWindow.employee.subdivision.Name;
                

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

        private void ListView_LostFocus(object sender, RoutedEventArgs e)
        {
            ListSetting.SelectedIndex = -1;
        }

        private void Setting(object sender, RoutedEventArgs e)
        {
            ListSetting.SelectedIndex = -1;
            this.Hide(); // Скрываем нынешнее окно

            // Создаем объект на основе определенного окан
            SettingConnHost windowSetting = new SettingConnHost(this);
            // Показываем новое окно
            windowSetting.Show();

        }

        private void ListViewItem_Selected_Exit(object sender, RoutedEventArgs e)
        {
            this.Hide();
            AutWindow autWindow = new AutWindow();
            autWindow.Show();
            this.Close();
        }

        private void ListViewItem_LostMouseCapture(object sender, MouseEventArgs e)
        {
            ListSetting.SelectedIndex = -1;
        }

        private void WindowMaximizeApp(object sender, RoutedEventArgs e)
        {

            if (isMaximazed)
            {
                this.WindowState = WindowState.Normal;
                this.Width = 1440;
                this.Height = 900;
                isMaximazed = false;
            }
            else
            {
                this.WindowState = WindowState.Maximized;

                isMaximazed = true;
            }

        }

        private void WindowMinimizeApp(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void item0_Selected(object sender, RoutedEventArgs e)
        {
            if (FrameUserData != null)
            {
                FrameUserData.Navigate(new PageUserHome());
            }
        }

        private void item1_Selected(object sender, RoutedEventArgs e)
        {
            FrameUserData.Navigate(new PageUserInfo());
        }

        private void selecAddMass(object sender, RoutedEventArgs e)
        {
            FrameUserData.Navigate(new PageUserMass());
        }

        private void About(object sender, MouseButtonEventArgs e)
        {
            ListSetting.SelectedIndex = -1;
            WindowAbout windowAbout = new WindowAbout();
            // Показываем новое окно
            windowAbout.Show();
        }
    }
}
