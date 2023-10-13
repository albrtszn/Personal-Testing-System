using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Client.classDTO
{
    public class OneQuestion
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public string ImagePath { get; set; }
        public string Base64Image { get; set; }
        public int Number { get; set; }
        public int IdQuestionType { get; set; }
        public BitmapImage Img { get; set; }
        public OneAnswer[] Answers { get; set; }
        public int PushAnswersID { get; set; }

    }
}
