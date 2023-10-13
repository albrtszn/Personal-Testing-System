using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.classDTO
{
    public class ResultDto
    {

        public int Id { get; set; }
        public int ScoreFrom { get; set; }
        public int ScoreTo { get; set; }
        public OneResult Result { get; set;}
        public OneEmployee Employee { get; set;}
    }
}
