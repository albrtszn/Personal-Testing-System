using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Repository.Models;

[Table("SecondPart")]
[Index("IdFirstPart", Name = "UQ__SecondPa__7D5B4DC35D8C4C0E", IsUnique = true)]
public partial class SecondPart
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("text")]
    [StringLength(500)]
    public string? Text { get; set; }

    [Column("idFirstPart")]
    [StringLength(50)]
    public string? IdFirstPart { get; set; }

    [InverseProperty("IdSecondPartNavigation")]
    public virtual ICollection<EmployeeMatching> EmployeeMatchings { get; set; } = new List<EmployeeMatching>();

    [ForeignKey("IdFirstPart")]
    [InverseProperty("SecondPart")]
    public virtual FirstPart? IdFirstPartNavigation { get; set; }
}
