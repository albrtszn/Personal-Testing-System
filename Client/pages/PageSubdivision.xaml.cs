using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
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
using Client.forms;
using Client.classDTO;
using System.Collections.ObjectModel;

namespace Client.pages
{
    public partial class PageSubdivision : Page
    {
        private GlobalRes globalRes = new GlobalRes();
        public PageSubdivision()
        {
            InitializeComponent();
            Loaded += PageSubdivision_Loaded;
            
        }

        private void PageSubdivision_Loaded(object sender, RoutedEventArgs e)
        {
            LoadSubdivisions();
        }

        private async void LoadSubdivisions()
        {
            dataGridSubdivision.ItemsSource = null;
            await globalRes.LoadSubdivisions();
            dataGridSubdivision.ItemsSource = GlobalRes.itemsSubdivision;
            BRquestion.Visibility = Visibility.Collapsed;
        }

        private void ButtonAdd(object sender, RoutedEventArgs e)
        {
            BRquestion.Visibility = Visibility.Visible;
        }

        private void ButtonClose(object sender, RoutedEventArgs e)
        {
            BRquestion.Visibility = Visibility.Collapsed;
        }

        private async void ButtonSubADD(object sender, RoutedEventArgs e)
        {
            int a = groupeCB.SelectedIndex;
            int b = profileCB.SelectedIndex;
            string c = NameSub.Text.Trim();
            int inx = a + b * 4;
            int idGroupe = GlobalRes.itemsGroupPosition[inx].Id;  

            if (c.Length > 2)
            {
                string pay = "{\"Name\":\"" + c + "\",\"IdGroupPositions\":" + idGroupe.ToString() + "}";
                ConnectHost conn = new ConnectHost();
                JToken jObject = await conn.AddSubdivision(pay);
                if (jObject != null)
                {
                    MessageBox.Show("Должность успешно добавлена");
                    NameSub.Text = string.Empty;
                    groupeCB.SelectedIndex = 0;
                    profileCB.SelectedIndex = 0;
                    LoadSubdivisions();
                }
                else
                {
                    MessageBox.Show("Не удалось добавить должность");
                }

                
            }

            
        }
    }
}
