using System;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Threading;
using System.Windows;
using System.Net.WebSockets;
using System.Windows.Input;
using System.Windows.Media;
using Client.classDTO;
using Client.forms;
using Client.pages;
using Client.windows;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using ScottPlot.Drawing.Colormaps;

namespace Client
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ClientWebSocket websocket = new ClientWebSocket();

        public MainWindow()
        {
            InitializeComponent();

            if (ConnectHost.userRole == 2)
            {
                textBlock_UserName.Text = AutWindow.admin.LastName;
                textBlock_UserName2.Text = AutWindow.admin.FirstName + " " + AutWindow.admin.SecondName;
            }
            else if (ConnectHost.userRole == 1)
            {
                textBlock_UserName.Text = AutWindow.employee.LastName;
                textBlock_UserName2.Text = AutWindow.admin.FirstName + " " + AutWindow.admin.SecondName;
            }
            else
            {
                textBlock_UserName.Text = "Петров";
                textBlock_UserName2.Text = "Петр Петрович";
            }



            

            Loaded += MainWindow_Loaded;
            
        }

        private void ParseMessage(string inStr)
        {
            //string tmp = "{\"type\":1,\"target\":\"ReceiveMessage\",\"arguments\":[\"07.12.2023 22:50:32 Пользователь 'Михайлович Валерий Чертков завершил тест 'Склонность к риску с оценкой '0'.\"]}";
            var jsonSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            try
            {
                NotificationDTO outStr = JsonConvert.DeserializeObject<NotificationDTO>(inStr, jsonSettings);

                if ((outStr.type == 1) && (outStr.target == "TestCompleteNotification"))
                {
                    TBMess.Text = outStr.arguments[0];
                    BRMess.Visibility = Visibility.Visible;

                }
                
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        public async Task RunWebSocketClient()
        {
            ClientWebSocket websocket = new ClientWebSocket();
            string url = "wss://fitpsu.online/notification-hub";
           
            await websocket.ConnectAsync(new Uri(url), CancellationToken.None);
            string message = "{\"protocol\":\"json\",\"version\":1}\u001e";
          
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            
            await websocket.SendAsync(new ArraySegment<byte>(messageBytes), WebSocketMessageType.Text, true, CancellationToken.None);
            
            byte[] incomingData = new byte[1024];
            while (true)
            {
                WebSocketReceiveResult result = await websocket.ReceiveAsync(new ArraySegment<byte>(incomingData), CancellationToken.None);
                if (result.CloseStatus.HasValue)
                {
                    return;
                }
                else
                {
                    string tmp = Encoding.UTF8.GetString(incomingData, 0, result.Count-1);
                    ParseMessage(tmp);
                }
            }
        }



        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            GlobalRes global = new GlobalRes();
            RunWebSocketClient();
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

            if (adminFrame != null)
            {
                adminFrame.Navigate(new PageUsers());
            }
        }

        private void item1_Selected(object sender, RoutedEventArgs e)
        {
            adminFrame.Navigate(new PageUserReg());
        }

        private void item2_Selected(object sender, RoutedEventArgs e)
        {
            adminFrame.Navigate(new PageSubdivision());
        }

        private async void item3_Selected(object sender, RoutedEventArgs e)
        {
            adminFrame.Navigate(new PageCompetence());
        // отправка сообщения
        //       await connection.InvokeAsync("Send", "{\"protocol\":\"json\",\"version\":1}\u001e");

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

        private void ListViewItem_LostMouseCapture(object sender, MouseEventArgs e)
        {
             ListSetting.SelectedIndex = -1;
        }

        //private void About(object sender, RoutedEventArgs e)
        //{
        //    ListSetting.SelectedIndex = -1;
        //    WindowAbout windowAbout = new WindowAbout();
        //    // Показываем новое окно
        //    windowAbout.Show();
        //}

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

        private void itemMessage_Selected(object sender, RoutedEventArgs e)
        {
            adminFrame.Navigate(new PageAdminMass());
        }

        private void itemLogs_Selected(object sender, RoutedEventArgs e)
        {

            adminFrame.Navigate(new PageLogs());
        }

        private void Button_Click_Read(object sender, RoutedEventArgs e)
        {
            TBMess.Text = "";
            BRMess.Visibility = Visibility.Collapsed;
        }

        private void CloseQuestion(object sender, RoutedEventArgs e)
        {
            TBMess.Text = "";
            BRMess.Visibility = Visibility.Collapsed;
        }

        private void About(object sender, MouseButtonEventArgs e)
        {
            ListSetting.SelectedIndex = -1;
            WindowAbout windowAbout = new WindowAbout();
            // Показываем новое окно
            windowAbout.Show();
        }

        private void Setting(object sender, MouseButtonEventArgs e)
        {
            ListSetting.SelectedIndex = -1;
            this.Hide(); // Скрываем нынешнее окно

            // Создаем объект на основе определенного окан
            SettingConnHost windowSetting = new SettingConnHost(this);
            // Показываем новое окно
            windowSetting.Show();
        }

        private void itemAnalyse_Selected(object sender, RoutedEventArgs e)
        {
            adminFrame.Navigate(new PageAnalyseResult());
        }
    }




}
