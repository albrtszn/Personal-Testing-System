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
### /user-api
---
- POST /Login
Input <- LoginModel:
```
{
    "Login":"login",
    "Password":"password"
}
```
Output -> string:
```
{
    "EmployeeId":"xxxxxxxxxxxxxx..."
}
```
- GET /GetPurposesByEmployeeId?employeeId=d4359d8e-7bd9-4fae-b060-5633801f7a1a
Input <- value(employeeId) in url.
Output -> List<PurposeModel>:
```
[
    {
        "Id": 1,
        "IdEmployee": "2bf2df78-80dc-4539-be61-167bdd901b49",
        "Test": {
            "Id": "57e6acdc-1c76-4bf8-b815-cc136deff9ac",
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
- GET /GetTest?id=21fb8618-5277-474d-a759-08403a7f65e0
Input <- value(id of test) in url.
Output -> TestModel:
```
{
    "Id": "57e6acdc-1c76-4bf8-b815-cc136deff9ac",
    "Name": "Тест для оценки школьных знаний",
    "Competence": {
        "Id": 1,
        "Name": "Базовые знания"
    },
    "Questions": [
        {
            "Id": "288c27c1-bca7-4682-aa58-ce81b0186c4c",
            "Text": "Составте последовательность действий при чп на рабочем месте расставляя варианты в нужном порядке:",
            "IdQuestionType": 4,
            "Answers": [
                {
                    "Id": 1,
                    "Text": "Выключить станок/компьютер и устранить последствия",
                    "IdQuestion": "288c27c1-bca7-4682-aa58-ce81b0186c4c",
                    "Number": 2
                },
                {
                    "Id": 2,
                    "Text": "Разбить стекло шкафа с аптечкой и подхилиться",
                    "IdQuestion": "288c27c1-bca7-4682-aa58-ce81b0186c4c",
                    "Number": 1
                },
                {
                    "Id": 3,
                    "Text": "Сообщить начальнику о чп",
                    "IdQuestion": "288c27c1-bca7-4682-aa58-ce81b0186c4c",
                    "Number": 3
                }
            ]
        },
        {
            "Id": "468ae1fe-c2a4-45fd-8e38-bb431c51d8e1",
            "Text": "Сумма чисел равная 8 в следующих вариантах:",
            "IdQuestionType": 2,
            "Answers": [
                {
                    "Id": 4,
                    "Text": "3+5",
                    "IdQuestion": "468ae1fe-c2a4-45fd-8e38-bb431c51d8e1",
                    "Correct": true
                },
                {
                    "Id": 5,
                    "Text": "6+2",
                    "IdQuestion": "468ae1fe-c2a4-45fd-8e38-bb431c51d8e1",
                    "Correct": true
                },
                {
                    "Id": 6,
                    "Text": "3+3",
                    "IdQuestion": "468ae1fe-c2a4-45fd-8e38-bb431c51d8e1",
                    "Correct": false
                }
            ]
        },
        {
            "Id": "c37a74f3-6c02-48a7-9e13-744b2aad9114",
            "Text": "Переведите слова с английского языка и соедините с правильным вариантом:",
            "IdQuestionType": 3,
            "Answers": [
                {
                    "Id": "07e96419-cbc6-4b80-bebb-96c3db6883cb",
                    "Text": "Kind-hearted",
                    "IdQuestion": "c37a74f3-6c02-48a7-9e13-744b2aad9114"
                },
                {
                    "Id": "753f6352-89aa-4f02-a977-607ad65d56b0",
                    "Text": "Motherhood",
                    "IdQuestion": "c37a74f3-6c02-48a7-9e13-744b2aad9114"
                },
                {
                    "Id": "c677f824-f7cc-4ecd-8ebe-0fa9446a53c0",
                    "Text": "Remote relatives",
                    "IdQuestion": "c37a74f3-6c02-48a7-9e13-744b2aad9114"
                },
                {
                    "Id": 3,
                    "Text": "Добродушный",
                    "IdFirstPart": "07e96419-cbc6-4b80-bebb-96c3db6883cb"
                },
                {
                    "Id": 1,
                    "Text": "Родина",
                    "IdFirstPart": "753f6352-89aa-4f02-a977-607ad65d56b0"
                },
                {
                    "Id": 2,
                    "Text": "Дальние родственники",
                    "IdFirstPart": "c677f824-f7cc-4ecd-8ebe-0fa9446a53c0"
                }
            ]
        },
        {
            "Id": "eba3f532-1a01-4ecd-89fe-5aacc8a5a59d",
            "Text": "Выберите правильное кол-во букв в русском алфавите:",
            "IdQuestionType": 1,
            "Answers": [
                {
                    "Id": 1,
                    "Text": "32",
                    "IdQuestion": "eba3f532-1a01-4ecd-89fe-5aacc8a5a59d",
                    "Correct": false
                },
                {
                    "Id": 2,
                    "Text": "34",
                    "IdQuestion": "eba3f532-1a01-4ecd-89fe-5aacc8a5a59d",
                    "Correct": false
                },
                {
                    "Id": 3,
                    "Text": "33",
                    "IdQuestion": "eba3f532-1a01-4ecd-89fe-5aacc8a5a59d",
                    "Correct": true
                }
            ]
        }
    ]
}
```
- POST /PushTest
Input <- TestResultModel.
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
Output -> string(score):
```
{
    "message": "Тест выполнен. Оценка: 3"
}
```
