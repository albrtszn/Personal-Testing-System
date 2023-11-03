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
using System.Windows.Forms;
using Client.windows;
using static System.Net.Mime.MediaTypeNames;

namespace Client.VM
{
    class UserVM : DependencyObject
    {
        private PageUsers myGlobal;

        public RelayCommand CmdAddPurpose { get; }
        public RelayCommand CmdDeletUser { get; }
        public RelayCommand CmdEditUser { get; }


        public static UserEmployee SelectedItems { get; set; }



        public string FilterPhone
        {
            get { return (string)GetValue(FilterPhoneProperty); }
            set { SetValue(FilterPhoneProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FilterPhone.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FilterPhoneProperty =
            DependencyProperty.Register("FilterPhone", typeof(string), typeof(UserVM), new PropertyMetadata("", FilterText_Changed));



        public string FilterDateB
        {
            get { return (string)GetValue(FilterDateBProperty); }
            set { SetValue(FilterDateBProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FilterText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FilterDateBProperty =
            DependencyProperty.Register("FilterDateB", typeof(string), typeof(UserVM), new PropertyMetadata("", FilterText_Changed));

        public string FilterSub
        {
            get { return (string)GetValue(FilterSubProperty); }
            set { SetValue(FilterSubProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FilterText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FilterSubProperty =
            DependencyProperty.Register("FilterSub", typeof(string), typeof(UserVM), new PropertyMetadata("", FilterText_Changed));


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
                curr.Items.Filter = curr.Filters;
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
            this.CmdDeletUser = new RelayCommand(FuncDeletUser);
            this.CmdEditUser = new RelayCommand(FuncEditUser);
            
        }

        void FuncEditUser(object param)
        {
            UserEmployee tmp = param as UserEmployee;
            SelectedItems = tmp;
            myGlobal.NavigationService.Navigate(new PageUserEdit(tmp.employee));

        }

        void FuncDeletUser(object param)
        {
            UserEmployee tmp_user = param as UserEmployee;
            var tmp = new PasswordWindow(tmp_user.employee.Id);
            tmp.Show();
            SelectedItems = null;
            GlobalRes.flagUpdateEmployee = true;
        }
        
        void FuncAddPurpose(object param)
        {
            UserEmployee tmp = param as UserEmployee;
            SelectedItems = tmp;
            //this.NavigationService.Navigate(new PageAddPurpose(emp.employee));
            myGlobal.NavigationService.Navigate(new PageAddPurpose(tmp.employee));
        }

        public async void GetUsers()
        {
            EmployeeDto[] employees;
            ConnectHost conn = new ConnectHost();
            JToken jObject = null;
            var jsonSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };


            if (GlobalRes.flagLoadSubdivision == false)
            {
                jObject = await conn.GetGroupPositions();
                GlobalRes.itemsGroupPosition = JsonConvert.DeserializeObject<GroupPositionDto[]>(jObject.ToString(), jsonSettings);

                jObject = await conn.GetCompetences();
                GlobalRes.itemsCompetence = JsonConvert.DeserializeObject<CompetenceDto[]>(jObject.ToString(), jsonSettings);


                jObject = await conn.GetSubdivisions();
                GlobalRes.itemsSubdivision = JsonConvert.DeserializeObject<SubdivisionDto[]>(jObject.ToString(), jsonSettings);


                foreach (var item in GlobalRes.itemsSubdivision)
                {
                    var index = GlobalRes.searchID_Groupe(item.IdGroupPositions);
                    item.Profile = GlobalRes.itemsProfile[GlobalRes.itemsGroupPosition[index].IdProfile - 1];
                    item.NameGroupPositions = GlobalRes.itemsGroupPosition[index].Name;
                    if (item.NameGroupPositions == "Группа 1")
                    {
                        item.NameGroupPositions2 = "Группа 1. Рабочие";
                    }
                    else if (item.NameGroupPositions == "Группа 2")
                    {
                        item.NameGroupPositions2 = "Группа 2. Инженерные и руководящие начального уровня";
                    }
                    else if (item.NameGroupPositions == "Группа 3")
                    {
                        item.NameGroupPositions2 = "Группа 3. Руководящие среднего уровня";
                    }
                    else if (item.NameGroupPositions == "Группа 4")
                    {
                        item.NameGroupPositions2 = "Группа 4. Руководящие высшего уровня";
                    }
                }
                GlobalRes.flagLoadSubdivision = true;
            }
            
            if (GlobalRes.flagUpdateEmployee == true)
            {
                jObject = await conn.GetEmployees();
                employees = JsonConvert.DeserializeObject<EmployeeDto[]>(jObject.ToString(), jsonSettings);

                UserEmployee[] users = new UserEmployee[employees.Count()];
                int i = 0;

                foreach (EmployeeDto employee in employees)
                {
                    var tmp = new UserEmployee();

                    tmp.employee = employee;
                    tmp.sub = GlobalRes.GetSubdivision(employee.IdSubdivision).Name;
                    tmp.prof = GlobalRes.GetSubdivision(employee.IdSubdivision).Profile;
                    tmp.kolPur = employee.CountOfPurposes.ToString() + " из " + employee.CountOfTestsToPurpose.ToString();
                    users[i] = tmp;

                    i++;
                }

                GlobalRes.itemsUserEmployee = users;
                SelectedItems = users[0];
                GlobalRes.flagUpdateEmployee = false;
            }

            Items = CollectionViewSource.GetDefaultView(GlobalRes.itemsUserEmployee);
            Items.Filter = Filters;
            
        }

        private bool Filters(object obj)
        {
            bool result = true;
            UserEmployee curr = obj as UserEmployee;
            bool A = true;
            bool B = false;
            bool C = true;
            bool D = true;
           
            if (curr != null)
            {
                A = string.IsNullOrEmpty(FilterText) || curr.employee.FirstName.Contains(FilterText) || (curr.employee.LastName.Contains(FilterText)) || (curr.employee.SecondName.Contains(FilterText));
                C = string.IsNullOrEmpty(FilterSub) || curr.sub.Contains(FilterSub);
                D = string.IsNullOrEmpty(FilterDateB) || curr.employee.DateOfBirth.Contains(FilterDateB); 
                B = string.IsNullOrEmpty(FilterPhone) || curr.employee.Phone.Contains(FilterPhone);
                result = A && C && D && B; 
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
