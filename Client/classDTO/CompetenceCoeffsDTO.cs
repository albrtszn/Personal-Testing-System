using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.classDTO
{
    public class CompetenceCoeffsDTO
    {
        public int Id { get; set; }
        public int IdCompetence { get; set; }
        public int IdGroup { get; set; }
        public float Coefficient { get; set; }
    }
}
