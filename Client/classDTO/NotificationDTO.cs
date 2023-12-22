using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.classDTO
{
    public class NotificationDTO
    {
        public int type {  get; set; }
        public string target { get; set; }  
        public string[] arguments {  get; set; }
    
    }
}
