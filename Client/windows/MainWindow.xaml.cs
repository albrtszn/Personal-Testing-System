using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;

using System.Windows.Input;
using System.Windows.Media;
using Client.classDTO;
using Client.forms;
using Client.pages;
using Client.windows;

namespace Client
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            if (ConnectHost.userRole == 2)
            {
                textBlock_UserName.Text = AutWindow.admin.LastName + " " + AutWindow.admin.FirstName + " " + AutWindow.admin.SecondName;
            }
            else if (ConnectHost.userRole == 1)
            {
                textBlock_UserName.Text = AutWindow.employee.LastName + " " + AutWindow.employee.FirstName + " " + AutWindow.employee.SecondName;
            }
            else
            {
                textBlock_UserName.Text = "Петров Петр Петрович";
            }

            Loaded += MainWindow_Loaded;
            
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            GlobalRes global = new GlobalRes(); 
       
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

        private void item0_Selected(object sender, RoutedEventArgs e)
        {
         
            adminFrame.Navigate(new PageUsers());
        }

        private void item1_Selected(object sender, RoutedEventArgs e)
        {
            adminFrame.Navigate(new PageUserReg());
        }

        private void item2_Selected(object sender, RoutedEventArgs e)
        {
            adminFrame.Navigate(new PageSubdivision());
        }

        private void item3_Selected(object sender, RoutedEventArgs e)
        {
            adminFrame.Navigate(new PageCompetence());
        }

        private void itemTests_Selected(object sender, RoutedEventArgs e)
        {
            adminFrame.Navigate(new PageTests());
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

        private void itemResults_Selected(object sender, RoutedEventArgs e)
        {
            adminFrame.Navigate(new PageChoiceUser());
        }

        private void itemSetting_Selected(object sender, RoutedEventArgs e)
        {

        }

          private void ListViewItem_Selected_Exit(object sender, RoutedEventArgs e)
        {
            this.Hide();
            AutWindow autWindow = new AutWindow();
            autWindow.Show();
            this.Close();
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

        private void ListViewItem_LostMouseCapture(object sender, MouseEventArgs e)
        {
             ListSetting.SelectedIndex = -1;
        }

        private void About(object sender, RoutedEventArgs e)
        {
            ListSetting.SelectedIndex = -1;
            WindowAbout windowAbout = new WindowAbout();
            // Показываем новое окно
            windowAbout.Show();
        }
    }




}
