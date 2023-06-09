﻿using DataBase.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.interfaces
{
    public interface IEmployeeRepo
    {
        List<Employee> GetAllEmployees();
        Employee GetEmployeeById(string id);
        void SaveEmployee(Employee EmployeeToSave);
        void DeleteEmployeeById(string id);
    }
}
