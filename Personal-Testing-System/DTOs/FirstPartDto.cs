﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Personal_Testing_System.DTOs
{
    public class FirstPartDto
    {
        public int Id { get; set; }

        [Column("text")]
        [StringLength(500)]
        public string? Text { get; set; }

        [Column("idQuestion")]
        public int? IdQuestion { get; set; }
    }
}
