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

    [Column("idResult")]
    [StringLength(50)]
    public string? IdResult { get; set; }

    [ForeignKey("IdAnswer")]
    [InverseProperty("EmployeeAnswers")]
    public virtual Answer? IdAnswerNavigation { get; set; }

    [ForeignKey("IdResult")]
    [InverseProperty("EmployeeAnswers")]
    public virtual Result? IdResultNavigation { get; set; }
}
