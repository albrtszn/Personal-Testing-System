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

        public PageSubdivision()
        {
            InitializeComponent();
            LoadSubdivisions();
        }

        private async void LoadSubdivisions()
        {

           
            dataGridSubdivision.ItemsSource = GlobalRes.itemsSubdivision;

        }
    }
}
