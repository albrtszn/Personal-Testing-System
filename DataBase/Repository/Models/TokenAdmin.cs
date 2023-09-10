using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Repository.Models;

[Table("TokenAdmin", Schema = "fitpsuon_fitpsuon")]
[Index("Token", Name = "UQ__TokenAdm__CA90DA7A4CA68350", IsUnique = true)]
public partial class TokenAdmin
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("idAdmin")]
    [StringLength(50)]
    public string? IdAdmin { get; set; }

    [Column("token")]
    [StringLength(50)]
    public string? Token { get; set; }

    [Column("issuingTime", TypeName = "datetime")]
    public DateTime? IssuingTime { get; set; }

    [Column("state")]
    public bool? State { get; set; }

    [ForeignKey("IdAdmin")]
    [InverseProperty("TokenAdmins")]
    public virtual Admin? IdAdminNavigation { get; set; }
}
