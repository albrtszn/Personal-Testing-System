using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Repository.Models;

[Table("QuestionsInTest")]
public partial class QuestionsInTest
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("idTest")]
    [StringLength(50)]
    public string? IdTest { get; set; }

    [Column("idQuestion")]
    [StringLength(50)]
    public string? IdQuestion { get; set; }

    [ForeignKey("IdQuestion")]
    [InverseProperty("QuestionsInTests")]
    public virtual Question? IdQuestionNavigation { get; set; }

    [ForeignKey("IdTest")]
    [InverseProperty("QuestionsInTests")]
    public virtual Test? IdTestNavigation { get; set; }
}
