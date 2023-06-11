using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Repository.Models;

[Table("EmployeeMatching")]
public partial class EmployeeMatching
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("idFirstPart")]
    [StringLength(50)]
    public string? IdFirstPart { get; set; }

    [Column("idSecondPart")]
    public int? IdSecondPart { get; set; }

    [Column("idTestResult")]
    [StringLength(50)]
    public string? IdTestResult { get; set; }

    [ForeignKey("IdFirstPart")]
    [InverseProperty("EmployeeMatchings")]
    public virtual FirstPart? IdFirstPartNavigation { get; set; }

    [ForeignKey("IdSecondPart")]
    [InverseProperty("EmployeeMatchings")]
    public virtual SecondPart? IdSecondPartNavigation { get; set; }

    [ForeignKey("IdTestResult")]
    [InverseProperty("EmployeeMatchings")]
    public virtual TestResult? IdTestResultNavigation { get; set; }
}
