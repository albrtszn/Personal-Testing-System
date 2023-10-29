using Client.classDTO;
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

namespace Client.pages
{
    public partial class PageUserEdit : Page
    {
        private EmployeeDto user = new EmployeeDto();

        public PageUserEdit(EmployeeDto myUser)
        {
            InitializeComponent();
            user = myUser;
            DataContext = user;
            profileCB.Text = GlobalRes.GetSubdivision(user.IdSubdivision).Profile;
            groupeCB.Text = GlobalRes.GetSubdivision(user.IdSubdivision).NameGroupPositions2;
            subdivisionComboBox.Text = GlobalRes.GetSubdivision(user.IdSubdivision).Name;
            DataRegUpdate.Text = user.RegistrationDate;
        }

        private void Button_Click_Back(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private async void Button_Click_update(object sender, RoutedEventArgs e)
        {
            BT_update.IsEnabled = false;
            user.RegistrationDate = System.DateTime.Now.ToShortDateString();    
            string tmp = JsonConvert.SerializeObject(user);
            ConnectHost conn = new ConnectHost();
            JToken jObject = await conn.UpdateEmployee(tmp);
            if (jObject != null)
            {
                MessageBox.Show("Данные пользователя успешно обновлены");
                DataRegUpdate.Text = user.RegistrationDate;
            }
            else
            {
                MessageBox.Show("Данные не обновлены!");
            }
            BT_update.IsEnabled = true;
        }
    }
}
