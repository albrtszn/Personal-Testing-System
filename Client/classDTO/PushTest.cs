using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.classDTO
{
    public class PushTest
    {
        public string TestId { get; set; }
        public string EmployeeId { get; set; }
        public string StartDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Description { get; set; }
        public PushQuestion[] Questions { get; set; }
    }
}