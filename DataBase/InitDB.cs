using DataBase.Repository;
using DataBase.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase
{
    public class InitDB
    {
        public static void InitData(EFDbContext db)
        {
            if (!db.Subdivisions.Any())
            {
                db.Subdivisions.Add(new Subdivision
                {
                    Name = "Отдел кадров"
                });

                db.SaveChanges(true);
            }

            if (!db.Employees.Any())
            {
                db.Employees.Add(new Employee
                {
                    FirstName = "Андрей",
                    SecondName = "Кравцов",
                    LastName = "Нежеленко",
                    IdSubdivision = db.Subdivisions.ToList().Find(x => x.Name == "Отдел кадров").Id
                });

                db.SaveChanges(true);
            }
        }
    }
}