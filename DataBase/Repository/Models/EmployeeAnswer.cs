using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Repository.Models;

[Table("EmployeeAnswer", Schema = "fitpsuon_fitpsuon")]
public partial class EmployeeAnswer
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

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
