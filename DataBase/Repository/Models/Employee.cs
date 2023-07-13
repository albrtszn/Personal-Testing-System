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
    [StringLength(50)]
    public string Id { get; set; } = null!;

    [StringLength(50)]
    public string? FirstName { get; set; }

    [StringLength(50)]
    public string? SecondName { get; set; }

    [StringLength(50)]
    public string? LastName { get; set; }

    [Column("login")]
    [StringLength(50)]
    public string? Login { get; set; }

    [Column("password")]
    [StringLength(50)]
    public string? Password { get; set; }

    [Column("dateOfBirth")]
    public DateOnly? DateOfBirth { get; set; }

    [Column("idSubdivision")]
    public int? IdSubdivision { get; set; }

    [InverseProperty("IdEmployeeNavigation")]
    public virtual ICollection<EmployeeResult> EmployeeResults { get; set; } = new List<EmployeeResult>();

    [ForeignKey("IdSubdivision")]
    [InverseProperty("Employees")]
    public virtual Subdivision? IdSubdivisionNavigation { get; set; }

    [InverseProperty("IdEmployeeNavigation")]
    public virtual ICollection<TestPurpose> TestPurposes { get; set; } = new List<TestPurpose>();
}
