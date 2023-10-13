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
using System.Threading;
using Client.pages;
using Client.forms;
using System.Runtime.CompilerServices;

namespace Client.VM
{
    class UserVM : DependencyObject
    {
        private PageUsers myGlobal;

        public class UserEmployee
        {
            public EmployeeDto employee { get; set; }
            public string sub { get; set; }
            public string prof { get; set; }
        }

        public RelayCommand CmdAddPurpose { get; }

        public string FilterText
        {
            get { return (string)GetValue(FilterTextProperty); }
            set { SetValue(FilterTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FilterText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FilterTextProperty =
            DependencyProperty.Register("FilterText", typeof(string), typeof(UserVM), new PropertyMetadata("", FilterText_Changed));

        private static void FilterText_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var curr = d as UserVM;
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
            DependencyProperty.Register("Items", typeof(ICollectionView), typeof(UserVM), new PropertyMetadata(null));

        public UserVM(object myOwner) 
        {
            myGlobal = myOwner as PageUsers;

            GetUsers();
            this.CmdAddPurpose = new RelayCommand(FuncAddPurpose);

        }

        void FuncAddPurpose(object param)
        {
            MessageBox.Show("Hello");
        }

        public async void GetUsers()
        {

            classDTO.GlobalRes globalRes = new classDTO.GlobalRes();
            

            EmployeeDto[] employees;
            ConnectHost conn = new ConnectHost();
            JToken jObject = await conn.GetEmployees();
            employees = JsonConvert.DeserializeObject<EmployeeDto[]>(jObject.ToString());

            
            UserEmployee[] users = new UserEmployee[employees.Count()];
            int i = 0;
            foreach (EmployeeDto employee in employees) 
            {
                var tmp = new UserEmployee();
                
                tmp.employee = employee;
                tmp.sub = GlobalRes.GetSubdivision(employee.IdSubdivision).Name;
                tmp.prof = GlobalRes.GetSubdivision(employee.IdSubdivision).Profile;
                users[i] = tmp;
                    
                i++;    
            }

            Items = CollectionViewSource.GetDefaultView(users);
            Items.Filter = FilterUsers;
        }



        private bool FilterUsers(object obj)
        {
            bool result = true;
            UserEmployee curr = obj as UserEmployee;
            if (!string.IsNullOrEmpty(FilterText) && (curr != null) && !(curr.employee.FirstName.Contains(FilterText)) && !(curr.employee.LastName.Contains(FilterText)))
            {
                result = false;

            }
            return result;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
