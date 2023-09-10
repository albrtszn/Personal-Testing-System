using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Repository.Models;

[Table("Competence", Schema = "fitpsuon_fitpsuon")]
public partial class Competence
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    [StringLength(100)]
    public string? Name { get; set; }

    [InverseProperty("IdCompetenceNavigation")]
    public virtual ICollection<Test> Tests { get; set; } = new List<Test>();
}
