﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Repository.Models;

[Table("Answer")]
public partial class Answer
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("text")]
    [StringLength(500)]
    public string? Text { get; set; }

    [Column("idQuestion")]
    [StringLength(50)]
    public string? IdQuestion { get; set; }

    [Column("correct")]
    public bool? Correct { get; set; }

    [Column("number")]
    public int? Number { get; set; }

    [Column("imagePath")]
    [StringLength(150)]
    public string? ImagePath { get; set; }

    [Column("weight")]
    public int? Weight { get; set; }

    [InverseProperty("IdAnswerNavigation")]
    public virtual ICollection<EmployeeAnswer> EmployeeAnswers { get; set; } = new List<EmployeeAnswer>();

    [ForeignKey("IdQuestion")]
    [InverseProperty("Answers")]
    public virtual Question? IdQuestionNavigation { get; set; }
}
