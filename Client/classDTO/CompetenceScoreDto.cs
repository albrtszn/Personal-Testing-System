using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.classDTO
{
    public class CompetenceScoreDto
    {
       public int Id {  get; set; }
       public int IdCompetence { get; set; }
       public int IdGroup {  get; set; }
       public string Name {  get; set; }
       public int NumberPoints {  get; set; }
       public string Description { get; set; }

    }
}
