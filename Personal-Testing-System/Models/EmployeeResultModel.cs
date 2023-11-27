﻿using Personal_Testing_System.DTOs;

namespace Personal_Testing_System.Models
{
    public class EmployeeResultModel
    {
        public int? Id { get; set; }
        public int? ScoreFrom { get; set; }
        public int? ScoreTo { get; set; }
        public ResultModel? Result { get; set; }
        public EmployeeModel? Employee { get; set; }
    }
}