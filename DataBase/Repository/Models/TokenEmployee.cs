using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Repository.Models;

[Table("TokenEmployee")]
[Index("Token", Name = "UQ__TokenEmp__CA90DA7AB8DF8C27", IsUnique = true)]
public partial class TokenEmployee
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("idEmployee")]
    [StringLength(50)]
    public string? IdEmployee { get; set; }

    [Column("token")]
    [StringLength(50)]
    public string? Token { get; set; }

    [Column("issuingTime", TypeName = "datetime")]
    public DateTime? IssuingTime { get; set; }

    [Column("state")]
    public bool? State { get; set; }

    [ForeignKey("IdEmployee")]
    [InverseProperty("TokenEmployees")]
    public virtual Employee? IdEmployeeNavigation { get; set; }
}
