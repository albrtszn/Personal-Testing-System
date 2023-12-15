using Personal_Testing_System.Models;

namespace Personal_Testing_System.RateLogic
{
    public class RateLogic
    {
            // "Id": "ad3d986c-dfd8-4f68-436c-c0a5e4aa9723",
            // "Name": "Способность принимать решения ",

            //  "Id": "ad3d986c-dfd8-4f68-836c-1aa5e4aa9723",
            //  "Name": "Физико-механические",

            //  "Id": "ad3d986c-dfd8-4f68-836c-40a5e4aa9723",
            //  "Name": "Лидерские способности ",

            //  "Id": "ad3d986c-dfd8-4f68-836c-6aa4e4aa9723",
            //  "Name": "Инновационные",

            //    "Id": "ad3d986c-dfd8-4f68-836c-c015e4aa9723",
            //  "Name": "Ответственность",


            //  "Id": "ad3d986c-dfd8-4f68-836c-c0a5a8aa9723",
            //  "Name": "Личная организованность",

            //  "Id": "ad3d986c-dfd8-4f68-836c-c0a5e44a9723",
            //  "Name": "Креативность",

            //  "Id": "ad3d986c-dfd8-4f68-836c-c0a5e4aa9721",
            //  "Name": "Стратегическое мышление",



            //  "Id": "ad3d986c-dfd8-4f68-836c-c0a5e4aa9723",
            //  "Name": "Дисциплинированность",
            const string test_discipline = "ad3d986c-dfd8-4f68-836c-c0a5e4aa9723";

            //  "Id": "ad3d986c-dfd8-4f68-836c-c0a5e4aa9724",
            //  "Name": "Лояльность",
            const string test_loyalty = "ad3d986c-dfd8-4f68-836c-c0a5e4aa9724";

            //    "Id": "ad3d986c-dfd8-4f68-836c-c0a5e4aa972a",
            //    "Name": "Морально-этическая ответственность ",
            const string test_moral = "ad3d986c-dfd8-4f68-836c-c0a5e4aa972a";

            //    "Id": "ad3d986c-dfd8-4f68-836c-c0a5e4aa972b",
            //   "Name": "Командность",
            const string test_teamwork = "ad3d986c-dfd8-4f68-836c-c0a5e4aa972b";

            //    "Id": "ad3d986c-dfd8-4f68-836c-c0a5e4aa9744",
            //    "Name": "Ориентация на профессиональное развитие",
            const string test_evolution = "ad3d986c-dfd8-4f68-836c-c0a5e4aa9744";

            //    "Id": "ad3d986c-dfd8-4f68-836c-c0a5e7aa9723",
            //    "Name": "Организаторские способности ",

            //    "Id": "ad3d986c-dfd8-4f68-836c-c0a5e8aa6723",
            //    "Name": "Склонность к риску",

            //    "Id": "ad3d986c-dfd8-4f68-836c-c0a5e8aa9722",
            //    "Name": "Стратегическое мышление",
            const string test_thinking = "ad3d986c-dfd8-4f68-836c-c0a5e8aa9722";

            //    "Id": "ad3d986c-dfd8-4f68-836c-c0a5e8aa9723",
            //    "Name": "Стрессоустойчивость",
            const string test_sterss = "ad3d986c-dfd8-4f68-836c-c0a5e8aa9723";

            //    "Id": "ad3d986c-dfd8-4f68-836c-c745e4aa9723",
            //    "Name": "Адаптивность к изменениям",
            const string test_adaptability = "ad3d986c-dfd8-4f68-836c-c745e4aa9723";

            //    "Id": "ad3d986c-dfd8-4f68-836c-c7a5e4aa9723",
            //    "Name": "Экономические",

            //    "Id": "ad3d986c-dfd8-4f68-836c-ca45e4aa9723",
            //    "Name": "Производственные",


            //    "Id": "ad3d986c-dfd8-4f68-836c-caa4e4aa9723",
            //    "Name": "Технологические",
            const string test_tehno = "ad3d986c-dfd8-4f68-836c-caa4e4aa9723";

            //    "Id": "ad3d986c-dfd8-4f68-836c-caa5e4aa9723",
            //    "Name": "Эмоциональная уравновешенность",


            //    "Id": "ad3d986c-dfd8-4f68-836c-caa5e4aa9725",
            //    "Name": "Ориентация на достижения",
            const string test_achievements = "ad3d986c-dfd8-4f68-836c-caa5e4aa9725";

            //    "Id": "ad3d986c-dfd8-4f68-836c-caa5e4aa9742",
            //    "Name": "Коммуникативность",
            const string test_communication = "ad3d986c-dfd8-4f68-836c-caa5e4aa9742";


        //    "Id": "ar3d999c-dfd8-4f68-836c-1aa5e4aa9723",
        //    "Name": "Физико-механические",

        //    "Id": "d53e7264-8a39-3333-89bf-248bf7794cb3",
        //    "Name": "Химические",

        //    "Id": "d53e7264-8a39-4444-89bf-248bf7794cb3",
        //    "Name": "Химические",

        //    "Id": "d53e7264-8a39-47f9-89bf-241bf7794cb3",
        //    "Name": "Кратковременное запоминание ",

        //    "Id": "d53e7264-8a39-47f9-89bf-248bf7794cb3",
        //    "Name": "Концентрация внимания ",


        //                             AnswersUserTest\TestResultModel\EmployeeResultAnswersModel
        public static int GetPointTest(EmployeeResultAnswersModel ans, string idTest)
            {
                int res = 0;
                int kol_Q = 0;
                int kol_A = 0;
                bool tmp_num = false;

                switch (idTest)
                {
                    case test_thinking:
                    case test_evolution:
                    case test_achievements:
                    case test_communication:
                    case test_teamwork:
                    case test_adaptability:

                        kol_Q = ans.Questions.Count();
                        res = 0;
                        for (int i = 0; i < kol_Q; i++)
                        {
                            kol_A = ans.Questions[i].Answers.Count();
                            for (int j = 0; j < kol_A; j++)
                            {
                                if (((EmployeeAnswerModel)ans.Questions[i].Answers[j]).IsUserAnswer.Value)
                                {
                                    res = res + ((EmployeeAnswerModel)ans.Questions[i].Answers[j]).Weight.Value;
                                    break;
                                }
                            }
                        }
                        break;

                    case test_tehno:

                        kol_Q = ans.Questions.Count();
                        res = 0;
                        for (int i = 0; i < kol_Q; i++)
                        {
                            kol_A = ans.Questions[i].Answers.Count();
                            for (int j = 0; j < kol_A; j++)
                            {

                                if ( ((EmployeeAnswerModel)ans.Questions[i].Answers[j]).Correct == ((EmployeeAnswerModel)ans.Questions[i].Answers[j]).IsUserAnswer && ((EmployeeAnswerModel)ans.Questions[i].Answers[j]).Correct == true) 
                                {
                                    res = res + ((EmployeeAnswerModel)ans.Questions[i].Answers[j]).Weight.Value;
                                    break;
                                }
                            }
                        }

                        break;

                    case test_loyalty:
                        int[] numbers = { 1, 3, 4, 5, 11, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 27, 32, 34 };

                        kol_Q = ans.Questions.Count();
                        res = 0;
                        for (int i = 0; i < kol_Q; i++)
                        {
                            tmp_num = numbers.Contains(ans.Questions[i].Number.Value);
                            if (tmp_num == false)
                            {
                                continue;
                            }
                            kol_A = ans.Questions[i].Answers.Count();
                            for (int j = 0; j < kol_A; j++)
                            {
                                if (((EmployeeAnswerModel)ans.Questions[i].Answers[j]).IsUserAnswer == true)
                                {
                                    res = res + ((EmployeeAnswerModel)ans.Questions[i].Answers[j]).Weight.Value;
                                    break;
                                }
                            }
                        }

                        break;

                    case test_moral:
                        int[] numbers1_no = { 3, 9, 21, 27 };
                        int[] numbers1_da = { 15 };

                        int[] numbers2_no = { 2, 8, 14, 20, 26 };

                        int[] numbers3_no = { 4, 10, 16, 22, 28 };

                        int[] numbers4_no = { 1, 7, 13, 19, 25 };

                        int[] numbers5_no = { 5, 11, 17, 23, 29 };

                        int[] numbers6_da = { 6, 12, 18, 24, 30 };


                        kol_Q = ans.Questions.Count();
                        res = 0;
                        int res1 = 0;
                        int res2 = 0;
                        int res3 = 0;
                        int res4 = 0;
                        int res5 = 0;
                        int res6 = 0;

                        tmp_num = false;
                        for (int i = 0; i < kol_Q; i++)
                        {
                            tmp_num = numbers1_da.Contains(ans.Questions[i].Number.Value);
                            if (tmp_num == true)
                            {
                                if (((EmployeeAnswerModel)ans.Questions[i].Answers[0]).IsUserAnswer == true)
                                {
                                    res1 = res1 + 1;
                                }
                            }

                            tmp_num = numbers1_no.Contains(ans.Questions[i].Number.Value);
                            if (tmp_num == true)
                            {
                                if (((EmployeeAnswerModel)ans.Questions[i].Answers[1]).IsUserAnswer == true)
                                {
                                    res1 = res1 + 1;
                                }
                            }

                            tmp_num = numbers2_no.Contains(ans.Questions[i].Number.Value);
                            if (tmp_num == true)
                            {
                                if (((EmployeeAnswerModel)ans.Questions[i].Answers[1]).IsUserAnswer == true)
                                {
                                    res2 = res2 + 1;
                                }
                            }

                            tmp_num = numbers3_no.Contains(ans.Questions[i].Number.Value);
                            if (tmp_num == true)
                            {
                                if (((EmployeeAnswerModel)ans.Questions[i].Answers[1]).IsUserAnswer == true)
                                {
                                    res3 = res3 + 1;
                                }
                            }

                            tmp_num = numbers4_no.Contains(ans.Questions[i].Number.Value);
                            if (tmp_num == true)
                            {
                                if (((EmployeeAnswerModel)ans.Questions[i].Answers[1]).IsUserAnswer == true)
                                {
                                    res4 = res4 + 1;
                                }
                            }

                            tmp_num = numbers5_no.Contains(ans.Questions[i].Number.Value);
                            if (tmp_num == true)
                            {
                                if (((EmployeeAnswerModel)ans.Questions[i].Answers[1]).IsUserAnswer == true)
                                {
                                    res5 = res5 + 1;
                                }
                            }

                            tmp_num = numbers6_da.Contains(ans.Questions[i].Number.Value);
                            if (tmp_num == true)
                            {
                                if (((EmployeeAnswerModel)ans.Questions[i].Answers[0]).IsUserAnswer == true)
                                {
                                    res6 = res6 + 1;
                                }
                            }

                        }

                        res = res1 + res2 + res3 + res4 + res5 + res6;
                        break;

                    case test_sterss:
                        kol_Q = ans.Questions.Count();
                        res = 0;
                        for (int i = 0; i < kol_Q; i++)
                        {
                            kol_A = ans.Questions[i].Answers.Count();
                            for (int j = 0; j < kol_A; j++)
                            {
                                if (((EmployeeAnswerModel)ans.Questions[i].Answers[j]).IsUserAnswer == true)
                                {
                                    res = res + ((EmployeeAnswerModel)ans.Questions[i].Answers[j]).Weight.Value;
                                    break;
                                }
                            }
                        }
                        break;

                    default:
                        res = 0;
                        break;
                }






                return res;
            }


            public static string GetLevelTestPoit(string IdTest, int Point)
            {
                string res = string.Empty;

                switch (IdTest)
                {
                    case test_thinking:
                        if (Point >= 0 && Point <= 75)
                        {
                            res = "низкий";
                        }
                        else if (Point >= 76 && Point <= 124)
                        {
                            res = "средний";
                        }
                        else if (Point >= 125)
                        {
                            res = "высокий";
                        }
                        break;

                    case test_evolution:
                        if (Point >= 0 && Point <= 10)
                        {
                            res = "низкий";
                        }
                        else if (Point >= 11 && Point <= 16)
                        {
                            res = "средний";
                        }
                        else if (Point >= 17)
                        {
                            res = "высокий";
                        }
                        break;

                    case test_achievements:
                        if (Point >= 0 && Point <= 122)
                        {
                            res = "низкий";
                        }
                        else if (Point >= 123 && Point <= 204)
                        {
                            res = "средний";
                        }
                        else if (Point >= 205)
                        {
                            res = "высокий";
                        }
                        break;

                    case test_communication:
                        if (Point >= 0 && Point <= 45)
                        {
                            res = "низкий";
                        }
                        else if (Point >= 46 && Point <= 65)
                        {
                            res = "средний";
                        }
                        else if (Point >= 66)
                        {
                            res = "высокий";
                        }

                        break;

                    case test_teamwork:
                        if (Point >= 0 && Point <= 71)
                        {
                            res = "низкий";
                        }
                        else if (Point >= 72 && Point <= 168)
                        {
                            res = "средний";
                        }
                        else if (Point >= 169)
                        {
                            res = "высокий";
                        }
                        break;

                    case test_adaptability:

                        if (Point >= 0 && Point <= 67)
                        {
                            res = "низкий";
                        }
                        else if (Point >= 68 && Point <= 136)
                        {
                            res = "средний";
                        }
                        else if (Point >= 137)
                        {
                            res = "высокий";
                        }

                        break;

                    case test_tehno:

                        if (Point >= 0 && Point <= 6)
                        {
                            res = "низкий";
                        }
                        else if (Point >= 7 && Point <= 12)
                        {
                            res = "ниже среднего";
                        }
                        else if (Point >= 13 && Point <= 18)
                        {
                            res = "средний";
                        }
                        else if (Point >= 19 && Point <= 24)
                        {
                            res = "выше среднего";
                        }
                        else
                        {
                            res = "высокий";
                        }

                        break;

                    case test_loyalty:

                        if (Point >= 54)
                        {
                            res = "высокий";
                        }
                        else if (Point >= 18 && Point <= 53)
                        {
                            res = "выше среднего";
                        }
                        else if (Point >= -18 && Point <= 17)
                        {
                            res = "средний";
                        }
                        else if (Point >= -30 && Point <= -17)
                        {
                            res = "ниже среднего";
                        }
                        else
                        {
                            res = "низкий";
                        }

                        break;

                    case test_moral:

                        if (Point >= 21)
                        {
                            res = "высокий";
                        }
                        else if (Point >= 11 && Point <= 20)
                        {
                            res = "средний";
                        }
                        else
                        {
                            res = "низкий";
                        }

                        break;

                    case test_sterss:
                        if (Point >= 24)
                        {
                            res = "низкий";
                        }
                        else if (Point >= 14 && Point <= 23)
                        {
                            res = "ниже среднего";
                        }
                        else if (Point >= 7 && Point <= 13)
                        {
                            res = "средний";
                        }
                        else if (Point >= 3 && Point <= 6)
                        {
                            res = "выше среднего";
                        }
                        else if (Point >= 0 && Point <= 2)
                        {
                            res = "высокий";
                        }
                        else
                        {
                            res = "не определен";
                        }

                        break;

                    default:
                        res = "не определен";
                        break;
                }

                return res;
            }
    }
}
