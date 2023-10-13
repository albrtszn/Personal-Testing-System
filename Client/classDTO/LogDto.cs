using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.classDTO
{
    public class LogDto
    {
        public int Id { get; set; }
        public string UrlPath { get; set; }
        public string UserId { get; set; }
        public string UserIp { get; set; }
        public string DataTime { get; set; }
        public string Params { get; set; }
    }
}
