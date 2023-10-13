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
        

        public async Task<JToken> AddQuestionInTest(string pay_data)
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
            var jObject = await ProcessRequestForm(data_payload, null);
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

        private async Task<JToken> ProcessRequest(Payload payload)
        {
            string xjson = "";
            var request = new HttpRequestMessage();
            HttpContent c = new StringContent(payload.payload, Encoding.UTF8, "application/json");
            JToken jObject = null;

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
            Console.WriteLine(xjson);
            return jObject;

        }

        private async Task<JToken> ProcessRequestForm(Payload payload, Payload_files[] payfile )
        {
            string xjson = "";

            JToken jObject = null;
            var request = new HttpRequestMessage();

            request.RequestUri = new Uri(payload.uri);
            request.Method = HttpMethod.Post;

            HttpContent c = new StringContent(payload.payload, Encoding.UTF8, "application/json");

            var formContent = new MultipartFormDataContent();

            formContent.Add(c, "Question");

            foreach (var file in payfile) 
            {
                var fileStreamContent = new StreamContent(File.OpenRead(file.filePath));
                formContent.Add(fileStreamContent, "files", file.name);
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
            public byte[] paramFileStream { get; set; }
            public string name { get; set; }
            public string filePath { get; set; }
        }
    }
}
