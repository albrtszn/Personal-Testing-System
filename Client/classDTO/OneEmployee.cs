﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Client.forms.addSubdivision;

namespace Client.classDTO
{
    public class OneEmployee
    {
        public string Id {  get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string LastName { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string DateOfBirth { get; set; }
        public string Phone { get; set; }
        public string RegistrationDate { get; set; }
        public OneSubdivision Subdivision { get; set; }
    }
}