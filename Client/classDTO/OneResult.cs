using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.classDTO
{
    public class OneResult
    {
        public string Id { get; set; }
        public int ScoreFrom { get; set; }
        public int ScoreTo { get; set; }
        public OneTestResult Test { get; set; }
        public string StartDate { get; set; }
        public string StartTime { get; set; }
        public int Duration { get; set; }
        public string EndTime { get; set; }
        public string Description { get; set; }

    }
}
