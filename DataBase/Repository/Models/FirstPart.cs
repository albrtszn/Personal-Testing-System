using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Repository.Models;

[Table("FirstPart")]
public partial class FirstPart
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

    [InverseProperty("IdFirstPartNavigation")]
    public virtual ICollection<EmployeeMatching> EmployeeMatchings { get; set; } = new List<EmployeeMatching>();

    [ForeignKey("IdQuestion")]
    [InverseProperty("FirstParts")]
    public virtual Question? IdQuestionNavigation { get; set; }

    [InverseProperty("IdFirstPartNavigation")]
    public virtual SecondPart? SecondPart { get; set; }
}
