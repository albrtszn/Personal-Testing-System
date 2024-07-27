using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Client.classDTO
{
    public class OneQuestionEdit
    {
        public string IdQuestion { get; set; }
        public string IdTest { get; set; }
        public string Text { get; set; }
        public string ImagePath { get; set; }
        public int IdQuestionType { get; set; }
        public AnswerDto[] Answers { get; set; }
        public int Weight { get; set; }
        public int Number { get; set; }
        

    }
}
