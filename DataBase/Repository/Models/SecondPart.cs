using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Repository.Models;

[Table("SecondPart")]
[Index("IdFirstPart", Name = "UQ__SecondPa__7D5B4DC3002F5B1D", IsUnique = true)]
public partial class SecondPart
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("text")]
    [StringLength(500)]
    public string? Text { get; set; }

    [Column("idFirstPart")]
    public int? IdFirstPart { get; set; }

    [InverseProperty("IdSecondPartNavigation")]
    public virtual ICollection<EmployeeMatching> EmployeeMatchings { get; set; } = new List<EmployeeMatching>();

    [ForeignKey("IdFirstPart")]
    [InverseProperty("SecondPart")]
    public virtual FirstPart? IdFirstPartNavigation { get; set; }
}
