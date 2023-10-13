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

namespace Client.classDTO
{
    public class GlobalRes
    {
        public static SubdivisionDto[] itemsSubdivision;
        public static CompetenceDto[] itemsCompetence;
        public static GroupPositionDto[] itemsGroupPosition;
        public static string[] itemsProfile = { "Механик", "Технолог"};

        public GlobalRes()
        {
            LoadSubdivisions();
        }

        public async void LoadSubdivisions()
        {
            ConnectHost conn = new ConnectHost();
            JToken jObject = await conn.GetGroupPositions();
            itemsGroupPosition = JsonConvert.DeserializeObject<GroupPositionDto[]>(jObject.ToString());

           
            jObject = await conn.GetCompetences();
            itemsCompetence = JsonConvert.DeserializeObject<CompetenceDto[]>(jObject.ToString());

           
            jObject =  await conn.GetSubdivisions();

            

            itemsSubdivision = JsonConvert.DeserializeObject<SubdivisionDto[]>(jObject.ToString());
            foreach (var item in itemsSubdivision)
            {
                var index = searchID_Groupe(item.IdGroupPositions);
                item.Profile = itemsProfile[itemsGroupPosition[index].IdProfile - 1];
                item.NameGroupPositions = itemsGroupPosition[index].Name;
            }
        }

        private int searchID_Groupe(int index)
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

    }




}
