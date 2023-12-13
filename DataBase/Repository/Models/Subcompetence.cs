using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Repository.Models;

[Table("Subcompetence")]
public partial class Subcompetence
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    [StringLength(500)]
    public string? Name { get; set; }

    [Column("description")]
    [StringLength(1000)]
    public string? Description { get; set; }

    [InverseProperty("IdSubcompetenceNavigation")]
    public virtual ICollection<ElployeeResultSubcompetence> ElployeeResultSubcompetences { get; set; } = new List<ElployeeResultSubcompetence>();

    [InverseProperty("IdSubcompetenceNavigation")]
    public virtual ICollection<QuestionSubcompetence> QuestionSubcompetences { get; set; } = new List<QuestionSubcompetence>();
}
