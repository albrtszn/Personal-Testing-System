using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.classDTO
{
    public class QuestionDto
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public int IdQuestionType { get; set; }
        public int Number { get; set; }
        public string IdTest { get; set; }
        public string ImagePath { get; set; }
    }
}
