using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Repository.Models;

[Table("EmployeeResult")]
public partial class EmployeeResult
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("idResult")]
    [StringLength(50)]
    public string? IdResult { get; set; }

    [Column("scoreFrom")]
    public int? ScoreFrom { get; set; }

    [Column("scoreTo")]
    public int? ScoreTo { get; set; }

    [Column("idEmployee")]
    [StringLength(50)]
    public string? IdEmployee { get; set; }

    [ForeignKey("IdEmployee")]
    [InverseProperty("EmployeeResults")]
    public virtual Employee? IdEmployeeNavigation { get; set; }

    [ForeignKey("IdResult")]
    [InverseProperty("EmployeeResults")]
    public virtual Result? IdResultNavigation { get; set; }
}
