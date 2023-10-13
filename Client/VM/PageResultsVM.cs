using Client.classDTO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Data;
using static Client.VM.UserVM;

namespace Client.VM
{

    public class PageResultsVM : DependencyObject
    {
        public class ResultView
        {
            public ResultDto result { get; set; }
            public string prof { get; set; }

        }

        public PageResultsVM(object myOwner)
        {
            LoadData();
        }


        public string FilterText
        {
            get { return (string)GetValue(FilterTextProperty); }
            set { SetValue(FilterTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FilterText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FilterTextProperty =
            DependencyProperty.Register("FilterText", typeof(string), typeof(PageResultsVM), new PropertyMetadata("", FilterText_Changed));

        private static void FilterText_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var curr = d as PageResultsVM;
            if (curr != null)
            {
                curr.Items.Filter = null;
                curr.Items.Filter = curr.FilterUsers;
            }
        }

        public ICollectionView Items
        {
            get { return (ICollectionView)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Items.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("Items", typeof(ICollectionView), typeof(PageResultsVM), new PropertyMetadata(null));

        public async void LoadData()
        {
            ResultDto[] res;
            
            ConnectHost conn = new ConnectHost();
            JToken jObject = await conn.GetResults();

            var jsonSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            res = JsonConvert.DeserializeObject<ResultDto[]>(jObject.ToString(), jsonSettings);

            ResultView[] resultViews = new ResultView[res.Count()];
            int i = 0;
            foreach ( var item in res ) 
            {
                resultViews[i] = new ResultView();
                resultViews[i].result = new ResultDto();
                resultViews[i].result = item;
                resultViews[i].prof = GlobalRes.GetSubdivision(item.Employee.Subdivision.Id).Profile;

                i++;
            }

            Items = CollectionViewSource.GetDefaultView(resultViews);


        }

        private bool FilterUsers(object obj)
        {
            bool result = true;
            ResultView curr = obj as ResultView;
            if (!string.IsNullOrEmpty(FilterText) && (curr != null) && !(curr.result.Employee.FirstName.Contains(FilterText)) && !(curr.result.Employee.LastName.Contains(FilterText)))
            {
                result = false;

            }
            return result;
        }
    }
}
