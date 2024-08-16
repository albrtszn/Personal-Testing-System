using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Remoting.Messaging;
using System.Windows;

namespace Client
{
    public class ConnectHost
    {
        public static string urlHost = Properties.Settings.Default.hostUrl;
        public static string proсHost = Properties.Settings.Default.protocol;
        public static string token = "";
        public static int userRole = 1;

        private enum connMetod{
            GET = 1, POST = 2
        };

        public ConnectHost() 
        {
            
        }

        public async Task<JToken> Login(string pay_data)
        {
            Payload data_payload = new Payload();
            if (userRole == 1)
            {
                data_payload.uri = proсHost + "://" + urlHost + "/user-api/Login";
            }
            else
            {
                data_payload.uri = proсHost + "://" + urlHost + "/admin-api/Login";
            }
            data_payload.payload = pay_data;
            data_payload.metod = (int)connMetod.POST;
            data_payload.token = token;
            var jObject = await ProcessRequest(data_payload);
            return jObject;
        }

        public async Task<JToken> AddSubdivision(string pay_data)
        {
            Payload data_payload = new Payload();
            if (userRole == 1)
            {
                data_payload.uri = proсHost + "://" + urlHost + "/admin-api/AddSubdivision";
            }
            else
            {
                data_payload.uri = proсHost + "://" + urlHost + "/admin-api/AddSubdivision";
            }
            data_payload.payload = pay_data;
            data_payload.metod = (int)connMetod.POST;
            data_payload.token = token;
            var jObject = await ProcessRequest(data_payload);
            return jObject;
        }

    
        public async Task<JToken> GetCompetenceScoresByGroupId(string pay_data)
        {
            Payload data_payload = new Payload();
            if (userRole == 1)
            {
                data_payload.uri = proсHost + "://" + urlHost + "/admin-api/GetCompetenceScoresByGroupId";
            }
            else
            {
                data_payload.uri = proсHost + "://" + urlHost + "/admin-api/GetCompetenceScoresByGroupId";
            }
            data_payload.payload = pay_data;
            data_payload.metod = (int)connMetod.POST;
            data_payload.token = token;
            var jObject = await ProcessRequest(data_payload);
            return jObject;
        }

        public async Task<JToken> GetResultsOfPurposesByEmployeeId(string pay_data)
        {
            Payload data_payload = new Payload();
            if (userRole == 1)
            {
                data_payload.uri = proсHost + "://" + urlHost + "/admin-api/GetResultsOfPurposesByEmployeeId";
            }
            else
            {
                data_payload.uri = proсHost + "://" + urlHost + "/admin-api/GetResultsOfPurposesByEmployeeId";
            }
            data_payload.payload = pay_data;
            data_payload.metod = (int)connMetod.POST;
            data_payload.token = token;
            var jObject = await ProcessRequest(data_payload);
            return jObject;
        }

        public async Task<JToken> GetEmployeesBySubdivisionId(string pay_data)
        {
            Payload data_payload = new Payload();
            if (userRole == 1)
            {
                data_payload.uri = proсHost + "://" + urlHost + "/admin-api/GetEmployeesBySubdivisionId";
            }
            else
            {
                data_payload.uri = proсHost + "://" + urlHost + "/admin-api/GetEmployeesBySubdivisionId";
            }
            data_payload.payload = pay_data;
            data_payload.metod = (int)connMetod.POST;
            data_payload.token = token;
            var jObject = await ProcessRequest(data_payload);
            return jObject;
        }
        

        public async Task<JToken> UpdateCompetenceCoeff(string pay_data)
        {
            Payload data_payload = new Payload();
            if (userRole == 1)
            {
                data_payload.uri = proсHost + "://" + urlHost + "/admin-api/UpdateCompetenceCoeff";
            }
            else
            {
                data_payload.uri = proсHost + "://" + urlHost + "/admin-api/UpdateCompetenceCoeff";
            }
            data_payload.payload = pay_data;
            data_payload.metod = (int)connMetod.POST;
            data_payload.token = token;
            var jObject = await ProcessRequest(data_payload);
            return jObject;
        }
        

        public async Task<JToken> DeleteQuestionInTest(string pay_data)
        {
            Payload data_payload = new Payload();
            if (userRole == 1)
            {
                data_payload.uri = proсHost + "://" + urlHost + "/admin-api/DeleteQuestionInTest";
            }
            else
            {
                data_payload.uri = proсHost + "://" + urlHost + "/admin-api/DeleteQuestionInTest";
            }
            data_payload.payload = pay_data;
            data_payload.metod = (int)connMetod.POST;
            data_payload.token = token;
            var jObject = await ProcessRequest(data_payload);
            return jObject;
        }

       
        public async Task<byte[]> GetWordTest(string pay_data)
        {
            Payload data_payload = new Payload();
            if (userRole == 1)
            {
                data_payload.uri = proсHost + "://" + urlHost + "/admin-api/GetWordTest";
            }
            else
            {
                data_payload.uri = proсHost + "://" + urlHost + "/admin-api/GetWordTest";
            }
            data_payload.payload = pay_data;
            data_payload.metod = (int)connMetod.POST;
            data_payload.token = token;
            var jObject = await ProcessRequestFile(data_payload);
            return jObject;
        }

        public async Task<byte[]> GetPdfTest(string pay_data)
        {
            Payload data_payload = new Payload();
            if (userRole == 1)
            {
                data_payload.uri = proсHost + "://" + urlHost + "/admin-api/GetPdfTest";
            }
            else
            {
                data_payload.uri = proсHost + "://" + urlHost + "/admin-api/GetPdfTest";
            }
            data_payload.payload = pay_data;
            data_payload.metod = (int)connMetod.POST;
            data_payload.token = token;
            var jObject = await ProcessRequestFile(data_payload);
            return jObject;
        }


        public async Task<JToken> DeleteEmployee(string pay_data)
        {
            Payload data_payload = new Payload();
            if (userRole == 1)
            {
                data_payload.uri = proсHost + "://" + urlHost + "/admin-api/DeleteEmployee";
            }
            else
            {
                data_payload.uri = proсHost + "://" + urlHost + "/admin-api/DeleteEmployee";
            }
            data_payload.payload = pay_data;
            data_payload.metod = (int)connMetod.POST;
            data_payload.token = token;
            var jObject = await ProcessRequest(data_payload);
            return jObject;
        }


        public async Task<JToken> AddPurpose(string pay_data)
        {
            Payload data_payload = new Payload();
            if (userRole == 1)
            {
                data_payload.uri = proсHost + "://" + urlHost + "/admin-api/AddPurpose";
            }
            else
            {
                data_payload.uri = proсHost + "://" + urlHost + "/admin-api/AddPurpose";
            }
            data_payload.payload = pay_data;
            data_payload.metod = (int)connMetod.POST;
            data_payload.token = token;
            var jObject = await ProcessRequest(data_payload);
            return jObject;
        }

        public async Task<JToken> GetPurposesByEmployeeId(string pay_data)
        {
            Payload data_payload = new Payload();
            if (userRole == 1)
            {
                data_payload.uri = proсHost + "://" + urlHost + "/user-api/GetPurposesByEmployeeId";
            }
            else
            {
                data_payload.uri = proсHost + "://" + urlHost + "/admin-api/GetPurposesByEmployeeId";
            }
            data_payload.payload = pay_data;
            data_payload.metod = (int)connMetod.POST;
            data_payload.token = token;
            var jObject = await ProcessRequest(data_payload);
            return jObject;
        }

        public async Task<JToken> GetTestResultsByEmployee(string pay_data)
        {
            Payload data_payload = new Payload();
            if (userRole == 1)
            {
                data_payload.uri = proсHost + "://" + urlHost + "/user-api/GetTestResultsByEmployee";
            }
            else
            {
                data_payload.uri = proсHost + "://" + urlHost + "/admin-api/GetTestResultsByEmployee";
            }
            data_payload.payload = pay_data;
            data_payload.metod = (int)connMetod.POST;
            data_payload.token = token;
            var jObject = await ProcessRequest(data_payload);
            return jObject;
        }
        

        public async Task<JToken> AddQuestionInTest(string pay_data, List<Payload_files> pay_Files)
        {
            Payload data_payload = new Payload();
            if (userRole == 1)
            {
                data_payload.uri = proсHost + "://" + urlHost + "/admin-api/AddQuestionInTest";
            }
            else
            {
                data_payload.uri = proсHost + "://" + urlHost + "/admin-api/AddQuestionInTest";
            }
            data_payload.payload = pay_data;
            data_payload.metod = (int)connMetod.POST;
            data_payload.token = token;
            var jObject = await ProcessRequestForm(data_payload, pay_Files);
            return jObject;
        }

        public async Task<JToken> UpdateQuestionInTest(string pay_data, List<Payload_files> pay_Files)
        {
            Payload data_payload = new Payload();
            if (userRole == 1)
            {
                data_payload.uri = proсHost + "://" + urlHost + "/admin-api/UpdateQuestionInTest";
            }
            else
            {
                data_payload.uri = proсHost + "://" + urlHost + "/admin-api/UpdateQuestionInTest";
            }
            data_payload.payload = pay_data;
            data_payload.metod = (int)connMetod.POST;
            data_payload.token = token;
            var jObject = await ProcessRequestForm(data_payload, pay_Files);
            return jObject;
        }

        public async Task<JToken> GetEmployee(string pay_data)
        {
            Payload data_payload = new Payload();
            if (userRole == 1)
            {
                data_payload.uri = proсHost + "://" + urlHost + "/user-api/GetEmployee";
            }
            else
            {
                data_payload.uri = proсHost + "://" + urlHost + "/admin-api/GetEmployee";
            }

            data_payload.payload = pay_data;
            data_payload.metod = (int)connMetod.POST;
            data_payload.token = token;
            var jObject = await ProcessRequest(data_payload);
            return jObject;
        }

        public async Task<JToken> GetTestsByEmployeeId(string pay_data)
        {
            Payload data_payload = new Payload();
            if (userRole == 1)
            {
                data_payload.uri = proсHost + "://" + urlHost + "/user-api/GetTestsByEmployeeId";
            }
            else
            {
                data_payload.uri = proсHost + "://" + urlHost + "/admin-api/GetTestsByEmployeeId";
            }

            data_payload.payload = pay_data;
            data_payload.metod = (int)connMetod.POST;
            data_payload.token = token;
            var jObject = await ProcessRequest(data_payload);
            return jObject;
        }

        public async Task<JToken> GetEmployeeResultAnswers(string pay_data)
        {
            Payload data_payload = new Payload();
            if (userRole == 1)
            {
                data_payload.uri = proсHost + "://" + urlHost + "/user-api/GetEmployeeResultAnswers";
            }
            else
            {
                data_payload.uri = proсHost + "://" + urlHost + "/admin-api/GetEmployeeResultAnswers";
            }

            data_payload.payload = pay_data;
            data_payload.metod = (int)connMetod.POST;
            data_payload.token = token;
            var jObject = await ProcessRequest(data_payload);
            return jObject;
        }

        public async Task<JToken> GetCompetenceCoeffs()
        {
            Payload data_payload = new Payload();
            if (userRole == 1)
            {
                data_payload.uri = proсHost + "://" + urlHost + "/admin-api/GetCompetenceCoeffs";
            }
            else
            {
                data_payload.uri = proсHost + "://" + urlHost + "/admin-api/GetCompetenceCoeffs";
            }

            data_payload.payload = "";
            data_payload.metod = (int)connMetod.GET;
            data_payload.token = token;
            var jObject = await ProcessRequest(data_payload);
            return jObject;
        }

        public async Task<JToken> GetResults()
        {
            Payload data_payload = new Payload();
            if (userRole == 1)
            {
                data_payload.uri = proсHost + "://" + urlHost + "/user-api/GetResults";
            }
            else
            {
                data_payload.uri = proсHost + "://" + urlHost + "/admin-api/GetResults";
            }

            data_payload.payload = "";
            data_payload.metod = (int)connMetod.POST;
            data_payload.token = token;
            var jObject = await ProcessRequest(data_payload);
            return jObject;
        }


        public async Task<JToken> GetResultsByEmployee(string pay_data)
        {
            Payload data_payload = new Payload();
            if (userRole == 1)
            {
                data_payload.uri = proсHost + "://" + urlHost + "/user-api/GetResultsByEmployee";
            }
            else
            {
                data_payload.uri = proсHost + "://" + urlHost + "/admin-api/GetResultsByEmployee";
            }

            data_payload.payload = pay_data;
            data_payload.metod = (int)connMetod.POST;
            data_payload.token = token;
            var jObject = await ProcessRequest(data_payload);
            return jObject;
        }

        public async Task<JToken> AddEmployee(string pay_data)
        {
            Payload data_payload = new Payload();
            if (userRole == 1)
            {
                data_payload.uri = proсHost + "://" + urlHost + "/admin-api/AddEmployee";
            }
            else
            {
                data_payload.uri = proсHost + "://" + urlHost + "/admin-api/AddEmployee";
            }
            data_payload.payload = pay_data;
            data_payload.metod = (int)connMetod.POST;
            data_payload.token = token;
            var jObject = await ProcessRequest(data_payload);
            return jObject;
        }
        
        public async Task<JToken> GetResultsBySubdivision(string pay_data)
        {
            Payload data_payload = new Payload();
            if (userRole == 1)
            {
                data_payload.uri = proсHost + "://" + urlHost + "/admin-api/GetResultsBySubdivision";
            }
            else
            {
                data_payload.uri = proсHost + "://" + urlHost + "/admin-api/GetResultsBySubdivision";
            }
            data_payload.payload = pay_data;
            data_payload.metod = (int)connMetod.POST;
            data_payload.token = token;
            var jObject = await ProcessRequest(data_payload);
            return jObject;
        }

        public async Task<JToken> AddAdmin(string pay_data)
        {
            Payload data_payload = new Payload();
            if (userRole == 1)
            {
                data_payload.uri = proсHost + "://" + urlHost + "/admin-api/AddAdmin";
            }
            else
            {
                data_payload.uri = proсHost + "://" + urlHost + "/admin-api/AddAdmin";
            }
            data_payload.payload = pay_data;
            data_payload.metod = (int)connMetod.POST;
            data_payload.token = token;
            var jObject = await ProcessRequest(data_payload);
            return jObject;
        }

        public async Task<JToken> UpdateAdmin(string pay_data)
        {
            Payload data_payload = new Payload();
            if (userRole == 1)
            {
                data_payload.uri = proсHost + "://" + urlHost + "/admin-api/UpdateAdmin";
            }
            else
            {
                data_payload.uri = proсHost + "://" + urlHost + "/admin-api/UpdateAdmin";
            }
            data_payload.payload = pay_data;
            data_payload.metod = (int)connMetod.POST;
            data_payload.token = token;
            var jObject = await ProcessRequest(data_payload);
            return jObject;
        }
        
        
        public async Task<JToken> UpdateGlobalConfigure(string pay_data)
        {
            Payload data_payload = new Payload();
            if (userRole == 1)
            {
                data_payload.uri = proсHost + "://" + urlHost + "/user-api/UpdateGlobalConfigure";
            }
            else
            {
                data_payload.uri = proсHost + "://" + urlHost + "/admin-api/UpdateGlobalConfigure";
            }

            data_payload.payload = pay_data;
            data_payload.metod = (int)connMetod.POST;
            data_payload.token = token;
            var jObject = await ProcessRequest(data_payload);
            return jObject;
        }

        public async Task<JToken> GetGlobalConfigures()
        {
            Payload data_payload = new Payload();
            if (userRole == 1)
            {
                data_payload.uri = proсHost + "://" + urlHost + "/user-api/GetGlobalConfigures";
            }
            else
            {
                data_payload.uri = proсHost + "://" + urlHost + "/admin-api/GetGlobalConfigures";
            }

            data_payload.payload = "";
            data_payload.metod = (int)connMetod.GET;
            data_payload.token = token;
            var jObject = await ProcessRequest(data_payload);
            return jObject;
        }

        public async Task<JToken> GetAdmins()
        {
            Payload data_payload = new Payload();
            if (userRole == 1)
            {
                data_payload.uri = proсHost + "://" + urlHost + "/user-api/GetAdmins";
            }
            else
            {
                data_payload.uri = proсHost + "://" + urlHost + "/admin-api/GetAdmins";
            }

            data_payload.payload = "";
            data_payload.metod = (int)connMetod.GET;
            data_payload.token = token;
            var jObject = await ProcessRequest(data_payload);
            return jObject;
        }

        public async Task<JToken> GetEmployees()
        {
            Payload data_payload = new Payload();
            if (userRole == 1)
            {
                data_payload.uri = proсHost + "://" + urlHost + "/user-api/GetEmployees";
            }
            else
            {
                data_payload.uri = proсHost + "://" + urlHost + "/admin-api/GetEmployees";
            }

            data_payload.payload = "";
            data_payload.metod = (int)connMetod.GET;
            data_payload.token = token;
            var jObject = await ProcessRequest(data_payload);
            return jObject;
        }

        public async Task<JToken> GetTests()
        {
            Payload data_payload = new Payload();
            if (userRole == 1)
            {
                data_payload.uri = proсHost + "://" + urlHost + "/user-api/GetTests";
            }
            else
            {
                data_payload.uri = proсHost + "://" + urlHost + "/admin-api/GetTests";
            }

            data_payload.payload = "";
            data_payload.metod = (int)connMetod.GET;
            data_payload.token = token;
            var jObject = await ProcessRequest(data_payload);
            return jObject;
        }

        public async Task<JToken> GetTest(string pay_data)
        {
            Payload data_payload = new Payload();
            if (userRole == 1)
            {
                data_payload.uri = proсHost + "://" + urlHost + "/user-api/GetTest";
            }
            else
            {
                data_payload.uri = proсHost + "://" + urlHost + "/admin-api/GetTest";
            }

            data_payload.payload = pay_data;
            data_payload.metod = (int)connMetod.POST;
            data_payload.token = token;
            var jObject = await ProcessRequest(data_payload);
            return jObject;
        }

        public async Task<JToken> PushTest(string pay_data)
        {
            Payload data_payload = new Payload();
            if (userRole == 1)
            {
                data_payload.uri = proсHost + "://" + urlHost + "/user-api/PushTest";
            }
            else
            {
                data_payload.uri = proсHost + "://" + urlHost + "/admin-api/PushTest";
            }

            data_payload.payload = pay_data;
            data_payload.metod = (int)connMetod.POST;
            data_payload.token = token;
            var jObject = await ProcessRequest(data_payload);
            return jObject;
        }


        public async Task<JToken> AddMessage(string pay_data)
        {
            Payload data_payload = new Payload();
            if (userRole == 1)
            {
                data_payload.uri = proсHost + "://" + urlHost + "/user-api/AddMessage";
            }
            else
            {
                data_payload.uri = proсHost + "://" + urlHost + "/admin-api/AddMessage";
            }

            data_payload.payload = pay_data;
            data_payload.metod = (int)connMetod.POST;
            data_payload.token = token;
            var jObject = await ProcessRequest(data_payload);
            return jObject;
        }

        public async Task<JToken> UpdateEmployee(string pay_data)
        {
            Payload data_payload = new Payload();
            if (userRole == 1)
            {
                data_payload.uri = proсHost + "://" + urlHost + "/user-api/UpdateEmployee";
            }
            else
            {
                data_payload.uri = proсHost + "://" + urlHost + "/admin-api/UpdateEmployee";
            }

            data_payload.payload = pay_data;
            data_payload.metod = (int)connMetod.POST;
            data_payload.token = token;
            var jObject = await ProcessRequest(data_payload);
            return jObject;
        }

        
        public async Task<JToken> ChangeMesssageStatus(string pay_data)
        {
            Payload data_payload = new Payload();
            if (userRole == 1)
            {
                data_payload.uri = proсHost + "://" + urlHost + "/user-api/ChangeMesssageStatus";
            }
            else
            {
                data_payload.uri = proсHost + "://" + urlHost + "/admin-api/ChangeMesssageStatus";
            }

            data_payload.payload = pay_data;
            data_payload.metod = (int)connMetod.POST;
            data_payload.token = token;
            var jObject = await ProcessRequest(data_payload);
            return jObject;
        }

        public async Task<JToken> DeleteMesssage(string pay_data)
        {
            Payload data_payload = new Payload();
            if (userRole == 1)
            {
                data_payload.uri = proсHost + "://" + urlHost + "/user-api/DeleteMesssage";
            }
            else
            {
                data_payload.uri = proсHost + "://" + urlHost + "/admin-api/DeleteMesssage";
            }

            data_payload.payload = pay_data;
            data_payload.metod = (int)connMetod.POST;
            data_payload.token = token;
            var jObject = await ProcessRequest(data_payload);
            return jObject;
        }

        
        public async Task<JToken> GetLogsPage(int Page)
        {
            Payload data_payload = new Payload();
            if (userRole == 1)
            {
                data_payload.uri = proсHost + "://" + urlHost + "/user-api/GetLogsPage?PageNumber=1&ItemsPerPage=100";
            }
            else
            {
                data_payload.uri = proсHost + "://" + urlHost + "/admin-api/GetLogsPage?PageNumber=" + Page.ToString() + "&ItemsPerPage=100";
            }

            data_payload.payload = "";
            data_payload.metod = (int)connMetod.GET;
            data_payload.token = token;
            var jObject = await ProcessRequestHead(data_payload);
            return jObject;
        }

        public async Task<JToken> GetSubdivisions()
        {
            Payload data_payload = new Payload();
            if (userRole == 1)
            {
                data_payload.uri = proсHost + "://" + urlHost + "/user-api/GetSubdivisions";
            }
            else
            {
                data_payload.uri = proсHost + "://" + urlHost + "/admin-api/GetSubdivisions";
            }

            data_payload.payload = "";
            data_payload.metod = (int)connMetod.GET;
            data_payload.token = token;
            var jObject = await ProcessRequest(data_payload);
            return jObject;
        }

        public async Task<JToken> GetCompetences()
        {
            Payload data_payload = new Payload();
            if (userRole == 1)
            {
                data_payload.uri = proсHost + "://" + urlHost + "/user-api/GetCompetences";
            }
            else
            {
                data_payload.uri = proсHost + "://" + urlHost + "/admin-api/GetCompetences";
            }

            data_payload.payload = "";
            data_payload.metod = (int)connMetod.GET;
            data_payload.token = token;
            var jObject = await ProcessRequest(data_payload);
            return jObject;
        }

        public async Task<JToken> GetGroupPositions()
        {
            Payload data_payload = new Payload();
            if (userRole == 1)
            {
                data_payload.uri = proсHost + "://" + urlHost + "/user-api/GetGroupPositions";
            }
            else
            {
                data_payload.uri = proсHost + "://" + urlHost + "/admin-api/GetGroupPositions";
            }

            data_payload.payload = "";
            data_payload.metod = (int)connMetod.GET;
            data_payload.token = token;
            var jObject = await ProcessRequest(data_payload);
            return jObject;
        }

        public async Task<JToken> GetLogs()
        {
            Payload data_payload = new Payload();
            if (userRole == 1)
            {
                data_payload.uri = proсHost + "://" + urlHost + "/user-api/GetLogs";
            }
            else
            {
                data_payload.uri = proсHost + "://" + urlHost + "/admin-api/GetLogs";
            }

            data_payload.payload = "";
            data_payload.metod = (int)connMetod.GET;
            data_payload.token = token;
            var jObject = await ProcessRequest(data_payload);
            return jObject;
        }

        public async Task<JToken> GetMesssages()
        {
            Payload data_payload = new Payload();
            if (userRole == 1)
            {
                data_payload.uri = proсHost + "://" + urlHost + "/user-api/GetMesssages";
            }
            else
            {
                data_payload.uri = proсHost + "://" + urlHost + "/admin-api/GetMesssages";
            }

            data_payload.payload = "";
            data_payload.metod = (int)connMetod.GET;
            data_payload.token = token;
            var jObject = await ProcessRequest(data_payload);
            return jObject;
        }
        

        public async Task<string> Ping()
        {
            Payload data_payload = new Payload();
            data_payload.uri = proсHost + "://" + urlHost + "/admin-api/Ping";
            data_payload.payload = "";
            data_payload.metod = (int)connMetod.GET;
            data_payload.token = token;

            string strOut = "";

            var jObject = await ProcessRequest(data_payload);
            if (jObject == null)
            {
                strOut = "Ошибка соединения!";
            }
            else
            {
                strOut = jObject["message"].ToString();
                string[] words = strOut.Split(' ');

                int i = 0;
                foreach (var word in words)
                {
                    //Console.WriteLine($"<{word}>");
                    i++;
                }
                strOut = words[i - 2] + " " + words[i - 1];
            }
            return strOut;
        }

        public async Task<string> Ping(string urlHost)
        {
            Payload data_payload = new Payload();
            data_payload.uri = proсHost + "://" + urlHost + "/admin-api/Ping";
            data_payload.payload = "";
            data_payload.metod = (int)connMetod.GET;
            data_payload.token = token;

            string strOut = "";

            var jObject = await ProcessRequest(data_payload);
            if (jObject == null)
            {
                strOut = "Ошибка соединения!";
            }
            else
            {
                strOut = jObject["message"].ToString();
                string[] words = strOut.Split(' ');

                int i = 0;
                foreach (var word in words)
                {
                    //Console.WriteLine($"<{word}>");
                    i++;
                }
                strOut = words[i - 2] + " " + words[i - 1];
            }
            return strOut;
        }

        private async Task<JToken> ProcessRequest(Payload payload)
        {
            string xjson = "";
            var request = new HttpRequestMessage();
            HttpContent c = new StringContent(payload.payload, Encoding.UTF8, "application/json");
            JToken jObject = null;

            if (payload.uri == null)
            {
                return jObject;
            }
            request.RequestUri = new Uri(payload.uri);
            if (payload.metod == (int)connMetod.POST) // Метод POST
            {
                request.Method = HttpMethod.Post;
                request.Content = c;
            }
            else if (payload.metod == (int)connMetod.GET) // Метод GET
            {
                request.Method = HttpMethod.Get;
            }
            else // Другие методы
            {
                request.Method = HttpMethod.Get;
            }

            try
            {
                var client = new HttpClient();
                if (token != "")
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token);
                }


                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.SendAsync(request);

                switch (response.StatusCode)
                {
                    case (System.Net.HttpStatusCode.OK):
                        HttpContent content = response.Content;
                        xjson = await content.ReadAsStringAsync();
                        jObject = JToken.Parse(xjson);
                        break;

                    default:

                        break;
                }
            }
            catch (Exception ex) 
            {
                jObject = null;
            }

            //Console.WriteLine(xjson);
            return jObject;

        }

        private async Task<byte[]> ProcessRequestFile(Payload payload)
        {
            byte[] fileByte = null;

            var request = new HttpRequestMessage();
            HttpContent c = new StringContent(payload.payload, Encoding.UTF8, "application/json");
            JToken jObject = null;

            if (payload.uri == null)
            {
                return fileByte;
            }
            request.RequestUri = new Uri(payload.uri);
            if (payload.metod == (int)connMetod.POST) // Метод POST
            {
                request.Method = HttpMethod.Post;
                request.Content = c;
            }
            else if (payload.metod == (int)connMetod.GET) // Метод GET
            {
                request.Method = HttpMethod.Get;
            }
            else // Другие методы
            {
                request.Method = HttpMethod.Get;
            }

            try
            {
                var client = new HttpClient();
                if (token != "")
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token);
                }


                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.SendAsync(request);

                switch (response.StatusCode)
                {
                    case (System.Net.HttpStatusCode.OK):
                        HttpContent content = response.Content;
                        fileByte = await content.ReadAsByteArrayAsync();
                        
                        break;

                    default:

                        break;
                }
            }
            catch (Exception ex)
            {
                fileByte = null;
            }

            //Console.WriteLine(xjson);
            return fileByte;

        }

        private async Task<JToken> ProcessRequestHead(Payload payload)
        {
            string xjson = "";
            string xjson2 = "";
            string xjson3 = "";

            var request = new HttpRequestMessage();
            HttpContent c = new StringContent(payload.payload, Encoding.UTF8, "application/json");
            JToken jObject = null;

            if (payload.uri == null)
            {
                return jObject;
            }
            request.RequestUri = new Uri(payload.uri);
            if (payload.metod == (int)connMetod.POST) // Метод POST
            {
                request.Method = HttpMethod.Post;
                request.Content = c;
            }
            else if (payload.metod == (int)connMetod.GET) // Метод GET
            {
                request.Method = HttpMethod.Get;
            }
            else // Другие методы
            {
                request.Method = HttpMethod.Get;
            }

            try
            {
                var client = new HttpClient();
                if (token != "")
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token);
                }


                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.SendAsync(request);

                switch (response.StatusCode)
                {
                    case (System.Net.HttpStatusCode.OK):
                        HttpContent content = response.Content;
                        xjson = await content.ReadAsStringAsync();
                       

                        if (response.Headers.Contains("pageheader"))
                        {
                            xjson2 = response.Headers.GetValues("pageheader").First().ToString();

                            //do something with the header value
                        }
                        xjson3 = "{ \"Head\":" + xjson2 + ", \"Data\": " + xjson + "  }";
                        jObject = JToken.Parse(xjson3);
                        break;

                    default:

                        break;
                }
            }
            catch (Exception ex)
            {
                jObject = null;
            }

            //Console.WriteLine(xjson);
            return jObject;

        }

        private async Task<JToken> ProcessRequestForm(Payload payload, List<Payload_files> payfile)
        {
            string xjson = "";

            JToken jObject = null;
            var request = new HttpRequestMessage();

            request.RequestUri = new Uri(payload.uri);
            request.Method = HttpMethod.Post;

            HttpContent c = new StringContent(payload.payload, Encoding.UTF8, "application/json");

            var formContent = new MultipartFormDataContent();

            formContent.Add(c, "Question");

            if (payfile != null)
            { 
                foreach (var file in payfile)
                {
                    byte[] fileToBytes = File.ReadAllBytes(file.filePath);
                    // формируем отправляемое содержимое
                    var content = new ByteArrayContent(fileToBytes);
                    // Устанавливаем заголовок Content-Type
                    // Добавляем загруженный файл в MultipartFormDataContent
                    formContent.Add(content, name: "files", fileName: file.name);
                }
            }

            request.Content = formContent;


            try
            {
                var client = new HttpClient();
                if (token != "")
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token);
                }


                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("multipart/form-data"));

                HttpResponseMessage response = await client.PostAsync(request.RequestUri, formContent);

                switch (response.StatusCode)
                {
                    case (System.Net.HttpStatusCode.OK):
                        HttpContent content = response.Content;
                        xjson = await content.ReadAsStringAsync();
                        jObject = JToken.Parse(xjson);
                        break;

                    default:
                        MessageBox.Show("КОД ОТВЕТА СЕРВЕРА: " + (int)response.StatusCode);
                        break;
                }
            }
            catch (Exception ex)
            {
                jObject = null;
            }
            Console.WriteLine(xjson);
            return jObject;

        }

        public class Payload
        {
            public string uri { get; set; }
            public string payload { get; set; }
            public int metod { get; set; }
            public string token { get; set; }
        }

        public class Payload_files
        {
            public string name { get; set; }
            public string filePath { get; set; }
        }
    }
}
