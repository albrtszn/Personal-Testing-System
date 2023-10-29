using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using Client.classDTO;
using static Client.VM.UserVM;

namespace Client.VM
{
    class TestsVM : DependencyObject
    {
        public class TestView
        {
            public int number { get; set; }
            public TestDto test { get; set; }
            public string nameCompetense { get; set; }
            public string nameProfile { get; set; }
        }

        public static TestDto[] tests;

        public ICollectionView ItemsTests
        {
            get { return (ICollectionView)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Items.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("ItemsTests", typeof(ICollectionView), typeof(TestsVM), new PropertyMetadata(null));



        public string FilterText
        {
            get { return (string)GetValue(FilterTextProperty); }
            set { SetValue(FilterTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FilterText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FilterTextProperty =
            DependencyProperty.Register("FilterText", typeof(string), typeof(TestsVM), new PropertyMetadata("", FilterText_Changed));

        private static void FilterText_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var curr = d as TestsVM;
            if (curr != null)
            {
                curr.ItemsTests.Filter = null;
                curr.ItemsTests.Filter = curr.FilterTests;
            }
        }

        private bool FilterTests(object obj)
        {
            bool result = true;
            TestView curr = obj as TestView;
            if (!string.IsNullOrEmpty(FilterText) && (curr != null) && !(curr.test.Name.Contains(FilterText)) )
            {
                result = false;

            }
            return result;
        }

        public TestsVM()
        {
            LoadData();


        }

        public async void LoadData()
        {
            
            ConnectHost conn = new ConnectHost();
            JToken jObject = await conn.GetTests();
            tests = JsonConvert.DeserializeObject<TestDto[]>(jObject.ToString());

            TestView[] alltest = new TestView[tests.Count()];
            int i = 0;
            foreach (TestDto test in tests)
            {
                var tmp = new TestView();

                tmp.number = i + 1;
                tmp.test = test;
                tmp.nameCompetense = GlobalRes.GetCompetence(test.CompetenceId).Name;
                alltest[i] = tmp;
                i++;
            }

            ItemsTests = CollectionViewSource.GetDefaultView(alltest);
            //ItemsTests.Filter = FilterTests;
        }
    }
}
