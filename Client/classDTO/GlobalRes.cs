using MaterialDesignThemes.Wpf;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Reflection;
using System.Threading;
using System.IO.Packaging;
using System.Windows.Controls;

namespace Client.classDTO
{
    public class GlobalRes
    {
        public static bool flagLoadSubdivision = false;
        public static SubdivisionDto[] itemsSubdivision;
        public static CompetenceDto[] itemsCompetence;
        public static GroupPositionDto[] itemsGroupPosition;
        public static string[] itemsProfile = { "Механик", "Технолог"};

        public static AdminDto[] itemsUserAdmin;
        public static UserEmployee[] itemsUserEmployee;
        public static bool flagUpdateEmployee = true;
        public static bool flagUpdateAdmin = true;
        
        public static int indexItemsUserEmployee = 0;
        public static float[,] matrixCoeff = new float[5, 11];

        public GlobalRes()
        {
            LoadSubdivisions();
        }

        public async Task LoadSubdivisions()
        {
            CompetenceCoeffsDTO[] coeffs;
            var jsonSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            ConnectHost conn = new ConnectHost();
            JToken jObject = await conn.GetGroupPositions();
            itemsGroupPosition = JsonConvert.DeserializeObject<GroupPositionDto[]>(jObject.ToString(), jsonSettings);

           
            jObject = await conn.GetCompetences();
            itemsCompetence = JsonConvert.DeserializeObject<CompetenceDto[]>(jObject.ToString(), jsonSettings);

           
            jObject =  await conn.GetSubdivisions();

            

            itemsSubdivision = JsonConvert.DeserializeObject<SubdivisionDto[]>(jObject.ToString(), jsonSettings);
            foreach (var item in itemsSubdivision)
            {
                var index = searchID_Groupe(item.IdGroupPositions);
                item.Profile = itemsProfile[itemsGroupPosition[index].IdProfile - 1];
                item.NameGroupPositions = itemsGroupPosition[index].Name;
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

            jObject = await conn.GetCompetenceCoeffs();
            coeffs = JsonConvert.DeserializeObject<CompetenceCoeffsDTO[]>(jObject.ToString(), jsonSettings);
            foreach (var coefficient in coeffs)
            {
                matrixCoeff[coefficient.IdCompetence, coefficient.IdGroup] = coefficient.Coefficient;
            }


        }

        public static int searchID_Groupe(int index)
        {
            int find_index = 0;
            int i = 0;
            foreach (var item in itemsGroupPosition) 
            {
                
                if (item.Id == index) 
                {
                    find_index = i;
                    return find_index;
                }
                i++;
            }
            return 0;
        }

        public static EmployeeDto GetEmployee(string ID)
        {
            foreach (var item in itemsUserEmployee)
            {
                if (item.employee.Id == ID)
                {
                    return item.employee;
                }
            }

            return null;
        }

        public static SubdivisionDto GetSubdivision(int ID)
        {
            foreach (var item in itemsSubdivision)
            {
                if (item.Id == ID)
                {
                    return item;
                }
            }

            return null;
        }

        public static CompetenceDto GetCompetence(int ID)
        {
            foreach (var item in itemsCompetence)
            {
                if (item.Id == ID)
                {
                    return item;
                }
            }

            return null;
        }

        public static GroupPositionDto GetGroupPosition(int ID)
        {
            foreach (var item in itemsGroupPosition)
            {
                if (item.Id == ID)
                {
                    return item;
                }
            }

            return null;
        }

        public static async void getUserEmployee()
        {
            EmployeeDto[] employees;
            ConnectHost conn = new ConnectHost();
            JToken jObject = await conn.GetEmployees();
            var jsonSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };
            employees = JsonConvert.DeserializeObject<EmployeeDto[]>(jObject.ToString(), jsonSettings);

            UserEmployee[] tmpUserEmployee;


            tmpUserEmployee = new UserEmployee[employees.Count()];


            int i = 0;

            foreach (EmployeeDto employee in employees)
            {
                var tmp = new UserEmployee();

                tmp.employee = employee;
                tmp.sub = GlobalRes.GetSubdivision(employee.IdSubdivision).Name;
                tmp.prof = GlobalRes.GetSubdivision(employee.IdSubdivision).Profile;
                tmpUserEmployee[i] = tmp;

                i++;
            }
            itemsUserEmployee = tmpUserEmployee;

            flagUpdateEmployee = false;
        }

        public static async void getUserAdmin()
        {
            AdminDto[] admins;
            ConnectHost conn = new ConnectHost();
            JToken jObject = await conn.GetAdmins();
            var jsonSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };
            admins = JsonConvert.DeserializeObject<AdminDto[]>(jObject.ToString(), jsonSettings);

           
            itemsUserAdmin = admins;

            flagUpdateAdmin = false;
        }

    }
}
