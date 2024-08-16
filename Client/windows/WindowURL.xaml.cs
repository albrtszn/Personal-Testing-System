using System.Windows;
using System.Windows.Media;

namespace Client.windows
{
    /// <summary>
    /// Логика взаимодействия для WindowURL.xaml
    /// </summary>
    public partial class WindowURL : Window
    {
        public WindowURL()
        {
            InitializeComponent();
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        public string TextUrl
        {
            get { return urlBox.Text; }
        }

        private async void Test_Click(object sender, RoutedEventArgs e)
        {
            ButtonTest.IsEnabled = false;
            ConnectHost conn = new ConnectHost();
            string output = "Ошибка соединения!";
            
            output = await conn.Ping(TextUrl);
            if (output.Contains("Ошибка соединения!"))
            {
                StatusBox.Foreground = Brushes.Red;
                ButtonOK.IsEnabled = false;
            }
            else
            {
                StatusBox.Foreground = Brushes.Green;
                ButtonOK.IsEnabled = true;
            }
            StatusBox.Content = output;
            ButtonTest.IsEnabled = true;


        }

        private void urlBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (TextUrl.Length > 0)
            {
                ButtonTest.IsEnabled = true;
            }
            else
            { ButtonTest.IsEnabled = false; }

            ButtonOK.IsEnabled = false;
        }
    }
}
