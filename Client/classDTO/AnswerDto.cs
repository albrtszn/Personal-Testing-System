using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.classDTO
{
    public class AnswerDto
    {
        public int IdAnswer { get; set; }
        public int Number { get; set; }
        public string Text { get; set; }
        public int Weight { get; set; }
        public string IdQuestion { get; set; }
        public bool Correct { get; set; }
        public string ImagePath { get; set; }
    }
}
