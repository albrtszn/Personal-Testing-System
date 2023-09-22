using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Repository.Models;

[Table("Subdivision")]
public partial class Subdivision
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    [StringLength(120)]
    public string? Name { get; set; }

    [Column("idGroupPositions")]
    public int? IdGroupPositions { get; set; }

    [InverseProperty("IdSubdivisionNavigation")]
    public virtual ICollection<Admin> Admins { get; set; } = new List<Admin>();

    [InverseProperty("IdSubdivisionNavigation")]
    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    [ForeignKey("IdGroupPositions")]
    [InverseProperty("Subdivisions")]
    public virtual GroupPosition? IdGroupPositionsNavigation { get; set; }
}
