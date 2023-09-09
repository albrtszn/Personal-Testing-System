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
        public InitDB(EFDbContext db)
        {
            if (!db.Subdivisions.Any())
            {
                db.Subdivisions.Add(new Subdivision
                {
                    Name = "Отдел кадров"
                });

                db.Subdivisions.Add(new Subdivision
                {
                    Name = "Инженерный цех"
                });

                db.SaveChanges(true);
            }

            string employeeId = Guid.NewGuid().ToString();
            if (!db.Employees.Any())
            {
                db.Employees.Add(new Employee
                {
                    Id = employeeId,
                    FirstName = "Андрей",
                    SecondName = "Кравцов",
                    LastName = "Нежеленко",
                    Login = "login",
                    Password = "password",
                    DateOfBirth = DateOnly.Parse("01.01.2000"),
                    IdSubdivision = db.Subdivisions.ToList().Find(x => x.Name.Equals("Инженерный цех")).Id
                });

                db.SaveChanges(true);
            }

            if (!db.Admins.Any())
            {
                db.Admins.Add(new Admin
                {
                    Id = Guid.NewGuid().ToString(),
                    FirstName = "Евгений",
                    SecondName = "Жма",
                    LastName = "Дворцов",
                    Login = "admin",
                    Password = "password",
                    //DateOfBirth = DateOnly.Parse("01.01.2000"),
                    IdSubdivision = db.Subdivisions.ToList().Find(x => x.Name.Equals("Отдел кадров")).Id
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

            string testId = Guid.NewGuid().ToString();
            if (!db.Tests.Any())
            {
                db.Tests.Add(new Test
                {
                    Id = testId,
                    Name = "Тест для оценки школьных знаний",
                    Weight = 4,
                    IdCompetence = db.Competences.ToList().Find(x => x.Name.Equals("Оценка имеющихся компетенций")).Id
                });
                db.SaveChanges(true);


                if (!db.Questions.Any())
                {
                    //  1
                    string questionId1 = Guid.NewGuid().ToString();
                    db.Questions.Add(new Question
                    {
                        Id = questionId1,
                        Text = "Выберите правильное кол-во букв в русском алфавите:",
                        IdQuestionType = db.QuestionTypes.ToList().Find(x => x.Name.Equals("Выбор одного варианта ответа")).Id,
                        IdTest = testId
                    });

                    db.Answers.Add(new Answer
                    {
                        Text = "32",
                        IdQuestion = questionId1,
                        Correct = false
                        //Number
                        //ImagePath
                    });

                    db.Answers.Add(new Answer
                    {
                        Text = "34",
                        IdQuestion = questionId1,
                        Correct = false
                        //Number
                        //ImagePath
                    });

                    db.Answers.Add(new Answer
                    {
                        Text = "33",
                        IdQuestion = questionId1,
                        Correct = true
                        //Number
                        //ImagePath
                    });

                    //2
                    string questionId2 = Guid.NewGuid().ToString();
                    db.Questions.Add(new Question
                    {
                        Id = questionId2,
                        Text = "Сумма чисел равная 8 в следующих вариантах:",
                        IdQuestionType = db.QuestionTypes.ToList().Find(x => x.Name.Equals("Выбор нескольких вариантов ответа")).Id,
                        IdTest = testId
                    });

                    db.Answers.Add(new Answer
                    {
                        Text = "3+5",
                        IdQuestion = questionId2,
                        Correct = true
                        //Number
                        //ImagePath
                    });

                    db.Answers.Add(new Answer
                    {
                        Text = "6+2",
                        IdQuestion = questionId2,
                        Correct = true
                        //Number
                        //ImagePath
                    });

                    db.Answers.Add(new Answer
                    {
                        Text = "3+3",
                        IdQuestion = questionId2,
                        Correct = false
                        //Number
                        //ImagePath
                    });

                    //3
                    string questionId3 = Guid.NewGuid().ToString();
                    db.Questions.Add(new Question
                    {
                        Id = questionId3,
                        Text = "Переведите слова с английского языка и соедините с правильным вариантом:",
                        IdQuestionType = db.QuestionTypes.ToList().Find(x => x.Name.Equals("Установка соответствия")).Id,
                        IdTest = testId
                    });

                    string firstPartId1 = Guid.NewGuid().ToString();
                    db.FirstParts.Add(new FirstPart
                    {
                        Id = firstPartId1,
                        Text = "Motherhood",
                        IdQuestion = questionId3,
                    });

                    db.SecondParts.Add(new SecondPart
                    {
                        Text = "Родина",
                        IdFirstPart = firstPartId1
                    });

                    string firstPartId2 = Guid.NewGuid().ToString();
                    db.FirstParts.Add(new FirstPart
                    {
                        Id = firstPartId2,
                        Text = "Remote relatives",
                        IdQuestion = questionId3,
                    });

                    db.SecondParts.Add(new SecondPart
                    {
                        Text = "Дальние родственники",
                        IdFirstPart = firstPartId2
                    });

                    string firstPartId3 = Guid.NewGuid().ToString();
                    db.FirstParts.Add(new FirstPart
                    {
                        Id = firstPartId3,
                        Text = "Kind-hearted",
                        IdQuestion = questionId3,
                    });

                    db.SecondParts.Add(new SecondPart
                    {
                        Text = "Добродушный",
                        IdFirstPart = firstPartId3
                    });

                    //4
                    string questionId4 = Guid.NewGuid().ToString();
                    db.Questions.Add(new Question
                    {
                        Id = questionId4,
                        Text = "Составте последовательность действий при чп на рабочем месте расставляя варианты в нужном порядке:",
                        IdQuestionType = db.QuestionTypes.ToList().Find(x => x.Name.Equals("Расстановка в нужном порядке")).Id,
                        IdTest = testId
                    });

                    db.Subsequences.Add(new Subsequence
                    {
                        Text = "Выключить станок/компьютер и устранить последствия",
                        IdQuestion = questionId4,
                        Number = 2
                    });

                    db.Subsequences.Add(new Subsequence
                    {
                        Text = "Разбить стекло шкафа с аптечкой и подхилиться",
                        IdQuestion = questionId4,
                        Number = 1
                    });

                    db.Subsequences.Add(new Subsequence
                    {
                        Text = "Сообщить начальнику о чп",
                        IdQuestion = questionId4,
                        Number = 3
                    });

                    db.SaveChanges(true);

                    if (!db.TestPurposes.Any())
                    {
                        db.TestPurposes.Add(new TestPurpose
                        {
                            IdEmployee = employeeId,
                            IdTest = testId,
                            DatatimePurpose = DateTime.Parse("13.07.2023 12:00:00"),
                        });

                        db.SaveChanges(true);
                    }
                }
            }
        }
        public static void InitData(EFDbContext db)
        {
            if (!db.Subdivisions.Any())
            {
                db.Subdivisions.Add(new Subdivision
                {
                    Name = "Отдел кадров"
                });

                db.Subdivisions.Add(new Subdivision
                {
                    Name = "Инженерный цех"
                });

                db.SaveChanges(true);
            }

            /*string employeeId = Guid.NewGuid().ToString();
            if (!db.Employees.Any())
            {
                db.Employees.Add(new Employee
                {
                    Id = employeeId,
                    FirstName = "Андрей",
                    SecondName = "Кравцов",
                    LastName = "Нежеленко",
                    Login = "login",
                    Password = "password",
                    DateOfBirth = DateOnly.Parse("01.01.2000"),
                    IdSubdivision = db.Subdivisions.ToList().Find(x => x.Name.Equals("Инженерный цех")).Id
                });

                db.SaveChanges(true);
            }*/

            if (!db.Admins.Any())
            {
                db.Admins.Add(new Admin
                {
                    Id = Guid.NewGuid().ToString(),
                    FirstName = "Евгений",
                    SecondName = "Жма",
                    LastName = "Дворцов",
                    Login = "admin",
                    Password = "password",
                    //DateOfBirth = DateOnly.Parse("01.01.2000"),
                    IdSubdivision = db.Subdivisions.ToList().Find(x => x.Name.Equals("Отдел кадров")).Id
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

            /*string testId = Guid.NewGuid().ToString();
            if (!db.Tests.Any())
            {
                db.Tests.Add(new Test
                {
                    Id = testId,
                    Name = "Тест для оценки школьных знаний",
                    Weight = 4,
                    IdCompetence = db.Competences.ToList().Find(x => x.Name.Equals("Оценка имеющихся компетенций")).Id
                });
                db.SaveChanges(true);


                if (!db.Questions.Any())
                {
                    //  1
                    string questionId1 = Guid.NewGuid().ToString();
                    db.Questions.Add(new Question
                    {
                        Id = questionId1,
                        Text = "Выберите правильное кол-во букв в русском алфавите:",
                        IdQuestionType = db.QuestionTypes.ToList().Find(x => x.Name.Equals("Выбор одного варианта ответа")).Id,
                        IdTest = testId
                    });

                    db.Answers.Add(new Answer
                    {
                        Text = "32",
                        IdQuestion = questionId1,
                        Correct = false
                        //Number
                        //ImagePath
                    });

                    db.Answers.Add(new Answer
                    {
                        Text = "34",
                        IdQuestion = questionId1,
                        Correct = false
                        //Number
                        //ImagePath
                    });

                    db.Answers.Add(new Answer
                    {
                        Text = "33",
                        IdQuestion = questionId1,
                        Correct = true
                        //Number
                        //ImagePath
                    });

                    //2
                    string questionId2 = Guid.NewGuid().ToString();
                    db.Questions.Add(new Question
                    {
                        Id = questionId2,
                        Text = "Сумма чисел равная 8 в следующих вариантах:",
                        IdQuestionType = db.QuestionTypes.ToList().Find(x => x.Name.Equals("Выбор нескольких вариантов ответа")).Id,
                        IdTest = testId
                    });

                    db.Answers.Add(new Answer
                    {
                        Text = "3+5",
                        IdQuestion = questionId2,
                        Correct = true
                        //Number
                        //ImagePath
                    });

                    db.Answers.Add(new Answer
                    {
                        Text = "6+2",
                        IdQuestion = questionId2,
                        Correct = true
                        //Number
                        //ImagePath
                    });

                    db.Answers.Add(new Answer
                    {
                        Text = "3+3",
                        IdQuestion = questionId2,
                        Correct = false
                        //Number
                        //ImagePath
                    });

                    //3
                    string questionId3 = Guid.NewGuid().ToString();
                    db.Questions.Add(new Question
                    {
                        Id = questionId3,
                        Text = "Переведите слова с английского языка и соедините с правильным вариантом:",
                        IdQuestionType = db.QuestionTypes.ToList().Find(x => x.Name.Equals("Установка соответствия")).Id,
                        IdTest = testId
                    });

                    string firstPartId1 = Guid.NewGuid().ToString();
                    db.FirstParts.Add(new FirstPart
                    {
                        Id = firstPartId1,
                        Text = "Motherhood",
                        IdQuestion = questionId3,
                    });

                    db.SecondParts.Add(new SecondPart
                    {
                        Text = "Родина",
                        IdFirstPart = firstPartId1
                    });

                    string firstPartId2 = Guid.NewGuid().ToString();
                    db.FirstParts.Add(new FirstPart
                    {
                        Id = firstPartId2,
                        Text = "Remote relatives",
                        IdQuestion = questionId3,
                    });

                    db.SecondParts.Add(new SecondPart
                    {
                        Text = "Дальние родственники",
                        IdFirstPart = firstPartId2
                    });

                    string firstPartId3 = Guid.NewGuid().ToString();
                    db.FirstParts.Add(new FirstPart
                    {
                        Id = firstPartId3,
                        Text = "Kind-hearted",
                        IdQuestion = questionId3,
                    });

                    db.SecondParts.Add(new SecondPart
                    {
                        Text = "Добродушный",
                        IdFirstPart = firstPartId3
                    });

                    //4
                    string questionId4 = Guid.NewGuid().ToString();
                    db.Questions.Add(new Question
                    {
                        Id = questionId4,
                        Text = "Составте последовательность действий при чп на рабочем месте расставляя варианты в нужном порядке:",
                        IdQuestionType = db.QuestionTypes.ToList().Find(x => x.Name.Equals("Расстановка в нужном порядке")).Id,
                        IdTest = testId
                    });

                    db.Subsequences.Add(new Subsequence
                    {
                        Text = "Выключить станок/компьютер и устранить последствия",
                        IdQuestion = questionId4,
                        Number = 2
                    });

                    db.Subsequences.Add(new Subsequence
                    {
                        Text = "Разбить стекло шкафа с аптечкой и подхилиться",
                        IdQuestion = questionId4,
                        Number = 1
                    });

                    db.Subsequences.Add(new Subsequence
                    {
                        Text = "Сообщить начальнику о чп",
                        IdQuestion = questionId4,
                        Number = 3
                    });

                    db.SaveChanges(true);

                    if (!db.TestPurposes.Any())
                    {
                        db.TestPurposes.Add(new TestPurpose
                        {
                            IdEmployee = employeeId,
                            IdTest = testId,
                            DatatimePurpose = DateTime.Parse("13.07.2023 12:00:00"),
                        });
            
                        db.SaveChanges(true);
                    }
                }
            }*/
            db.SaveChanges(true);
        }
    }
}