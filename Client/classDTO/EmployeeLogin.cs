using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.classDTO
{
    public class EmployeeLogin
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string LastName { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string DateOfBirth { get; set; }
        public SubdivisionDto subdivision { get; set; }
        public string Phone { get; set; }
        public string RegistrationDate { get; set; }

    }
}
