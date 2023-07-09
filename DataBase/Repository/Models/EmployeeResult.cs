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
    [StringLength(50)]
    public string Id { get; set; } = null!;

    [Column("idResult")]
    [StringLength(50)]
    public string? IdResult { get; set; }

    [Column("idEmployee")]
    public int? IdEmployee { get; set; }

    [Column("startDate")]
    public DateOnly? StartDate { get; set; }

    [Column("startTime")]
    public TimeOnly? StartTime { get; set; }

    [Column("duration")]
    public byte? Duration { get; set; }

    [Column("endTime")]
    public TimeOnly? EndTime { get; set; }

    [ForeignKey("IdEmployee")]
    [InverseProperty("EmployeeResults")]
    public virtual Employee? IdEmployeeNavigation { get; set; }

    [ForeignKey("IdResult")]
    [InverseProperty("EmployeeResults")]
    public virtual Result? IdResultNavigation { get; set; }
}
