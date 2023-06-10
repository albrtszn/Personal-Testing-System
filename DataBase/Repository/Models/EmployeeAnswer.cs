using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Repository.Models;

[Table("EmployeeAnswer")]
public partial class EmployeeAnswer
{
    [Key]
    [Column("id")]
    [StringLength(50)]
    public string Id { get; set; } = null!;

    [Column("idAnswer")]
    public int? IdAnswer { get; set; }

    [Column("idTestResult")]
    [StringLength(50)]
    public string? IdTestResult { get; set; }

    [ForeignKey("IdAnswer")]
    [InverseProperty("EmployeeAnswers")]
    public virtual Answer? IdAnswerNavigation { get; set; }

    [ForeignKey("IdTestResult")]
    [InverseProperty("EmployeeAnswers")]
    public virtual TestResult? IdTestResultNavigation { get; set; }
}
