using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.classDTO
{
    public class MessageDTO
    {
            public int Id { get; set; }
            public string IdEmployee {  get; set; }
            public string MessageText {  get; set; }
            public bool StatusRead { get; set; }
            public string DateAndTime {  get; set; }
            public string FIO {  get; set; }
            public string TextStatus { get; set; }
    }
}
