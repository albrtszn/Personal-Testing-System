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
                    Login = "login",
                    Password = "password",
                    DateOfBirth = DateOnly.Parse("01.01.2000"),
                    IdSubdivision = db.Subdivisions.ToList().Find(x => x.Name == "Отдел кадров").Id
                });

                db.SaveChanges(true);
            }

            if (!db.Admins.Any())
            {
                db.Admins.Add(new Admin
                {
                    FirstName = "Евгений",
                    SecondName = "Жма",
                    LastName = "Дворцов",
                    Login = "admin",
                    Password = "password",
                    //DateOfBirth = DateOnly.Parse("01.01.2000"),
                    IdSubdivision = db.Subdivisions.ToList().Find(x => x.Name == "Отдел кадров").Id
                });

                db.SaveChanges(true);
            }

            if (!db.Competences.Any())
            {
                db.Competences.Add(new Competence
                {
                    Name = "Оценка имеющихся компетенций"
                });

                db.SaveChanges(true);
            }

            if (!db.QuestionTypes.Any())
            {
                db.QuestionTypes.Add(new QuestionType
                {
                    Name = "Выбор одного варианта ответа"
                });

                db.QuestionTypes.Add(new QuestionType
                {
                    Name = "Выбор нескольких вариантов ответа"
                });

                db.QuestionTypes.Add(new QuestionType
                {
                    Name = "Установка соответствия"
                });

                db.QuestionTypes.Add(new QuestionType
                {
                    Name = "Расстановка в нужном порядке"
                });

                db.SaveChanges(true);
            }
        }
    }
}