using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Client.classDTO
{
    public class OneTest
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Weight { get; set; }
        public string Description { get; set; }
        public string Instruction { get; set; }
        public int CompetenceId { get; set; }
        public OneQuestion[] Questions { get; set; }


    }
}