using Client.classDTO;
using Client.forms;
using Client.pages;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Client.VM
{
    class PageChoiceUserVM : DependencyObject
    {
        private PageChoiceUser myGlobal;
        public RelayCommand CmdViewResult { get; }
        public RelayCommand CmdViewСompetency {get;}

        public class UserEmployee
        {
            public string Name { get; set; }
            public string id { get; set; }
            public string dateB { get ; set; }
            public string sub { get; set; }
            public string prof { get; set; }
        }

        public string FilterText
        {
            get { return (string)GetValue(FilterTextProperty); }
            set { SetValue(FilterTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FilterText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FilterTextProperty =
            DependencyProperty.Register("FilterText", typeof(string), typeof(PageChoiceUserVM), new PropertyMetadata("", FilterText_Changed));

        private static void FilterText_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var curr = d as PageChoiceUserVM;
            if (curr != null)
            {
                curr.Items.Filter = null;
                curr.Items.Filter = curr.FilterUsers;
            }
        }

        private bool FilterUsers(object obj)
        {
            bool result = true;
            UserEmployee curr = obj as UserEmployee;
            if (!string.IsNullOrEmpty(FilterText) && (curr != null) && !(curr.Name.Contains(FilterText)) && !(curr.sub.Contains(FilterText)))
            {
                result = false;

            }
            return result;
        }

        public ICollectionView Items
        {
            get { return (ICollectionView)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Items.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("Items", typeof(ICollectionView), typeof(PageChoiceUserVM), new PropertyMetadata(null));


        public PageChoiceUserVM(object myOwner)
        {
            myGlobal = myOwner as PageChoiceUser;
            this.CmdViewResult = new RelayCommand(FuncViewResult);
            this.CmdViewСompetency = new RelayCommand(FuncViewСompetency);
            
            GetUsers();
        }

        void FuncViewResult(object param)
        {
            UserEmployee tmp = param as UserEmployee;

            

            myGlobal.NavigationService.Navigate(new PageResults(tmp.id));
            //Navigate(new PageResults());
        }

        void FuncViewСompetency(object param)
        {
            UserEmployee tmp = param as UserEmployee;

            myGlobal.NavigationService.Navigate(new PageCompetencyAsses(tmp.id));
            //Navigate(new PageResults());
        }

        public async void GetUsers()
        {

            EmployeeDto[] employees;
            ConnectHost conn = new ConnectHost();
            JToken jObject = await conn.GetEmployees();

            var jsonSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            employees = JsonConvert.DeserializeObject<EmployeeDto[]>(jObject.ToString(), jsonSettings);

            UserEmployee[] users = new UserEmployee[employees.Count()];
            
            int i = 0;
            foreach (EmployeeDto employee in employees)
            {
                var tmp = new UserEmployee();
                tmp.id = employee.Id;
                tmp.Name = employee.LastName + " " + employee.FirstName + " " + employee.SecondName;
                tmp.dateB = employee.DateOfBirth;
                tmp.sub = GlobalRes.GetSubdivision(employee.IdSubdivision).Name;
                tmp.prof = GlobalRes.GetSubdivision(employee.IdSubdivision).Profile;
                users[i] = tmp;

                i++;
            }

            Items = CollectionViewSource.GetDefaultView(users);
            Items.Filter = FilterUsers;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}