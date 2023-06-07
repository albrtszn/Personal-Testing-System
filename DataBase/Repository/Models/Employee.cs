using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Repository.Models;

[Table("Employee")]
public partial class Employee
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [StringLength(50)]
    public string? FirstName { get; set; }

    [StringLength(50)]
    public string? SecondName { get; set; }

    [StringLength(50)]
    public string? LastName { get; set; }

    [Column("idSubdivision")]
    public int? IdSubdivision { get; set; }

    [ForeignKey("IdSubdivision")]
    [InverseProperty("Employees")]
    public virtual Subdivision? IdSubdivisionNavigation { get; set; }

    [InverseProperty("IdEmployeeNavigation")]
    public virtual ICollection<TestPurpose> TestPurposes { get; set; } = new List<TestPurpose>();

    [InverseProperty("IdEmployeeNavigation")]
    public virtual ICollection<TestResult> TestResults { get; set; } = new List<TestResult>();
}
