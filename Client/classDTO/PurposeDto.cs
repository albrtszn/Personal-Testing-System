using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.classDTO
{
    public class PurposeDto
    {
        public int Id { get; set; }
        public string IdEmployee { get; set; }
        public TestDto Test { get; set; }
        public string DatatimePurpose { get; set; }
        public int Timer { get; set; }
    }
}
