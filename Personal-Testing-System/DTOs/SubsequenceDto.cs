﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Personal_Testing_System.DTOs
{
    public class SubsequenceDto
    {
        public int Id { get; set; }
        public string? Text { get; set; }
        public string? IdQuestion { get; set; }
        public byte? Number { get; set; }
    }
}
