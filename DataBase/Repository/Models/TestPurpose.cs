using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Repository.Models;

[Table("TestPurpose", Schema = "fitpsuon_fitpsuon")]
public partial class TestPurpose
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("idEmployee")]
    [StringLength(50)]
    public string? IdEmployee { get; set; }

    [Column("idTest")]
    [StringLength(50)]
    public string? IdTest { get; set; }

    [Column("datatimePurpose", TypeName = "datetime")]
    public DateTime? DatatimePurpose { get; set; }

    [ForeignKey("IdEmployee")]
    [InverseProperty("TestPurposes")]
    public virtual Employee? IdEmployeeNavigation { get; set; }

    [ForeignKey("IdTest")]
    [InverseProperty("TestPurposes")]
    public virtual Test? IdTestNavigation { get; set; }
}
