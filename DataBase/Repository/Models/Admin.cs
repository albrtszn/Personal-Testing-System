using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Repository.Models;

[Table("Admin", Schema = "fitpsuon_fitpsuon")]
public partial class Admin
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

    [Column("idSubdivision")]
    public int? IdSubdivision { get; set; }

    [ForeignKey("IdSubdivision")]
    [InverseProperty("Admins")]
    public virtual Subdivision? IdSubdivisionNavigation { get; set; }

    [InverseProperty("IdAdminNavigation")]
    public virtual ICollection<TokenAdmin> TokenAdmins { get; set; } = new List<TokenAdmin>();
}
