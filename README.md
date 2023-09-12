# Personal-Testing-System
Testing system for personal(workers/candidates). Conducting a test to determine the assessment of skills and the level of education for effective work.
---
Система создана для определения профессиональных навыков, уровня развития и общей оценке кандидатов/рабочих. В качестве СУБД используется MS SQL, backend - asp.Net, front(клиентская программа) - C# Win Forms.
## Требования к ПО: ##
- Разделение на две подсистемы (пользователь , администратор);
- Доступ к работе с приложением после авторизации. Идентификация каждого актера работающего с приложением;
- Разделение вопросов на 3 типа: выбор правильного варианта, выбор нескольких вариантов ответа, установка соответствия,расстановка в нужном порядке;
- Администратор: ввод/редактирование тестов (с помощью адаптивного редактора тестов), назначение тестов, просмотр результатов (в формате -> Имя, Фамилия, Отчество, Подразделение, Название теста, Дата тестирования, Время начала, Длительность, Время завершения), вывод бумажной версии или Word образца теста с ответами\без ответов;
- Пользователь: прохождение теста после авторизации со стационарного компьютера.
---
## Документация API
В header 'Authorization' записывается токен авторизации для доступа к приложению. Вся информация принимается в Request body(пр. StringIdModel, IntIdmodel, и т.д.), отправляется в Response body(пр. TestModel, TestResultModel и т.д.). 
### /user-api
---
1. POST /Login
Request <- [LoginModel](Personal-Testing-System/Models/LoginModel.cs):
```
{
    "Login":"login",
    "Password":"password"
}
```
Response -> string, [employeeDto](Personal-Testing-System/DTOs/EmployeeDto.cs):
```
{
    "TokenEmployee": "xxxxxxxxxxxxxxxxxxxxxxxxxxxx",
    "Employee": {
        "Id": "xxxxxxxxxxxxxxxxxxxxxxxxxxxx",
        "FirstName": "Андрей",
        "SecondName": "Кравцов",
        "LastName": "Нежеленко",
        "Login": "login",
        "Password": "password",
        "DateOfBirth": "01.01.2000",
        "IdSubdivision": 2
    }
}
```
2. Post /user-api/LogOut
Request - Authorization.
Response - status message.
```
{
  "message": "Выполнен выход из системы"
}
```
3. GET /GetPurposesByEmployeeId
- Request <- Authroziation, [StringIdModel](Personal-Testing-System/Models/StringIdModel.cs).
```
Authorization(header): xxxxxxxxxxxxx
{
  "Id": "xxxxxxxxxxx"
}
```
- Response -> List of [PurposeModels](Personal-Testing-System/Models/PurposeModel.cs):
```
[
    {
        "Id": 1,
        "IdEmployee": "xxxxxxxxxxxxxxxxxxx",
        "Test": {
            "Id": "xxxxxxxxxxxxxxxxxxxx",
            "Name": "Тест для оценки школьных знаний",
            "Competence": {
                "Id": 1,
                "Name": "Базовые знания"
            }
        },
        "DatatimePurpose": "13.07.2023 0:00:00"
    }
]
```
4. GET /GetTest
- Request <- [StringIdModel](Personal-Testing-System/Models/StringIdModel.cs).
```
{
  "Id": "xxxxxxxxxxxxxxxx"
}
```
- Response -> [TestModel](Personal-Testing-System/Models/TestModel.cs):
```
{
  "Id": "02d3b2fa-f356-400a-ac20-90b5f358ea5d",
  "Name": "Тест для оценки школьных знаний",
  "Competence": {
    "Id": 1,
    "Name": "Оценка имеющихся компетенций"
  },
  "Questions": [
    {
      "Id": "02adb7d6-ad30-4a7d-bbd4-6463b8089a70",
      "Text": "Переведите слова с английского языка и соедините с правильным вариантом:",
      "ImagePath": null,
      "Base64Image": null,
      "IdQuestionType": 3,
      "Answers": [
        {
          "IdFirstPart": "0d9855bc-e833-4e13-ad72-b8bd1541afcb",
          "Text": "Motherhood",
          "IdQuestion": "02adb7d6-ad30-4a7d-bbd4-6463b8089a70"
        },
        {
          "IdFirstPart": "3fe5113f-f0d6-41a6-bfd7-4c8a011887c3",
          "Text": "Remote relatives",
          "IdQuestion": "02adb7d6-ad30-4a7d-bbd4-6463b8089a70"
        },
        {
          "IdFirstPart": "644078bb-05c2-44db-b330-8dabe350f492",
          "Text": "Kind-hearted",
          "IdQuestion": "02adb7d6-ad30-4a7d-bbd4-6463b8089a70"
        },
        {
          "IdSecondPart": 1,
          "Text": "Родина",
          "IdFirstPart": "0d9855bc-e833-4e13-ad72-b8bd1541afcb"
        },
        {
          "IdSecondPart": 2,
          "Text": "Дальние родственники",
          "IdFirstPart": "3fe5113f-f0d6-41a6-bfd7-4c8a011887c3"
        },
        {
          "IdSecondPart": 3,
          "Text": "Добродушный",
          "IdFirstPart": "644078bb-05c2-44db-b330-8dabe350f492"
        }
      ]
    },
    {
      "Id": "647113ea-5c83-465e-8d63-fb311f377d4b",
      "Text": "Составте последовательность действий при чп на рабочем месте расставляя варианты в нужном порядке:",
      "ImagePath": null,
      "Base64Image": null,
      "IdQuestionType": 4,
      "Answers": [
        {
          "IdSubsequence": 1,
          "Text": "Выключить станок/компьютер и устранить последствия",
          "IdQuestion": "647113ea-5c83-465e-8d63-fb311f377d4b",
          "Number": 2
        },
        {
          "IdSubsequence": 2,
          "Text": "Разбить стекло шкафа с аптечкой и подхилиться",
          "IdQuestion": "647113ea-5c83-465e-8d63-fb311f377d4b",
          "Number": 1
        },
        {
          "IdSubsequence": 3,
          "Text": "Сообщить начальнику о чп",
          "IdQuestion": "647113ea-5c83-465e-8d63-fb311f377d4b",
          "Number": 3
        }
      ]
    },
    {
      "Id": "c91631c2-e518-41f0-9761-0ee2a9669f59",
      "Text": "Выберите правильное кол-во букв в русском алфавите:",
      "ImagePath": null,
      "Base64Image": null,
      "IdQuestionType": 1,
      "Answers": [
        {
          "IdAnswer": 1,
          "Text": "32",
          "IdQuestion": "c91631c2-e518-41f0-9761-0ee2a9669f59",
          "Correct": false,
          "ImagePath": null
        },
        {
          "IdAnswer": 2,
          "Text": "34",
          "IdQuestion": "c91631c2-e518-41f0-9761-0ee2a9669f59",
          "Correct": false,
          "ImagePath": null
        },
        {
          "IdAnswer": 3,
          "Text": "33",
          "IdQuestion": "c91631c2-e518-41f0-9761-0ee2a9669f59",
          "Correct": true,
          "ImagePath": null
        }
      ]
    },
    {
      "Id": "d567b54c-9028-4051-a18f-09fad7b9c9d6",
      "Text": "Сумма чисел равная 8 в следующих вариантах:",
      "ImagePath": null,
      "Base64Image": null,
      "IdQuestionType": 2,
      "Answers": [
        {
          "IdAnswer": 4,
          "Text": "3+5",
          "IdQuestion": "d567b54c-9028-4051-a18f-09fad7b9c9d6",
          "Correct": true,
          "ImagePath": null
        },
        {
          "IdAnswer": 5,
          "Text": "6+2",
          "IdQuestion": "d567b54c-9028-4051-a18f-09fad7b9c9d6",
          "Correct": true,
          "ImagePath": null
        },
        {
          "IdAnswer": 6,
          "Text": "3+3",
          "IdQuestion": "d567b54c-9028-4051-a18f-09fad7b9c9d6",
          "Correct": false,
          "ImagePath": null
        }
      ]
    }
  ]
}
```
5. POST /PushTest
- Request <- [TestResultModel](Personal-Testing-System/Models/TestResultModel.cs).
```
{
    "TestId" : "57e6acdc-1c76-4bf8-b815-cc136deff9ac",
    "EmployeeId" : "2bf2df78-80dc-4539-be61-167bdd901b49",
    "StartDate" : "15.07.2023",
    "StartTime" : "12:12:00",
    "EndTime" : "13:12:00",
    "Questions" : 
    [
        {
            "QuestionId" : "eba3f532-1a01-4ecd-89fe-5aacc8a5a59d",
            "Answers" :
            [
                {
                    "AnswerId" : 3
                }
            ]
        },
        {
            "QuestionId" : "c37a74f3-6c02-48a7-9e13-744b2aad9114",
            "Answers" :
            [
                {
                    "AnswerId" : 4
                },
                {
                    "AnswerId" : 5
                }
            ]
        },
        {
            "QuestionId" : "288c27c1-bca7-4682-aa58-ce81b0186c4c",
            "Answers" :
            [
                {
                    "SubsequenceId" : 1,
                    "Number" : 2
                },
                {
                    "SubsequenceId" : 2,
                    "Number" : 1
                },                
                {
                    "SubsequenceId" : 3,
                    "Number" : 3
                }
            ]
        },
        {
            "QuestionId" : "468ae1fe-c2a4-45fd-8e38-bb431c51d8e1",
            "Answers" :
            [
                {
                    "FirstPartId" : "07e96419-cbc6-4b80-bebb-96c3db6883cb",
                    "SecondPartId" : 3
                },
                {
                   "FirstPartId" : "753f6352-89aa-4f02-a977-607ad65d56b0",
                    "SecondPartId" : 1
                },                
                {
                    "FirstPartId" : "c677f824-f7cc-4ecd-8ebe-0fa9446a53c0",
                    "SecondPartId" : 1
                }
            ]
        }
    ]
}
```
- Response -> string(score):
```
{
    "message": "Тест выполнен. Оценка: 3"
}
```
### admin-api/
---
1. POST /admin-api/Login.
- Request <- [LoginModel](Personal-Testing-System/Models/LoginModel.cs):
```
{
    "Login": "admin",
    "Password": "password"
}
```
- Response -> [AdminDto](Personal-Testing-System/DTOs/AdminDto.cs) + string(TokenAdmin):
```
{
  "TokenAdmin": "xxxxxxxxxxxxxxxx",
  "Admin": {
    "Id": "xxxxxxxxxxxxxxxxxx",
    "FirstName": "Евгений",
    "SecondName": "Жма",
    "LastName": "Дворцов",
    "Login": "admin",
    "Password": "password",
    "IdSubdivision": 1
  }
}
```
2. GET /admin-api/GetSubdivisions.
- Request - Authorization.
- Response -> list of [SubdivisionDto](Personal-Testing-System/DTOs/SubdivisionDto.cs):
```
[
  {
    "Id": 1,
    "Name": "Отдел кадров"
  },
  {
    "Id": 2,
    "Name": "Инженерный цех"
  }
]
```
3. POST /admin-api/AddSubdivision.
- Request <- Authorization, [SubdivisionModel](Personal-Testing-System/Models/SubdivisionModel.cs):
```
{
    "Name" : "Финансовый отдел"
}
```
- Response -> status message.
4. POST /admin-api/UpdateSubdivision.
- Request <- Authorization, [AddSubdivisionModel](Personal-Testing-System/Models/AddSubdivisionModel.cs):
```
{
    "Id" : 7,
    "Name" : "Финансовый отдел."
}
```
- Response -> status message.
5. POST /admin-api/DeleteSubdivision.
- Request <- Authorization, [IntIdModel](Personal-Testing-System/Models/IntIdModel.cs):
```
{
    "Id" : 7
}
```
- Response -> status message.
6. GET /admin-api/GetEmployees.
- Request - Authorization.
- Response -> list of [EmployeeModels](Personal-Testing-System/Models/EmployeeModel.cs).
```
[
  {
    "Id": "xxxxxxxxxxxxxxxxxx",
    "FirstName": "Андрей",
    "SecondName": "Кравцов",
    "LastName": "Нежеленко",
    "Login": "login",
    "Password": "password",
    "DateOfBirth": "01.01.2000",
    "Subdivision": {
      "Id": 2,
      "Name": "Инженерный цех"
    }
  }
]
```
7. GET /user-api/GetEmployee.
- Request <- Authorization, [StringIdModel]((Personal-Testing-System/Models/StringIdModel.cs)).
```
{
    "Id" : "xxxxxxxxxxxxxxxxxxxx"
}
```
- Response -> [EmployeeModel](Personal-Testing-System/Models/EmployeeModel.cs).
```
  {
    "Id": "xxxxxxxxxxxxxxxxxx",
    "FirstName": "Андрей",
    "SecondName": "Кравцов",
    "LastName": "Нежеленко",
    "Login": "login",
    "Password": "password",
    "DateOfBirth": "01.01.2000",
    "Subdivision": {
      "Id": 2,
      "Name": "Инженерный цех"
    }
  }
```
8. POST /user-api/AddEmployee.
- Request <- Authorization, [AddEmployeeModel](Personal-Testing-System/Models/Employeemodel.cs).
```
{
    "FirstName": "Водила",
    "SecondName": "Водила",
    "LastName": "Водила",
    "Login": "vodila",
    "Password": "password",
    "DateOfBirth": "24.07.1985",
    "IdSubdivision": "1"
}
```
- Response -> status message.
9. POST /admin-api/UpdateEmployee.
- Request <- Authorization, [EmployeeDto](Personal-Testing-System/DTOs/EmployeeDto.cs).
```
{
    "Id": "2722684a-d6c7-4ebc-9f46-dc66ed43d5af",
    "FirstName": "Водила1",
    "SecondName": "Водила1",
    "LastName": "Водила1",
    "Login": "vodila",
    "Password": "password",
    "DateOfBirth": "24.07.1985",
    "IdSubdivision": "7"
}
```
- Response -> status message.
10. POST /admin-api/DeleteEmployee.
- Request <- Authorization, [StringIdModel](Personal-Testing-System/Models/StringIdModel.cs).
```
{
    "Id" : "xxxxxxxxxxxxxxxxxxxx"
}
```
- Output -> stautus message.
11. GET /admin-api/GetCompetences.
- Request - Authoriztion.
- Response -> list of [CompetenceDto](Personal-Testing-System/DTOs/CompetenceDto.cs):
```
[
  {
    "Id": 1,
    "Name": "Оценка имеющихся компетенций"
  }
]

```
12. GET /admin-api/GetCompetence.
- Request <- Authorization, [IntIdModel](Personal-Testing-System/Models/IntIdModel.cs).
```
  {
    "Id": 1
  }
```
- Response -> [CompetenceDto](Personal-Testing-System/DTOs/CompetenceDto.cs).
```
  {
    "Id": 1,
    "Name": "Оценка имеющихся компетенций"
  }
```
13. POST /admin-api/AddCompetence.
- Request <- Autorization, [AddCompetenceModel](Personal-Testing-System/Models/AddCompetenceModel.cs):
```
{
    "Name" : "Проверка квалификации"
}
```
- Response -> status message.
14. POST /admin-api/UpdateCompetence.
- Request <- [CopmpetenceDto](Personal-Testing-System/DTOs/CompetenceDto.cs).
```
{
    "Id" : 3,
    "Name" : "Проверка квалификации."
}
```
- Response -> status message.
15. POST /admin-api/DeleteCompetence.
- Request <- Authorization, I[ntIdModel](Personal-Testing-System/Models/IntIdModel.cs).
```
{
    "Id" : 3,
    "Name" : "Проверка квалификации."
}
```
- Response -> status message.
16. GET /admin-api/GetTests.
- Request - Authorization.
- Response -> list of [GetTestModels](Personal-Testing-System/Models/GetTestModel.cs):
```
[
  {
    "Id": "02d3b2fa-f356-400a-ac20-90b5f358ea5d",
    "Name": "Тест для оценки школьных знаний",
    "Competence": {
      "Id": 1,
      "Name": "Оценка имеющихся компетенций"
    }
  },
  {
    "Id": "8dd4cca5-016f-4241-a9a7-0363c736d02f",
    "Name": "new Test1",
    "Competence": {
      "Id": 1,
      "Name": "Оценка имеющихся компетенций"
    }
  }
]
```
17. GET /admin-api/GetTest?id.
[TestModel](Personal-Testing-System/Models/TestModel.cs) имеет сложную структуру, так как существует несколько типов вопросов. [QuestionModel](Personal-Testing-System/Models/QuestionModel.cs) включает в себя List<Object>, который может содержать [AnswerModel](Personal-Testing-System/Models/AnswerModel.cs), [SubsequenceDto](Personal-Testing-System/DTOs/SubsequenceDto.cs), [FirstPartDto](Personal-Testing-System/DTOs/FirstPartDto.cs), [SecondPartDto](Personal-Testing-System/DTOs/SecondPartDto.cs).
- Request <- Aythorization, [StringIdModel](Personal-Testing-System/Models/StringIdModel.cs):
```
{
    "Id" : "xxxxx"
}
```
- Response -> [TestModel](Personal-Testing-System/Models/TestModel.cs):
```
{
  "Id": "02d3b2fa-f356-400a-ac20-90b5f358ea5d",
  "Name": "Тест для оценки школьных знаний",
  "Competence": {
    "Id": 1,
    "Name": "Оценка имеющихся компетенций"
  },
  "Questions": [
    {
      "Id": "02adb7d6-ad30-4a7d-bbd4-6463b8089a70",
      "Text": "Переведите слова с английского языка и соедините с правильным вариантом:",
      "ImagePath": null,
      "Base64Image": null,
      "IdQuestionType": 3,
      "Answers": [
        {
          "IdFirstPart": "0d9855bc-e833-4e13-ad72-b8bd1541afcb",
          "Text": "Motherhood",
          "IdQuestion": "02adb7d6-ad30-4a7d-bbd4-6463b8089a70"
        },
        {
          "IdFirstPart": "3fe5113f-f0d6-41a6-bfd7-4c8a011887c3",
          "Text": "Remote relatives",
          "IdQuestion": "02adb7d6-ad30-4a7d-bbd4-6463b8089a70"
        },
        {
          "IdFirstPart": "644078bb-05c2-44db-b330-8dabe350f492",
          "Text": "Kind-hearted",
          "IdQuestion": "02adb7d6-ad30-4a7d-bbd4-6463b8089a70"
        },
        {
          "IdSecondPart": 1,
          "Text": "Родина",
          "IdFirstPart": "0d9855bc-e833-4e13-ad72-b8bd1541afcb"
        },
        {
          "IdSecondPart": 2,
          "Text": "Дальние родственники",
          "IdFirstPart": "3fe5113f-f0d6-41a6-bfd7-4c8a011887c3"
        },
        {
          "IdSecondPart": 3,
          "Text": "Добродушный",
          "IdFirstPart": "644078bb-05c2-44db-b330-8dabe350f492"
        }
      ]
    },
    {
      "Id": "647113ea-5c83-465e-8d63-fb311f377d4b",
      "Text": "Составте последовательность действий при чп на рабочем месте расставляя варианты в нужном порядке:",
      "ImagePath": null,
      "Base64Image": null,
      "IdQuestionType": 4,
      "Answers": [
        {
          "IdSubsequence": 1,
          "Text": "Выключить станок/компьютер и устранить последствия",
          "IdQuestion": "647113ea-5c83-465e-8d63-fb311f377d4b",
          "Number": 2
        },
        {
          "IdSubsequence": 2,
          "Text": "Разбить стекло шкафа с аптечкой и подхилиться",
          "IdQuestion": "647113ea-5c83-465e-8d63-fb311f377d4b",
          "Number": 1
        },
        {
          "IdSubsequence": 3,
          "Text": "Сообщить начальнику о чп",
          "IdQuestion": "647113ea-5c83-465e-8d63-fb311f377d4b",
          "Number": 3
        }
      ]
    },
    {
      "Id": "c91631c2-e518-41f0-9761-0ee2a9669f59",
      "Text": "Выберите правильное кол-во букв в русском алфавите:",
      "ImagePath": null,
      "Base64Image": null,
      "IdQuestionType": 1,
      "Answers": [
        {
          "IdAnswer": 1,
          "Text": "32",
          "IdQuestion": "c91631c2-e518-41f0-9761-0ee2a9669f59",
          "Correct": false,
          "Base64Image": null,
          "ImagePath": null
        },
        {
          "IdAnswer": 2,
          "Text": "34",
          "IdQuestion": "c91631c2-e518-41f0-9761-0ee2a9669f59",
          "Correct": false,
          "Base64Image": null,
          "ImagePath": null
        },
        {
          "IdAnswer": 3,
          "Text": "33",
          "IdQuestion": "c91631c2-e518-41f0-9761-0ee2a9669f59",
          "Correct": true,
          "Base64Image": null,
          "ImagePath": null
        }
      ]
    },
    {
      "Id": "d567b54c-9028-4051-a18f-09fad7b9c9d6",
      "Text": "Сумма чисел равная 8 в следующих вариантах:",
      "ImagePath": null,
      "Base64Image": null,
      "IdQuestionType": 2,
      "Answers": [
        {
          "IdAnswer": 4,
          "Text": "3+5",
          "IdQuestion": "d567b54c-9028-4051-a18f-09fad7b9c9d6",
          "Correct": true,
          "Base64Image": null,
          "ImagePath": null
        },
        {
          "IdAnswer": 5,
          "Text": "6+2",
          "IdQuestion": "d567b54c-9028-4051-a18f-09fad7b9c9d6",
          "Correct": true,
          "Base64Image": null,
          "ImagePath": null
        },
        {
          "IdAnswer": 6,
          "Text": "3+3",
          "IdQuestion": "d567b54c-9028-4051-a18f-09fad7b9c9d6",
          "Correct": false,
          "Base64Image": null,
          "ImagePath": null
        }
      ]
    }
  ]
}
```
18. POST https://localhost:7273/user-api/AddTest.
[AddTestPostModel](Personal-Testing-System/Models/AddPostTestModel.cs) имеет сложную структуру, так как существует несколько типов вопросов. [QuestionModel](Personal-Testing-System/Models/QuestionModel.cs) включает в себя List<Object>, который может содержать [AnswerDto](Personal-Testing-System/DTOs/AnswerDto.cs), [SubsequenceDto](Personal-Testing-System/DTOs/SubsequenceDto.cs), [FirstSecondPartDto](Personal-Testing-System/DTOs/FirstSecondPartDto.cs).
- Request <- FormData contains files ans [AddTestPostModel](Personal-Testing-System/Models/AddPostTestModel.cs):
```
files    /D:/Wallpapers/16220688790371.jpg
Test
{
  "Name": "sample Test",
  "Weight" : 22,
  "CompetenceId" : 1,
  "Description" : "sample Description",
  "Instruction" : "sample Instruction",
  "IdTestType": 1,
  "Questions": [
    {
      "Text": "sample Question1",
      "IdQuestionType": 1,
      "Answers": [
        {
            "Text" : "Example answer1.1",
			"Weight" : 5,
            "Correct" : true
        },
        {
            "Text" : "Example answer1.2",
			"Weight" : 5,
            "Correct" : false
        },
        {
            "Text" : "Example answer1.3",
			"Weight" : 5,
            "Correct" : false
        },
        {
            "Text" : "Example answer1.4",
			"Weight" : 5,
            "Correct" : false
        }
      ]
    },
    {
      "Text": "Example question2?",
      "IdQuestionType": 2,
      "Answers": [
        {
            "Text" : "Example answer2.1",
			"Weight" : 5,
            "Correct" : true
        },
        {
            "Text" : "Example answer2.2",
			"Weight" : 5,
            "Correct" : true
        },
        {
            "Text" : "Example answer2.3",
			"Weight" : 5,
            "Correct" : true
        },
        {
            "Text" : "Example answer2.4",
			"Weight" : 5,
            "Correct" : true
        }
      ]
    },
    {
      "Text": "Example question3?",
      "IdQuestionType": 3,
      "Answers": [
        {
            "FirstPartText" : "Example answer3.1.1",
            "SecondPartText" : "Example answer3.1.2"
        },
        {
            "FirstPartText" : "Example answer3.2.1",
            "SecondPartText" : "Example answer3.2.2"
        },
        {
            "FirstPartText" : "Example answer3.3.1",
            "SecondPartText" : "Example answer3.3.2"
        },
        {
            "FirstPartText" : "Example answer3.4.1",
            "SecondPartText" : "Example answer3.4.2"
        }
      ]
    },
    {
      "Text": "Example question4?",
      "IdQuestionType": 4,
      "Answers": [
        {
            "Text" : "Example first",
            "Number" : 1
        },
        {
            "Text" : "Example second",
            "Number" : 3
        },
        {
            "Text" : "Example third",
            "Number" : 4
        },
        {
            "Text" : "Example fourth",
            "Number" : 2
        }
      ]
    }
  ]
}
```
- Response -> status message.
19. Post https://localhost:7273/admin-api/GetPdfTest.
- Request - Authorization, [StringIdModel](Personal-Testing-System/Models/StringIdModel.cs):
```
{
    "Id" : "xxxxx"
}
```
- Response - content-type: application/pdf: 
20. Post https://localhost:7273/admin-api/GetPdfCorrectTest.
- Request - Authorization, [StringIdModel](Personal-Testing-System/Models/StringIdModel.cs):
```
{
    "Id" : "xxxxx"
}
```
- Response - content-type: application/pdf: 
21. POST https://localhost:7273/admin-api/UpdateTest.
[UpdateTestPostModel](Personal-Testing-System/Models/UpdatePostTestModel.cs) имеет сложную структуру, так как существует несколько типов вопросов. [QuestionModel](Personal-Testing-System/Models/QuestionModel.cs) включает в себя List<Object>, который может содержать [AnswerDto](Personal-Testing-System/DTOs/AnswerDto.cs), [SubsequenceDto](Personal-Testing-System/DTOs/SubsequenceDto.cs), [FirstPartDto](Personal-Testing-System/DTOs/FirstPartDto.cs), [SecondPartDto](Personal-Testing-System/DTOs/SecondPartDto.cs), [FirstSecondPartDto](Personal-Testing-System/DTOs/FirstSecondPartDto.cs).
- Request <- FormData contains files and [UpdateTestPostModel](Personal-Testing-System/Models/UpdateTestPostModel.cs):
```
files    /D:/Wallpapers/16220688790371.jpg
Test    
{
    "Id": "a736b797-19a6-4828-8a7c-e01ca0133be1",
    "Name": "First Test",
    "CompetenceId": 1,
    "Questions": [
        {
            "Id": "73bd8680-5ac9-44a9-92b8-f3469fc224a1",
            "Text": "Example question2?",
            "ImagePath": null,
            "IdQuestionType": 2,
            "Answers": [
                {
                    "IdAnswer": 11,
                    "Text": "Example answer2.1",
                    "IdQuestion": "73bd8680-5ac9-44a9-92b8-f3469fc224a1",
                    "Correct": true,
                    "ImagePath": null
                },
                {
                    "IdAnswer": 12,
                    "Text": "Example answer2.2",
                    "IdQuestion": "73bd8680-5ac9-44a9-92b8-f3469fc224a1",
                    "Correct": true,
                    "ImagePath": null
                }
            ]
        },
        {
            "Id": "7f8ece2e-8c0a-477b-b62d-bff61a097282",
            "Text": "Example question4?",
            "ImagePath": null,
            "IdQuestionType": 4,
            "Answers": [
                {
                    "IdSubsequence": 7,
                    "Text": "Example first",
                    "IdQuestion": "7f8ece2e-8c0a-477b-b62d-bff61a097282",
                    "Number": 1
                },
                {
                    "IdSubsequence": 8,
                    "Text": "Example second",
                    "IdQuestion": "7f8ece2e-8c0a-477b-b62d-bff61a097282",
                    "Number": 2
                },
                {
                    "IdSubsequence": 9,
                    "Text": "Example third",
                    "IdQuestion": "7f8ece2e-8c0a-477b-b62d-bff61a097282",
                    "Number": 3
                }
            ]
        },
        {
            "Id": "a252feaa-1ae7-4004-995b-44b95ed86c24",
            "Text": "Example question3?",
            "ImagePath": null,
            "IdQuestionType": 3,
            "Answers": [
                {
                    "IdFirstPart": "2602287c-1abf-46a4-bbd8-583ea30ff588",
                    "Text": "Example answer3.1",
                    "IdQuestion": "a252feaa-1ae7-4004-995b-44b95ed86c24"
                },
                {
                    "IdFirstPart": "334cbf82-ddbe-479e-8166-a6934a91e7f0",
                    "Text": "Example answer3.2",
                    "IdQuestion": "a252feaa-1ae7-4004-995b-44b95ed86c24"
                },
                {
                    "IdSecondPart": 5,
                    "Text": "Example answer3.1",
                    "IdFirstPart": "2602287c-1abf-46a4-bbd8-583ea30ff588"
                },
                {
                    "IdSecondPart": 6,
                    "Text": "Example answer3.2",
                    "IdFirstPart": "334cbf82-ddbe-479e-8166-a6934a91e7f0"
                }
            ]
        },
        {
            "Id": "d4c6b3d8-89e3-47ab-b03d-4711a632bcff",
            "Text": "Example Question1?",
            "ImagePath": "97356a1b-d341-4c07-bfed-206ca3e659e4.jpg",
            "IdQuestionType": 1,
            "Answers": [
                {
                    "IdAnswer": 9,
                    "Text": "Example answer1.1",
                    "IdQuestion": "d4c6b3d8-89e3-47ab-b03d-4711a632bcff",
                    "Correct": true,
                    "ImagePath": "60289b36-4269-4d00-b8c2-39f8dc436d38.jpg"
                },
                {
                    "IdAnswer": 10,
                    "Text": "Example answer1.2",
                    "IdQuestion": "d4c6b3d8-89e3-47ab-b03d-4711a632bcff",
                    "Correct": false,
                    "ImagePath": "91847a30-10c2-4d9e-a7dd-e9457e42f0d2.jpg"
                }
            ]
        }
    ]
}
```
- Response -> :
```

```
22. POST https://localhost:7273/admin-api/DeleteTest.
- Request <- Authourization, [StringIdModel](Personal-Testing-System/Models/StringIdModel.cs):
```
{
    "Id" : "xxxxx"
}
```
- Response -> status message.

23. GET https://localhost:7273/admin-api/GetPurposes.
- Request - Authorization.
- Response -> list of [PurposeAdminModels](Personal-Testing-System/Models/PurposeAdminModel.cs):
```
[
  {
    "Id": 2,
    "Employee": {
      "Id": "48f04c54-d4b3-4889-9f3b-2d6098a6d3de",
      "FirstName": "Андрей",
      "SecondName": "Кравцов",
      "LastName": "Нежеленко",
      "Login": "login",
      "Password": "password",
      "DateOfBirth": "01.01.2000",
      "Subdivision": {
        "Id": 2,
        "Name": "Инженерный цех"
      }
    },
    "Test": {
      "Id": "02d3b2fa-f356-400a-ac20-90b5f358ea5d",
      "Name": "Тест для оценки школьных знаний",
      "Competence": {
        "Id": 1,
        "Name": "Оценка имеющихся компетенций"
      }
    },
    "DatatimePurpose": "08.08.2023 14:30:00"
  }
]
```
24. GET https://localhost:7273/admin-api/GetPurposesByEmployeeId.
- Request <- Authorization, [StringIdModel](Personal-Testing-System/Models/StringIdModel.cs).
```
{
    "Id" : "xxxxxxxxxxxxxx"
}
```
- Response -> list of [PurposeModels](Personal-Testing-System/Models/PurposeModel.cs).
```
[
  {
    "Id": 2,
    "Employee": {
      "Id": "48f04c54-d4b3-4889-9f3b-2d6098a6d3de",
      "FirstName": "Андрей",
      "SecondName": "Кравцов",
      "LastName": "Нежеленко",
      "Login": "login",
      "Password": "password",
      "DateOfBirth": "01.01.2000",
      "Subdivision": {
        "Id": 2,
        "Name": "Инженерный цех"
      }
    },
    "Test": {
      "Id": "02d3b2fa-f356-400a-ac20-90b5f358ea5d",
      "Name": "Тест для оценки школьных знаний",
      "Competence": {
        "Id": 1,
        "Name": "Оценка имеющихся компетенций"
      }
    },
    "DatatimePurpose": "08.08.2023 14:30:00"
  }
]
```
25. POST https://localhost:7273/admin-api/AddPurpose.
- Request <- Authorization, [AddTestPurposeModel](Personal-Testing-System/Models/AddTestPurposeModel.cs):
```
{
    "IdEmployee" : "8b69bcf7-a600-41d3-999d-ec3d76c72ec1",
    "IdTest" : "b0490b9d-8c07-4cd3-a1c5-cdce9b1cb33e",
    "DataTimePurpose" : "30.07.2023 14:00:00"
}
```
- Response -> :
```

```
26. POST https://localhost:7273/admin-api/AddPurposesBySubdivision.
- Request <- query params in url.
```
```
- Response -> :
```

```
27. POST https://localhost:7273/admin-api/UpdatePurpose.
- Request <- Authorization, [UpdateTestPurposeModel](Personal-Testing-System/Models/UpdatePurposeModel.cs).
```
{
    "Id" : 4,
    "IdEmployee" : "8b69bcf7-a600-41d3-999d-ec3d76c72ec1",
    "IdTest" : "b0490b9d-8c07-4cd3-a1c5-cdce9b1cb33e",
    "DataTimePurpose" : "26.07.2023 10:00:00"
}
```
- Response -> status message.
28. POST https://localhost:7273/admin-api/DeletePurpose.
- Request <- Authorization, [IntIdModel](Personal-Testing-System/Models/IntIdModel.cs).
```
{
    "Id" : 1
}
```
- Response -> status message.
29. POST https://localhost:7273/admin-api/DeleteResults.
- Request - Authorization.
30. GET https://localhost:7273/admin-api/GetResults.
- Input <- [ResultQuerryModel](Personal-Testing-System/Models/ResultQuerryModel.cs):
```
{
    "idSubdivision": "",
    "FirstName": "",
    "SecondName": "",
    "LastName": "",
    "Score": "",
    "SortType": ""
}
```
- Response -> list of [EmployeeResultModels](Personal-Testing-System/Models/EmployeeResultModel.cs).
```

```
